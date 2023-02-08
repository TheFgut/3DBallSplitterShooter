using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class menu : MonoBehaviour
{

    public interface transparencyRegulator
    {
        public void SetTransparency(float a);

        public void Activate();
    }

    public class button_ui : transparencyRegulator
    {
        private Button[] buttons;
        private images_text_ui images;
        public button_ui(Transform button)
        {
            buttons = button.GetComponentsInChildren<Button>();
            setButsActive(false);
            images = new images_text_ui(button);
            SetTransparency(0);
        }

        public void SetTransparency(float a)
        {
            for (int i = 0; i < buttons.Length;i++)
            {
                setButTransparency(buttons[i], a);
            }
            images.SetTransparency(a);
        }

        private void setButTransparency(Button but, float a)
        {
            ColorBlock colors = but.colors;
            colors.colorMultiplier = a;
        }

        private void setButsActive(bool active)
        {
            foreach (Button but in buttons)
            {
                but.interactable = active;
            }
        }

        public void Activate()
        {
            setButsActive(true);
        }
    }

    public class images_text_ui : transparencyRegulator
    {
        private Image[] images;
        private Text[] text;
        public images_text_ui(Transform parent)
        {
            images = parent.GetComponentsInChildren<Image>();
            text = parent.GetComponentsInChildren<Text>();
            SetTransparency(0);
        }

        public void SetTransparency(float a)
        {
            for(int i = 0; i < images.Length; i++)
            {
                Color changedColor = images[i].color;
                changedColor.a = a;
                images[i].color = changedColor;
            }

            for (int i = 0; i < text.Length; i++)
            {
                Color changedColor = text[i].color;
                changedColor.a = a;
                text[i].color = changedColor;
            }
        }
        public void Activate()
        {
        }
    }

}
