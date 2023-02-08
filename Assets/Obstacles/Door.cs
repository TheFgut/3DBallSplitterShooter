using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Quaternion closedRot;
    [SerializeField] private float openTime = 1.5f;
    [SerializeField] private Quaternion openedRot;
    void Start()
    {
        closedRot = transform.rotation;
    }

    [ContextMenu("setOpenRot")]
    private void setOpenRot()
    {
        openedRot = transform.rotation;
    }

    private bool opened;
    public void Open()
    {
        if(opened == true)
        {
            return;
        }
        StartCoroutine(openProcess());
        opened = true;
    }

    IEnumerator openProcess()
    {
        float timer = openTime;
        do
        {
            timer -= Time.deltaTime;
            transform.rotation = Quaternion.Lerp(openedRot, closedRot, timer/openTime );
            yield return new WaitForEndOfFrame();
        } while (timer > 0);
    }
}
