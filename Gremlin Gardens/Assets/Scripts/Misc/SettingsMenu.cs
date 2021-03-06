﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Settings")]

    public bool inVN = false;

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

    [Header("Other UI Stuff")]
    public GameObject settingsMenu;
    public GameObject Canvas;
    public GameObject SceneMusic;
    /// <summary>
    /// Quick hack to prevent the settings menu from acting weird in the main menu.
    /// </summary>
    public bool isMainMenu = false;
    /// <summary>
    /// How much to reduce the volume of BGM music when the game is paused
    /// </summary>
    [Tooltip("How much to reduce the volume of BGM music when the game is paused")]
    [Range(0.01f, 1.0f)]
    public float pausedBGMReduction = 0.3f;

    private GameObject pauseMenu;
    private GameObject optionsMenu;

    bool pausedGremlinSelect = false;

    public void Awake()
    {
        uiSounds = GameObject.Find("UI Sounds").GetComponents<AudioSource>();
        if (GameObject.Find("Player")) // This is so it doesn't break races.
        {
            player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        }
        optionsMenu = gameObject.transform.Find("OptionsMenu").gameObject;
        pauseMenu = gameObject.transform.Find("PauseMenu").gameObject;
        SetSensitivity(SensSlider.value);
        SetVolume(VolumeSlider.value);
    }

    void Update()
    {
        if (!isMainMenu && Input.GetButtonDown("Cancel"))
            ToggleSettingsMenu();
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
        if (sens < SensSlider.maxValue / 3f)
        {
            lowIconSens.SetActive(true);
            midIconSens.SetActive(false);
            highIconSens.SetActive(false);
            SensSlider.handleRect = lowIconSens.GetComponent<RectTransform>();
        }
        else if (sens < SensSlider.maxValue / 3f * 2f)
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
        paused = !paused;

        this.transform.Find("Background").gameObject.SetActive(paused);

        GameObject gremlinNamer = null;
        if (Canvas.transform.Find("Gremlin Namer(Clone)") != null)
            gremlinNamer = Canvas.transform.Find("Gremlin Namer(Clone)").gameObject;

        GameObject gremlinSelect = null;
        if (Canvas.transform.Find("Gremlin Select") != null)
            gremlinSelect = Canvas.transform.Find("Gremlin Select").gameObject;

        // Pause/Unpause physics
        if (paused)
        {
            // Pause
            PauseGame(true);
            SceneMusic.GetComponent<AudioSource>().volume *= pausedBGMReduction;
            if (gremlinNamer != null)
                gremlinNamer.SetActive(false);
            else if (gremlinSelect != null && gremlinSelect.activeInHierarchy == true)
            {
                gremlinSelect.SetActive(false);
                pausedGremlinSelect = true;
            }

            pauseMenu.SetActive(true);
            optionsMenu.SetActive(false);
            uiSounds[4].Play();
        }
        else
        {
            // Unpause
            PauseGame(false);
            SceneMusic.GetComponent<AudioSource>().volume /= pausedBGMReduction;
            if (gremlinNamer != null)
            {
                gremlinNamer.SetActive(true);
                ToggleMovement(false);
            }
            else if (gremlinSelect != null && pausedGremlinSelect == true)
            {
                gremlinSelect.SetActive(true);
                pausedGremlinSelect = false;
                ToggleMovement(false);
            }

            pauseMenu.SetActive(false);
            optionsMenu.SetActive(false);
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
            if (!inVN)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
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
        if (!isMainMenu)
        {
            pauseMenu.SetActive(!toggleOptions);
        }
        optionsMenu.gameObject.SetActive(toggleOptions);
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
