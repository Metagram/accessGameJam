/*
@Author - Patrick
@Description - Handles door triggers
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isClosed = false;
    public new string name;
    public float xPos, yPos, zPos;

    // // Start is called before the first frame update
    // void Start()
    // {
    //     xPos = transform.position.x;
    //     yPos = transform.position.y;
    //     zPos = transform.position.z;
    // }

    // Update is called once per frame
    void Update()
    {
        if (name == "NorthDoor"){
            isClosed = !(GameObject.Find("Hallway").GetComponent<Hallway>().northDoor);
        }
        if (name == "SouthDoor"){
            isClosed =  !(GameObject.Find("Hallway").GetComponent<Hallway>().southDoor);
        }
        if (name == "EastDoor"){
            isClosed =  !(GameObject.Find("Hallway").GetComponent<Hallway>().eastDoor);
        }
        if (name == "WestDoor"){
            isClosed =  !(GameObject.Find("Hallway").GetComponent<Hallway>().westDoor);
        }
    }

    //!Assume collider isn't an enemy. Teleport the player
    void OnTriggerEnter2D(Collider2D collider) {
        if (!GameManager.instance.isBattle && !isClosed){
            GameManager.instance.TeleportPlayer(name);
        }
    }
}