/*
@Author - Craig
@Description - 
*/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoveLine : MonoBehaviour
{
    public Move move;
    public GameObject selectionIndicator;
    public bool selected = false;

    // Update is called once per frame
    void Update()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = move.name;
        GetComponent<Image>().sprite = MoveListUI.instance.sprites[move.moveType];
        selectionIndicator.SetActive(selected);
    }
}
