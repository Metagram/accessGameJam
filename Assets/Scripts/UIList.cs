/*
@Author - Craig
@Description - 
*/

using UnityEngine;

public class UIList : MonoBehaviour
{
    public GameObject firstItem;
    public GameObject selectionIndicator;
    private UIListItem currentSelection;
    
    protected virtual void Start()
    {
        currentSelection = firstItem.GetComponent<UIListItem>();
    }
    void Update()
    {
        if (InputManager.instance.Up())
        {
            currentSelection = currentSelection.Previous();
        } else if (InputManager.instance.Down())
        {
            currentSelection = currentSelection.Next();
        }

        if (currentSelection.GetGameObject() != selectionIndicator.transform.parent.gameObject)
        {
            Vector3 oldLocalPosition = selectionIndicator.transform.localPosition;
            selectionIndicator.transform.SetParent(currentSelection.GetGameObject().transform);
            selectionIndicator.transform.localPosition = oldLocalPosition;
        }

        if (InputManager.instance.Confirm())
        {
            currentSelection.OnClick();
        }
    }
}
