using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler
{
    public AudioSource[] uiSounds;

    // Start is called before the first frame update
    void Start()
    {
        uiSounds = GameObject.Find("UI Sounds").GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        uiSounds[0].Play();
        Debug.Log("Button Hover");
    }
}
