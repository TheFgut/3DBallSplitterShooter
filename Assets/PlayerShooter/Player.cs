using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : resizableSphere
{
    [SerializeField] private float startMass;
    [SerializeField] private playerShooter shooter;
    [SerializeField] private PlayerController controls;
    [SerializeField] private simpleCamera cam;
    [SerializeField] private Road road;

    public override void PumpIn(float mass)
    {
        this.mass += mass;
        resize(this.mass);
        road.updateRoadWidth(getRadiusByMass(this.mass) / 2);
    }

    public override void PumpOut(float mass)
    {
        this.mass -= mass;
        if (this.mass < startMass * 0.2f)
        {
            UI_manager.manager.Loose();
        }
        resize(this.mass);
        road.updateRoadWidth(getRadiusByMass(this.mass) / 2);
    }

    // Start is called before the first frame update
    void Start()
    {
        mass = startMass;
        resize(mass);

        float radius = getRadiusByMass(mass);

        controls.Init(this, shooter);
        shooter.Init(this, radius);

        road.updateRoadWidth(radius/2);
        road.connectPlayer(this);
    }

    public void moveTo(Vector3 position,voidDelegate reachDestCallback)
    {

        Vector3 direction = position - transform.position;

        controls.Deactivate();
        StartCoroutine(movementAtPosition(position, reachDestCallback));
    }

    IEnumerator movementAtPosition(Vector3 position, voidDelegate reachDestCallback)
    {
        Vector3 startPos = transform.position;

        float transitionTime = 1;
        float transitionTimer = transitionTime;
        do
        {
            transitionTimer -= Time.fixedDeltaTime;
            lerpMove(position, startPos, transitionTimer / transitionTime);
            yield return new WaitForFixedUpdate();
        } while (transitionTimer > 0);

        reachDestCallback();
        controls.Activate();
    }

    private void lerpMove(Vector3 start, Vector3 destination, float coef)
    {
        transform.position = Vector3.Lerp(start, destination, coef);
    }
}
