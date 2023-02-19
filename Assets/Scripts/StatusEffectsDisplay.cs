/*
@Author - Craig
@Description - 
*/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusEffectsDisplay : MonoBehaviour
{
    private Entity entity;
    private List<GameObject> displays = new();

    void Start()
    {
        entity = GetComponentInParent<Entity>();
    }

    void Update()
    {
        foreach (GameObject obj in displays)
        {
            Destroy(obj);
        }
        
        displays = new List<GameObject>();
        foreach (StatusEffectType effectType in System.Enum.GetValues(typeof(StatusEffectType)).Cast<StatusEffectType>())
        {
            if (entity.GetEffectAmount(effectType) > 0)
            {
                GameObject obj = Instantiate(GameManager.instance.statusEffectImagePrefab, transform);
                obj.GetComponent<StatusEffectImage>().UpdateDisplay(entity, effectType);
                displays.Add(obj);
            }
        }
    }
}
