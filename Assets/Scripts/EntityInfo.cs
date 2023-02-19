/*
@Author - Craig
@Description - 
*/

using System.Collections.Generic;
using UnityEngine;

public class EntityInfo : ScriptableObject
{
    public int health;
    public new string name;
    public List<Move> moves;
    public float speed;
}