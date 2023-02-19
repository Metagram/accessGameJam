/*
@Author - Craig
@Description - 
*/

using UnityEngine;

public interface UIListItem
{
    public UIListItem Next();
    public UIListItem Previous();
    public void OnClick();

    public GameObject GetGameObject();
}