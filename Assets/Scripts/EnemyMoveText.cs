/*
@Author - Craig
@Description - 
*/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyMoveText : MonoBehaviour
{
    public static EnemyMoveText instance;
    void Start()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void SetText(string text)
    {
        GetComponent<TextMeshProUGUI>().text = text;
    }
}
