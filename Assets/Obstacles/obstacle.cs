using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class obstacle : MonoBehaviour
{
    public abstract void Infest();

    public abstract void collide(Bullet bullet, Collision col);
}
