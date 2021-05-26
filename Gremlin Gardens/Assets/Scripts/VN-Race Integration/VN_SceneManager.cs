using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VN_SceneManager : MonoBehaviour
{
    public VN_Manager VN_Manager;
    public bool loadCurrentRaceNext;

    private void Awake()
    {
        LoadingData.ConstructRaceStatuses();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        VN_Manager.OnEndStory.AddListener(OnEndVNCallback);

        if(loadCurrentRaceNext)
        {
            VN_Manager.nextScene = LoadingData.RaceSceneDictionary[LoadingData.currentRace];
        } 
    }

    private void Start()
    {
        VN_Manager.StartVN();
    }

    public void OnEndVNCallback()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Mark this scene as seen
        if(!loadCurrentRaceNext)
        {
            bool wonRace = LoadingData.RaceHistoryDictionary[LoadingData.currentRace].Item1;
            LoadingData.RaceHistoryDictionary[LoadingData.currentRace] = (wonRace, true);

            if(LoadingData.wonCurrentRace)
            {
                bool seenUnique = LoadingData.RaceHistoryDictionary[LoadingData.currentRace].Item2;
                LoadingData.RaceHistoryDictionary[LoadingData.currentRace] = (true, seenUnique);
            }

            print(this + " hasWonRace: " + LoadingData.RaceHistoryDictionary[LoadingData.currentRace].Item1
                + " seenUnique: " + LoadingData.RaceHistoryDictionary[LoadingData.currentRace].Item2);
        }
    }
}
