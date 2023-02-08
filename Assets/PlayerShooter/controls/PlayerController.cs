using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class PlayerController : MonoBehaviour, IPointerDownHandler,
    IDragHandler,IPointerUpHandler,IPointerExitHandler
{
    private Player player;
    private playerShooter shooter;
    private Camera cam;
    public void Init(Player player, playerShooter shooter)
    {
        this.player = player;
        this.shooter = shooter;
        cam = Camera.main;
    }
   
    //pointer events
    public void OnPointerDown(PointerEventData eventData)
    {
        if(active == false)
        {
            return;
        }
        shooter.startAiming(getRealWorldPointerPoint(eventData.position));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        shooter.Shoot(getRealWorldPointerPoint(eventData.position));
        prevRealWorldPointerPos = new Vector3(0, 0, 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        shooter.CancelShoot();
        prevRealWorldPointerPos = new Vector3(0,0,0);
    }

    public void OnDrag(PointerEventData eventData)
    {    
        shooter.updateAimTarget(getRealWorldPointerPoint(eventData.position));
    }

    //get real wold point

    private Vector3 prevRealWorldPointerPos;
    public Vector3 getRealWorldPointerPoint(Vector2 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Ground")))
        {
            prevRealWorldPointerPos = hit.point;
            return hit.point;
        }
        else
        {
            return prevRealWorldPointerPos;
        }
    }

    //activation
    private bool active = true;
    public void Deactivate()
    {
        active = false;
        shooter.CancelShoot();
    }

    public void Activate()
    {
        active = true;
    }
}
