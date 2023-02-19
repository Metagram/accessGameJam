/*
@Author - Craig
@Description - 
*/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Move")]
public class Move : ScriptableObject
{
    public enum SelectionType
    {
        ENEMY_ONLY, ALLY_ONLY, ALL, ALL_ENEMY
    }

    public enum Effect
    {
        HEAL, DAMAGE, APPLY_STRENGTH, APPLY_WEAKNESS, APPLY_IMMUNITY, APPLY_BLOCK_SELF
    }
    public new string name;
    public string description;
    public SelectionType selectionType = SelectionType.ENEMY_ONLY;
    public List<Effect> effects = new List<Effect>();
    public List<int> effectAmounts = new List<int>();
    
    //It is assumed that there is at most 1 cost of each type
    public enum Cost
    {
        HEALTH, MANA
    }

    public List<Cost> costs;
    public List<int> costAmounts;
    
    public enum MoveType
    {
        EARTH, FIRE, NORMAL, WATER
    }

    public MoveType moveType = MoveType.NORMAL;
}
