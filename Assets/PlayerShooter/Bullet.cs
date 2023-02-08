using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : resizableSphere
{
    private bulletParams parameters;
    public void Init(bulletParams parameters)
    {
        this.parameters = parameters;
        GetComponent<MeshRenderer>().material = parameters.projectileMaterial;
    }

    public override void PumpIn(float mass)
    {
        this.mass += mass;
        resize(this.mass);
    }

    public override void PumpOut(float mass)
    {
        this.mass -= mass;
        resize(this.mass);
    }

    public void Launch(Vector3 direction)
    {
        this.direction = direction;

        Rigidbody rig = gameObject.AddComponent<Rigidbody>();
        rig.useGravity = false;
        rig.constraints = RigidbodyConstraints.FreezeAll;
        StartCoroutine(Fly());
    }

    Vector3 direction;
    private const float dieTime = 50;
    private IEnumerator Fly()
    {
        float dieTimer = dieTime;
        do
        {
            transform.position += direction * parameters.speed * Time.fixedDeltaTime;
            dieTimer -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        } while (dieTimer > 0);
        destroy();
    }

    public void Explode()
    {

        bulletExplosion explosion = Instantiate(parameters.explodeEffect);

        float explodeRadius = getRadiusByMass(mass/parameters.minBulletSize);

        explosion.Init(explodeRadius,transform.position);

        destroy();
    }

    public float destroy()
    {
        Destroy(gameObject);
        return mass;
    }
    
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "obstacle")
        {
            obstacle obstacle = col.gameObject.GetComponent<obstacle>();
            obstacle.collide(this, col);
        }
    }

    public void Reflect(Vector3 normal)
    {
        direction = Vector3.Reflect(direction, normal);
    }
}
[System.Serializable]
public class bulletParams
{
    public bulletExplosion explodeEffect;
    public Material projectileMaterial;
    public float speed;
    public float minBulletSize;
}
