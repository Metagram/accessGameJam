/*
@Author - Craig
@Description - 
*/

using UnityEngine;

public abstract class BasicUIListItem : MonoBehaviour, UIListItem
{
    public GameObject next;
    public GameObject previous;

    public UIListItem Next()
    {
        return next.GetComponent<UIListItem>();
    }

    public UIListItem Previous()
    {
        return previous.GetComponent<UIListItem>();
    }
    
    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public abstract void OnClick();
}
