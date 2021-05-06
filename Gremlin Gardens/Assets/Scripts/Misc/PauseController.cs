using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] private SettingsMenu settingsMenu;
    public bool paused;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel")){
            settingsMenu.ToggleSettingsMenu();
        }
        paused = settingsMenu.paused;
    }
}
