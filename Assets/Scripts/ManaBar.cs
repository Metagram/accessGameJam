/*
@Author - Craig
@Description - 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManaBar : MonoBehaviour
{
    public Player player;

    private float originalWidth;

    void Start()
    {
        player = GetComponentInParent<Player>();
        originalWidth = ((RectTransform)transform).rect.width;
    }
    void Update()
    {
        float healthPercent = (float)player.mana / player.characterInfo.mana;
        transform.parent.GetComponentInChildren<TextMeshProUGUI>().text = player.mana + "";
        ((RectTransform)transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalWidth * healthPercent);
    }
}
