using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    private List<TutorialTask> tasks;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if a task is done?

        // Update the UI list to check off completed tasks

        // Bring up extended description of current item
    }

    // Called from each of the tutorial tasks to populate the list
    public void AddTask() {

    }

    // Called from each of the tutorial tasks to check off its item in the list
    public void CompletedTask() {

    }
}
