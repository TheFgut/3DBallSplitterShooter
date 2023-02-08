using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class playerShooter
{
    [SerializeField] private float massTransferSpeed;

    [SerializeField] private bulletParams bulletInitSettings;
    private Player player;
    private float bulletMoveRadius;
    public void Init(Player player,float radiusOfPlayer)
    {
        this.player = player;
        bulletMoveRadius = radiusOfPlayer / 2f;
    }

    private Coroutine aimProcess;
    private Bullet bulletToShoot;
    public void startAiming(Vector3 target)
    {
        if (aimProcess != null)
        {
            return;
        }
        //making bullet
        GameObject bulletObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bulletToShoot = bulletObj.AddComponent<Bullet>();
        bulletToShoot.Init(bulletInitSettings);
        player.PumpOut(bulletInitSettings.minBulletSize);
        bulletToShoot.PumpIn(bulletInitSettings.minBulletSize);
        //start aim
        aimProcess = player.StartCoroutine(aimRoutine());
        updateAimTarget(target);
    }

    private Vector3 aimTarget;
    public void updateAimTarget(Vector3 newTarget)
    {
        if (aimProcess == null)
        {
            return;
        }
        Vector3 pos = player.transform.position +
            (targetToDirection(newTarget) * bulletMoveRadius);
        bulletToShoot.transform.position = pos;

        updateBulletYPos();
        aimTarget = newTarget;
    }

    private void updateBulletYPos()
    {
        Vector3 prevPos = bulletToShoot.transform.position;
        prevPos.y = bulletToShoot.getRadiusByMass(bulletToShoot.mass) / 4;
        bulletToShoot.transform.position = prevPos;
    }


    public void Shoot(Vector3 target)
    {
        if (aimProcess == null)
        {
            return;
        }
        bulletToShoot.Launch(targetToDirection(target));

        stopAiming();
    }



    public void CancelShoot()
    {
        if (aimProcess == null)
        {
            return;
        }
        float massReturn = bulletToShoot.destroy();
        player.PumpIn(massReturn);

        stopAiming();
    }

    private void stopAiming()
    {
        player.StopCoroutine(aimProcess);
        aimProcess = null;
        bulletToShoot = null;
    }

    private Vector3 targetToDirection(Vector3 target)
    {
        Vector3 dir = target - player.transform.position;
        dir.Normalize();
        dir.y = 0;
        return dir;
    }

    IEnumerator aimRoutine()
    {
        do
        {
            increaseBulletPower();
            yield return new WaitForEndOfFrame();
        } while (true);
    }

    private void increaseBulletPower()
    {
        if (aimProcess == null)
        {
            return;
        }

        float transferMass = massTransferSpeed * Time.deltaTime;
        player.PumpOut(transferMass);
        bulletToShoot.PumpIn(transferMass);

        updateBulletYPos();
    }
}
