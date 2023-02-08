using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reflectObstacle : obstacle
{
    public override void collide(Bullet bullet, Collision col)
    {
        ContactPoint[] contacs = col.contacts;
        Vector3 normal = new Vector3();
        for (int i =0; i < contacs.Length;i++)
        {
            normal += contacs[i].normal;
        }
        normal.y = 0;
        normal.Normalize();
        bullet.Reflect(normal);
    }

    public override void Infest()
    {
        
    }

}
