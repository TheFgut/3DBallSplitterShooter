using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class winMenu : menu
{
    [SerializeField] private menu_graphics graphics;
    [SerializeField] private Text message;
    // Start is called before the first frame update
    void Start()
    {
        graphics.initFadeElements();
    }

    public void startAppear(string text,Color col)
    {
        col.a = 0;
        message.color = col;

        message.text = text;
        StartCoroutine(graphics.menuAppear());
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }



    [System.Serializable]
    private class menu_graphics
    {
        [SerializeField] private Transform[] elements;
        private transparencyRegulator[] fadeElements;


        public void setTransparency(float a)
        {
            for (int i = 0; i < fadeElements.Length; i++)
            {
                fadeElements[i].SetTransparency(a);
            }
        }

        public void allActivate()
        {
            for (int i = 0; i < fadeElements.Length; i++)
            {
                fadeElements[i].Activate();
            }
        }

        public void initFadeElements()
        {
            fadeElements = new transparencyRegulator[elements.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i].GetComponent<Button>() != null)
                {
                    fadeElements[i] = new button_ui(elements[i]);
                }
                else
                {
                    fadeElements[i] = new images_text_ui(elements[i]);
                }
            }
        }

        public IEnumerator menuAppear()
        {
            float time = 1f;
            float timer = time;
            do
            {
                timer -= Time.deltaTime;
                setTransparency(1 - (timer / time));
                yield return new WaitForEndOfFrame();
            } while (timer > 0);
            allActivate();
        }
    }
}
