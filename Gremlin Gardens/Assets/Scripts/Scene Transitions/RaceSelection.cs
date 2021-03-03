using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple script used to load racing scenes.
/// </summary>
public class RaceSelection : MonoBehaviour
{
    /// <summary>
    /// The player to use to detect if we should highlight this object.
    /// </summary>
    [Tooltip("The player to use to detect if we should highlight this object.")]
    public GameObject player;
    /// <summary>
    /// The SceneLoader script to load the scenes with.
    /// </summary>
    [Tooltip("The SceneLoader script to load the scenes with.")]
    public SceneLoader sceneLoader;
    /// <summary>
    /// How close we want the player to be to show the highlight.
    /// </summary>
    [Tooltip("How close we want the player to be to show the highlight.")]
    public float selectionDistance = 10.0f;
    /// <summary>
    /// The scene we're transitioning to next.
    /// </summary>
    [Tooltip("The scene we're transitioning to next.")]
    public string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Outline>().OutlineWidth = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) < selectionDistance) {
            GetComponent<Outline>().OutlineWidth = 10;
        }
    }

    private void OnMouseExit()
    {
        GetComponent<Outline>().OutlineWidth = 0;
    }

    private void OnMouseDown()
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) < selectionDistance)
        {
            sceneLoader.FadeOutLoad(sceneName, 1);
        }
    }
}
