using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VN_SceneManager : MonoBehaviour
{
    public VN_Manager VN_Manager;

    private void Awake()
    {
        LoadingData.ConstructRaceStatuses();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        VN_Manager.OnEndStory.AddListener(OnEndVNCallback);

        VN_Manager.nextScene = LoadingData.RaceSceneDictionary[LoadingData.currentRace];
    }

    private void Start()
    {
        VN_Manager.StartVN();
    }

    public void OnEndVNCallback()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
