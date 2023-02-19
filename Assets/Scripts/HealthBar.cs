/*
@Author - Craig
@Description - 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Entity entity;

    private float originalWidth;

    void Start()
    {
        entity = GetComponentInParent<Entity>();
        originalWidth = ((RectTransform)transform).rect.width;
    }
    void Update()
    {
        float healthPercent = (float)entity.GetHealth() / entity.GetEntityInfo().health;
        transform.parent.GetComponentInChildren<TextMeshProUGUI>().text = entity.GetHealth() + "";
        ((RectTransform)transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalWidth * healthPercent);
    }
}
