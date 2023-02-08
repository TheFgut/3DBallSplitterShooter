using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class staticObstacle : obstacle
{
    public override void collide(Bullet bullet, Collision col)
    {
        bullet.Explode();
    }

    public override void Infest()
    {
    }


}


