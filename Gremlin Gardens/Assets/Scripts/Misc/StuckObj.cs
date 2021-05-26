using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckObj : MonoBehaviour
{
    private GameObject player;
    void Start(){
        player = GameObject.Find("Player");
    }

    void OnTriggerEnter(Collider other){
        other.gameObject.transform.position = player.transform.position + new Vector3(0, 3, 0);
    }
}
