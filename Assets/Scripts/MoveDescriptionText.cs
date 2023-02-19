/*
@Author - Craig
@Description - 
*/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveDescriptionText : MonoBehaviour
{
    void Update()
    {
        if (MoveListUI.instance.hasSelection)
        {
            GetComponent<TextMeshProUGUI>().text = MoveListUI.instance.GetSelectedMove().description;
        }
        else
        {
            GetComponent<TextMeshProUGUI>().text = "";
        }
    }
}
