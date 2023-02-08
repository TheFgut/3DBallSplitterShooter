using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetFramerate : MonoBehaviour
{
    [SerializeField]private int targetFrameRate = 60;
    void Start()
    {
        Application.targetFrameRate = targetFrameRate;
    }

}
