using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

// Uses System.Reflection during runtime
// Perhaps dangerous, needs further testing?
public class VN_SharedVariables : MonoBehaviour
{
    [Header("Ink/Unity shared variables")]
    public string testString;
    public int testInt;

    public string gremlinToRace;
    public string selectedRace;
    public int seenUnique;
    public int wonRace;
    public int hasWonRace;

    private FieldInfo[] FieldInfoArray;
    private VN_Manager manager;

    public void Construct(VN_Manager manager)
    {
        this.manager = manager;
        FieldInfoArray = GetType().GetFields();

        LoadingData.ConstructRaceStatuses();

        gremlinToRace = LoadingData.gremlinToRace;
        selectedRace = LoadingData.currentRace.ToString();
        wonRace = Convert.ToInt32(LoadingData.wonCurrentRace);
        hasWonRace = Convert.ToInt32(LoadingData.RaceHistoryDictionary[LoadingData.currentRace].Item1);
        seenUnique = Convert.ToInt32(LoadingData.RaceHistoryDictionary[LoadingData.currentRace].Item2);
    }

    public void SetVariable(string varName, string newValString)
    {
        Type T = this.GetType();
        FieldInfo toSet = T.GetField(varName);

        // Set field value to newValString
        toSet.SetValue(this,
            // Try to convert the type to the correct type
            Convert.ChangeType(newValString, toSet.FieldType));
    }

    public string GetVariableValue(string varName)
    {
        Type T = this.GetType();
        FieldInfo toGet = T.GetField(varName);
        return toGet.GetValue(this).ToString();
    }
}