/*
@Author - Craig and Patrick
@Description - Handles all enemy game objects.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : Entity
{
    public EnemyInfo enemyInfo;

    IEnumerator turnLoop() {
        while (true) {
            turn();
            yield return new WaitForSeconds(1);
        }
    }

    void turn (){
        transform.Rotate(0, 0, -90);
    }

    public int GetPackSize()
    {
        return Random.Range(enemyInfo.minPackSize, enemyInfo.maxPackSize+1);
    }

    public override EntityInfo GetEntityInfo()
    {
        return enemyInfo;
    }
}
