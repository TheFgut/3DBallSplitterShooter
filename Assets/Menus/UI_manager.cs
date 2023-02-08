using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_manager : menu
{
    [SerializeField] private PlayerController controlsMenu;
    [SerializeField] private winMenu restartMenu;
    public static UI_manager manager;

    void Start()
    {
        manager = this;
    }
    public void Win()
    {
        restartMenu.startAppear("You are winner!",Color.green);
        deactivateControls();
    }

    public void Loose()
    {
        restartMenu.startAppear("Game over!",Color.red);
        deactivateControls();
    }

    private void deactivateControls()
    {
        controlsMenu.Deactivate();
        controlsMenu.gameObject.SetActive(false);
    }
}
