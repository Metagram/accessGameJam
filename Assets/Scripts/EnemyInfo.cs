/*
@Author - Craig
@Description - 
*/

using UnityEngine;

[CreateAssetMenu(menuName = "Enemy")]
public class EnemyInfo : EntityInfo
{
    public int minPackSize = 1;
    public int maxPackSize = 3;
    public int sightRange = 5;
}