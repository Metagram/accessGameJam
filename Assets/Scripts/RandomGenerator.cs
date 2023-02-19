/*
@Author - Patrick
@Description - The Randomizer! Randomizes enemy numbers, placement, etc
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenerator : MonoBehaviour
{
    public GameObject[] allEnemyPrefabs;
    public const int MAX_OVERWORLD_ENEMIES = 3;

    // Start is called before the first frame update
    void Start()
    {
       //create tutorial book prefabs with description
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //don't worry about destroying enemies, our code already does that
    public void EnemyRandomizer(){
        //Determines how many overworld enemies there will be. Max is exclusive cause Unity API cringe
        int numOverWorldEnemies = Random.Range(1, (allEnemyPrefabs.Length + 1));
        //Determines which elements those X enemies will be
        int[] whichPrefabs = new int[MAX_OVERWORLD_ENEMIES];
        
        //assigns invalid indexes
        for (int i = 0; i < whichPrefabs.Length; i++){
            whichPrefabs[i] = -1;
        }

        //assigns valid indexes
        for (int i = 0; i < numOverWorldEnemies; i++){
            whichPrefabs[i] = Random.Range(1, (allEnemyPrefabs.Length + 1)) - 1;
        }

        GameObject temp;
        
        float width = FindObjectOfType<Hallway>().GetComponent<Hallway>().width;
        float height = FindObjectOfType<Hallway>().GetComponent<Hallway>().height;
        
        float offset = 2f;

        float randX, randY;
        float randZ = -1;

        //actually instantiates enemies
        for (int i = 0; i < numOverWorldEnemies; i++){
            //checks for valid index
            if (whichPrefabs[i] != -1){
                temp = allEnemyPrefabs[whichPrefabs[i]];
                randX = Random.Range(-1 * ((width/2) - offset), (width/2) - offset);
                randY = Random.Range((-1 * (height/2) - offset), (height/2) - offset);
                Instantiate(temp, new Vector3(randX, randY, randZ), Quaternion.identity);
            }
        }
    }
}