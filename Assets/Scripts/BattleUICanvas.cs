/*
@Author - Craig
@Description - 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUICanvas : MonoBehaviour
{
    void Start()
    {
        GetComponent<Canvas>().sortingLayerID = SortingLayer.NameToID("main UI");
    }
}