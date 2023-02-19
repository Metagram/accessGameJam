/*
@Author - Patrick
@Description - Handles room system for game
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallway : MonoBehaviour {
    //!Making this "Hallway" prevents combat! Anything else will allow for combat.
    public string roomType;
    //remember this order for child GameObjects!
    public bool northDoor = true, southDoor = true, eastDoor = true, westDoor = true;
    public float width, height;
    //TODO - Increment this every time an enemy is created in the room
    public int numEnemiesInRoom = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;

        print("Width: " + width + ", Height " + height);
    }

    // Update is called once per frame
    void Update()
    {
        //closes doors
        if (!northDoor){
            transform.GetChild(0).gameObject.GetComponent<Door>().isClosed = true;
        }
        if (!southDoor){
            transform.GetChild(1).gameObject.GetComponent<Door>().isClosed = true;
        }
        if (!eastDoor){
            transform.GetChild(2).gameObject.GetComponent<Door>().isClosed = true;
        }
        if (!westDoor){
            transform.GetChild(3).gameObject.GetComponent<Door>().isClosed = true;
        }
    }

    //!DON'T CHANGE, WORKS FINE!
    void OnTriggerEnter2D(Collider2D collider) {
        Hallway obj = this;
        if (collider.gameObject.GetComponent<Player>() != null && !GameManager.instance.isBattle){
            GameManager.instance.currentRoom = obj;
            print("Current Room: " + GameManager.instance.currentRoom);
        }
    }

 //TODO
//     public static void drawRoomTiles(){
//         //epic randomization
//     }
// }
}