using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Placed on each tutorializable thing, registers a UnityEvent with the TutorialManager
/// </summary>
public class TutorialTask : MonoBehaviour
{
    public UnityEvent addTask;
    public UnityEvent completedTask;

    // description text for each sub-task, such as "Press E to pet a gremlin"
    public List<string> subtaskDescriptions;
    public List<bool> subtaskCompletions;
    
    // description and completion status of the entire task
    public string shortDescription;
    public bool taskCompletion;

    // Start is called before the first frame update
    void Start()
    {
        if (addTask != null) {
            addTask.Invoke();
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && completedTask != null) {
            completedTask.Invoke();
        }
    }
}
