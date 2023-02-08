using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletExplosion : MonoBehaviour
{
    [SerializeField] private float defaultRadius;
    public void Init(float radius,Vector3 position)
    {
        transform.position = position;
        transform.localScale = Vector3.zero;

        Collider[] contacts = Physics.OverlapSphere(position, radius, LayerMask.GetMask("Obstacles"));
        foreach (Collider contact in contacts)
        {
            obstacle obstScr = contact.GetComponent<obstacle>();
            obstScr.Infest();
        }

        StartCoroutine(explodeProcess(radius));
    }



    private IEnumerator explodeProcess(float radius)
    {
        float maxTime = 0.3f;
        float timer = maxTime;
        Vector3 finalRadius = Vector3.one / defaultRadius * radius;
        do
        {
            transform.localScale = Vector3.Lerp(finalRadius, Vector3.zero, timer/ maxTime);
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        } while (timer > 0);
        Destroy(gameObject);
    }

}
