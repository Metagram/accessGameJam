/*
@Author - Craig
@Description - 
*/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectImage : MonoBehaviour
{
    public void UpdateDisplay(Entity entity, StatusEffectType type)
    {
        GetComponent<Image>().sprite = GameManager.instance.statusEffectSprites[type];
        GetComponentInChildren<TextMeshProUGUI>().text = entity.GetEffectAmount(type) + "";
    }
}
