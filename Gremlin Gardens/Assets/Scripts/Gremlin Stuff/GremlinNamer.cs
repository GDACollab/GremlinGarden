using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GremlinNamer : MonoBehaviour
{
    public GameObject submitButton;
    TMP_InputField field;
    System.Action<string> nameCallback;

    /// <summary>
    /// The callback to validate input.
    /// </summary>
    /// <param name="text">The text to validate.</param>
    /// <returns>Whether or not to allow submission for the text.</returns>
    public delegate bool ValidationCallback(string text);

    /// <summary>
    /// The callback for whether or not to validate input.
    /// </summary>
    ValidationCallback validateCallback;
    // Start is called before the first frame update
    void Start()
    {
        field = GetComponentInChildren<TMP_InputField>();
        field.onValueChanged.AddListener(NewText);
        submitButton.GetComponent<Button>().onClick.AddListener(SubmitName);
    }

    /// <summary>
    /// Should be called by whatever creates this naming UI to then be able to get back the name that was input.
    /// </summary>
    /// <param name="callback">The callback that should accept the name from the text box.</param>
    /// <param name="validationCallback">The callback </param>
    public void BeginScanningInput(System.Action<string> callback, ValidationCallback validate) {
        nameCallback = callback;
        // Quick hack to get the player and disable input:
        GameObject.Find("Player").GetComponent<PlayerMovement>().enableMovement = false;
        validateCallback = validate;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    /// <summary>
    /// Called when the input field has new text.
    /// </summary>
    /// <param name="text">The text of the input field.</param>
    void NewText(string text) {
        if (text != "")
        {
            submitButton.SetActive(validateCallback(text));
        }
        else {
            submitButton.SetActive(false);
        }
    }

    void SubmitName() {
        nameCallback(field.text);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameObject.Find("Player").GetComponent<PlayerMovement>().enableMovement = true;
        // Now we clean up the UI:
        GameObject.Destroy(this.gameObject);
    }
}
