using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleCamera : MonoBehaviour
{
    [SerializeField]private Transform followTransform;

    private Vector3 idleDist;
    void Start()
    {
        idleDist = transform.position - followTransform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position,
            followTransform.position + idleDist,Time.fixedDeltaTime);

    }
}
