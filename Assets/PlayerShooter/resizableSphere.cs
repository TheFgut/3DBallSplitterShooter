using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class resizableSphere : MonoBehaviour
{
    internal float mass;

    public abstract void PumpIn(float mass);
    public abstract void PumpOut(float mass);

    public void resize(float mass)
    {
        float radius = getRadiusByMass(mass)/2f;

        transform.localScale = new Vector3(1, 1, 1) * radius;
    }

    internal float getRadiusByMass(float mass)
    {
        float one = (4f / 3f) * Mathf.PI;
        float two = mass / one;
        float radius = Mathf.Pow(two, 1f / 3f);
        return radius;
    }

}
