using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    /// <summary>
    /// The UI object to select the gremlins with.
    /// </summary>
    [Tooltip("The UI object to select gremlins with.")]
    public GameObject gremlinPicker;

    /// <summary>
    /// The button prefab for selecting gremlins.
    /// </summary>
    [Tooltip("The button prefab for selecting gremlins.")]
    public GameObject gremlinPickerButton;

    public SettingsMenu settings;

    /// <summary>
    /// Is the selection UI up?
    /// </summary>
    bool selectionUI = false;
    // Start is called before the first frame update
    void Start()
    {
        gremlinPicker.SetActive(false);
        GetComponent<Outline>().OutlineWidth = 0;
    }

    // Update is called once per frame
    void Update()
    {
        var playerMove = player.GetComponent<PlayerMovement>();
        if (playerMove.centeredObject == this.gameObject)
        {
            IsCentered();
            if (Input.GetMouseButtonDown(0)) {
                MouseDown();
            }
        }
        else if (playerMove.centeredObject != this.gameObject && playerMove.hitObjectIsNew)
        {
            IsExited();
        }
    }

    private void IsCentered()
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) < selectionDistance) {
            GetComponent<Outline>().OutlineWidth = 10;
        }
    }

    private void IsExited()
    {
        GetComponent<Outline>().OutlineWidth = 0;
    }

    private void MouseDown()
    {
        if (!settings.paused && Vector3.Distance(this.transform.position, player.transform.position) < selectionDistance && selectionUI == false && gremlinPicker.activeInHierarchy != true)
        {
            selectionUI = true;
            // Quick hack for getting a gremlin selector before the race. 
            foreach (KeyValuePair<string, Gremlin> savedGremlin in LoadingData.playerGremlins) {
                // Also make sure to update the gremlin's current position so that the GremlinSpawner can use it later:
                savedGremlin.Value.currentPosition = savedGremlin.Value.transformReference.position;
                savedGremlin.Value.currentRotation = savedGremlin.Value.transformReference.rotation;
                var button = Instantiate(gremlinPickerButton);
                button.transform.parent = gremlinPicker.transform;
                button.GetComponentInChildren<Text>().text = savedGremlin.Key;
                button.GetComponent<Button>().onClick.AddListener(delegate { GremlinSelected(savedGremlin.Key);  });
            }
            gremlinPicker.SetActive(true);
            // Bring up the gremlin selection UI.
            player.GetComponent<PlayerMovement>().enableMovement = false;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    public void DestroyUI() {
        // Destroy the buttons to select gremlins just in case this somehow gets called twice:
        for (int i = 0; i < gremlinPicker.transform.childCount; i++)
        {
            var child = gremlinPicker.transform.GetChild(i);
            Destroy(child.gameObject);
        }
        gremlinPicker.SetActive(false);
    }

    public void GremlinSelected(string gremlinName) {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        // Make sure player's position and rotation is saved:
        LoadingData.playerPosition = player.transform.position;
        LoadingData.playerRotation = player.transform.rotation;
        DestroyUI();
        LoadingData.gremlinToRace = gremlinName;
        sceneLoader.FadeOutLoad(sceneName, 1);
        selectionUI = false;
    }
}
