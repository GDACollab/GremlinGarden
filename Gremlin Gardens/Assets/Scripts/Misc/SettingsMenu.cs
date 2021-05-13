using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Settings")]


    [Header("References")]

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup audioMixerGroup;

    public AudioSource[] uiSounds;
    public PlayerMovement player;
    public bool paused = false;
    private bool toggleOptions = false;

    [Header("Volume Slider")]
    public GameObject lowIconVol;
    public GameObject midIconVol;
    public GameObject highIconVol;
    public Slider VolumeSlider;

    [Header("Sens Slider")]
    public GameObject lowIconSens;
    public GameObject midIconSens;
    public GameObject highIconSens;
    public Slider SensSlider;

    [Header("Screen Modes")]
    public Toggle windowedToggle;
    public Toggle fullscreenToggle;

    public GameObject settingsMenu;
    public GameObject Canvas;

    public void Awake()
    {
        uiSounds = GameObject.Find("UI Sounds").GetComponents<AudioSource>();
        if (GameObject.Find("Player")) // This is so it doesn't break races.
        {
            player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        }
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20);
        if (volume < 0.33f)
        {
            lowIconVol.SetActive(true);
            midIconVol.SetActive(false);
            highIconVol.SetActive(false);
            VolumeSlider.handleRect = lowIconVol.GetComponent<RectTransform>();
        }
        else if (volume < 0.66f)
        {
            lowIconVol.SetActive(false);
            midIconVol.SetActive(true);
            highIconVol.SetActive(false);
            VolumeSlider.handleRect = midIconVol.GetComponent<RectTransform>();
        }
        else
        {
            lowIconVol.SetActive(false);
            midIconVol.SetActive(false);
            highIconVol.SetActive(true);
            VolumeSlider.handleRect = highIconVol.GetComponent<RectTransform>();
        }
    }

    public void SetSensitivity(float sens)
    {
        if (player != null)
            player.GetComponent<PlayerMovement>().mouseSensitivity = sens;
        if (sens < 2.0f)
        {
            lowIconSens.SetActive(true);
            midIconSens.SetActive(false);
            highIconSens.SetActive(false);
            SensSlider.handleRect = lowIconSens.GetComponent<RectTransform>();
        }
        else if (sens < 4.0f)
        {
            lowIconSens.SetActive(false);
            midIconSens.SetActive(true);
            highIconSens.SetActive(false);
            SensSlider.handleRect = midIconSens.GetComponent<RectTransform>();
        }
        else
        {
            lowIconSens.SetActive(false);
            midIconSens.SetActive(false);
            highIconSens.SetActive(true);
            SensSlider.handleRect = highIconSens.GetComponent<RectTransform>();
        }
    }

    public void ToggleSettingsMenu()
    {
        paused = !gameObject.activeSelf;

        // Show or hide the menu
        gameObject.SetActive(paused);
        GameObject gremlinNamer = null;
        if(Canvas.transform.Find("Gremlin Namer(Clone)") != null)
            gremlinNamer = Canvas.transform.Find("Gremlin Namer(Clone)").gameObject;

        // Pause/Unpause physics
        if (paused)
        {
            // Pause
            PauseGame(true);
            if(gremlinNamer != null)
                gremlinNamer.SetActive(false);
            this.transform.GetChild(1).gameObject.SetActive(true);
            uiSounds[4].Play();
        }
        else
        {
            // Unpause
            PauseGame(false);
            if(gremlinNamer != null){
                gremlinNamer.SetActive(true);
                ToggleMovement(false);
            }
                
            this.transform.GetChild(1).gameObject.SetActive(false);
            uiSounds[3].Play();
        }

    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
            ToggleMovement(false);
        }
        else
        {
            Time.timeScale = 1;
            ToggleMovement(true);
        }
    }

    public void ToggleMovement(bool canMove)
    {
        if (player != null)
            player.enableMovement = canMove;
        if (canMove)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

    }

    public void ToggleOptionsMenu()
    {
        toggleOptions = !toggleOptions;
        this.transform.GetChild(1).gameObject.SetActive(!toggleOptions);
        this.transform.GetChild(2).gameObject.SetActive(toggleOptions);
    }

    public void ToggleScreenMode()
    {
        Screen.fullScreen = !Screen.fullScreen;
        if (!Screen.fullScreen)
        {
            windowedToggle.isOn = false;
            fullscreenToggle.isOn = true;
        }
        else
        {
            windowedToggle.isOn = true;
            fullscreenToggle.isOn = false;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void playButtonConfirm()
    {
        uiSounds[2].Play();
    }

    public void playButtonBack()
    {
        uiSounds[5].Play();
    }
}
