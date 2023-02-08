using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] dots;
    [SerializeField] private UI_manager winMenu;
    [SerializeField] private Door finalDoor;
    private roadPart[] roadParts;

    [ContextMenu("init road")]
    private void initRoad()
    {
        roadParts = new roadPart[dots.Length];
        for (int i = 0; i < dots.Length - 1;i++)
        {
            Vector3 way = dots[i + 1].transform.position - dots[i].transform.position;
            dots[i].transform.rotation = Quaternion.Euler(90, 
                -Vector2.SignedAngle(new Vector2(0,1), new Vector2(way.x,way.z)), 0);


            Transform[] children = dots[i].GetComponentsInChildren<Transform>();

            roadParts[i] = new roadPart(dots[i], 
                children[1].GetComponent<SpriteRenderer>(), way.magnitude, dots[i + 1].transform);
        }
        int lastId = dots.Length - 1;
        roadParts[lastId] = new roadPart(dots[lastId],winMenu);

        updateRoadWidth(1);

        roadParts[0].connectFreeWayListener(PlayerWayFree);
    }

    private Player player;
    public void connectPlayer(Player player)
    {
        this.player = player;
    }

    void Start()
    {
        initRoad();
    }

    public void updateRoadWidth(float width)
    {

        for (int i = 0; i < roadParts.Length;i++)
        {
            roadParts[i].setWidth(width);
        }

    }

    private int playerPos;
    public void PlayerWayFree()
    {
        
        player.moveTo(roadParts[playerPos + 1].getPos(), checkRoadBlocked);
        roadParts[playerPos + 1].connectFreeWayListener(PlayerWayFree);
        playerPos++;
        if (playerPos == roadParts.Length - 1)
        {
            finalDoor.Open();
        }
    }

    private void checkRoadBlocked()
    {
        int nextId = playerPos;
        if (nextId >= roadParts.Length)
        {
            return;
        }

        if (roadParts[nextId].isFree())
        {
            PlayerWayFree();
        }
    }

    private class roadPart
    {
        private SpriteRenderer circle;
        private SpriteRenderer line;
        private Transform nextRoadPart;
        private float length;

        private bool final;

        private int obstaclesCount;
        private int obstaclesCountStartCount;
        public roadPart(SpriteRenderer circle, SpriteRenderer line, float length, Transform nextRoadPart)
        {
            this.circle = circle;
            this.line = line;
            this.length = length;
            this.nextRoadPart = nextRoadPart;

            float playerStartRadius = 1.5f;
            Vector3 distance = nextRoadPart.transform.position - circle.transform.position;
            Vector3 center = (circle.transform.position + nextRoadPart.transform.position)/2;
            Vector3 halfExtends = new Vector3(playerStartRadius,3,distance.magnitude/2);

            Collider[] obstacles = Physics.OverlapBox(center, halfExtends,
                Quaternion.Euler(0, circle.transform.rotation.y,0),LayerMask.GetMask("Obstacles"));
            obstaclesCount = 0;
            for (int i = 0; i < obstacles.Length; i++)
            {
                Enemy obstacleScr = obstacles[i].GetComponent<Enemy>();
                if (obstacleScr.ConnectToRoad(obstacleDestroyed))
                {
                    obstaclesCount++;
                }   
            }
            obstaclesCountStartCount = obstaclesCount;
        }

        private UI_manager win_menu;
        public roadPart(SpriteRenderer circle, UI_manager win_menu)
        {
            this.circle = circle;
            this.win_menu = win_menu;
            final = true;
        }

        public void setWidth(float width)
        {
            if (final == true)
            {
                return;
            }
            circle.transform.localScale = new Vector3(width, width, width);
            line.transform.localScale = new Vector3(1, length / width, 1);
            line.transform.position = (circle.transform.position + nextRoadPart.position)/2
                + new Vector3(0, -0.02f, 0);

        }

        public void obstacleDestroyed()
        {
            obstaclesCount--;

            Color col = Color.Lerp(Color.green, Color.white, obstaclesCount / (float)obstaclesCountStartCount);
            circle.color = col;
            line.color = col;

            if(obstaclesCount <= 0 && freeWayCallBack != null)
            {
                freeWayCallBack();
                freeWayCallBack = null;
            }
        }
        private voidDelegate freeWayCallBack;
        public void connectFreeWayListener(voidDelegate freeWayCallBack)
        {
            if(final == true)
            {
                return;
            }
            this.freeWayCallBack = freeWayCallBack;
        }

        public Vector3 getPos()
        {
            return circle.transform.position;
        }

        public bool isFree()
        {
            if (final == true)
            {
                win_menu.Win();
                return false;
            }
            if (obstaclesCount <= 0)
            {
                return true;
            }
            return false;
        }
    }

}
