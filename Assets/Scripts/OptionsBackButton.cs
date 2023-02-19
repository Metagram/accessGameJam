/*
@Author - Craig
@Description - 
*/

using Unity.VisualScripting;
using UnityEngine;

public class OptionsBackButton : BasicUIListItem
{
    public GameObject pauseMenuBox;
    public override void OnClick()
    {
        transform.parent.gameObject.SetActive(false);
        pauseMenuBox.SetActive(true);
    }

    void Update()
    {
        if (GameManager.instance.PausePressed())
        {
            OnClick();
        }
    }
}