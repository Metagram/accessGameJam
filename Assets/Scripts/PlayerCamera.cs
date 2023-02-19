/*
@Author - Craig
@Description - 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    void Update()
    {
        if (GameManager.instance.isBattle)
        {
            transform.SetParent(null);
            transform.position = Vector3.Lerp(GameManager.instance.playerSpotsParent.transform.position,
                GameManager.instance.enemySpotsParent.transform.position, 0.5f) - new Vector3(0, 0, 10);
        }
        else
        {
            transform.SetParent(GameManager.instance.mainPlayer.transform);
            transform.localPosition = new Vector3(0, 0, -10);
        }
    }
}
