﻿using System.Collections;
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
        // while(Physics.Raycast(other.gameObject.transform.position, Vector3.up, 100)){
        //     other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y + 5.0f, other.gameObject.transform.position.z);
        // }
    }
}
