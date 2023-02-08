using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : obstacle
{

    [SerializeField]private infestation infestationParams;
    private idleAnimation idleAnim;
    void Start()
    {
        idleAnim = new idleAnimation(transform);
        infestationParams.setObjectToInfest(this);
    }
    void Update()
    {
        idleAnim.Animate();
    }


    public override void collide(Bullet bullet, Collision col)
    {
        bullet.Explode();
    }

    public override void Infest()
    {
        infestationParams.startInfestation();
    }

    [System.Serializable]
    private class infestation
    {
        private const float minInfestTime = 0.2f;
        private const float maxInfestTime = 0.4f;

        private Coroutine infestRoutine;
        private Enemy objectToInfest;
        [SerializeField]private ParticleSystem destroyEffect;
        public void setObjectToInfest(Enemy objectToInfest)
        {
            this.objectToInfest = objectToInfest;
        }
        private IEnumerator infestProcess()
        {
            Material obstacleMaterial = objectToInfest.GetComponent<MeshRenderer>().material;
            Color defaultColor = obstacleMaterial.color;

            float time = Random.Range(minInfestTime, maxInfestTime);
            float timer = time;
            do
            {
                timer -= Time.deltaTime;

                obstacleMaterial.color = Color.Lerp(Color.red, defaultColor, timer / time);
                yield return new WaitForEndOfFrame();
            } while (timer > 0);

            objectToInfest.dieCallback();

            Die();
        }

        public void startInfestation()
        {
            if (infestRoutine != null)
            {
                return;
            }
            infestRoutine = objectToInfest.StartCoroutine(infestProcess());
        }

        private void Die()
        {
            ParticleSystem effect = Object.Instantiate(destroyEffect);
            destroyEffect = effect;
            destroyEffect.transform.position = objectToInfest.transform.position;
            Destroy(objectToInfest.gameObject);
        }
    }

    internal void dieCallback()
    {
        if (roadAffect != null)
        {
            roadAffect();
        }
    }

    private voidDelegate roadAffect;
    public bool ConnectToRoad(voidDelegate destroyFunc)
    {
        if(roadAffect != null)
        {
            return false;
        }
        roadAffect = destroyFunc;
        return true;
    }


    class idleAnimation
    {
        private float coef;
        private Transform transform;

        private Vector3 downPos;
        private Vector3 upPos;

        private const float jumpHeigth = 0.15f;
        public idleAnimation(Transform transform)
        {
            this.transform = transform;
            downPos = transform.position;
            upPos = downPos + new Vector3(0,jumpHeigth,0);
            coef = Random.Range(0, 1f);
            sign = Random.Range(1, 2.5f);
        }

        float sign;
        public void Animate()
        {
            coef += Time.deltaTime * sign;
            if(coef < 0)
            {
                coef = 0;
                sign = -sign;
            }
            else if (coef > 1)
            {
                coef = 1;
                sign = -sign;
            }

            transform.position = Vector3.Lerp(downPos, upPos, coef);
        }
    }
}

public delegate void voidDelegate();