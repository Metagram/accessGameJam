/*
@Author - Craig
@Description - 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : BasicUIListItem
{
    private int whoAsked = 0;
    public override void OnClick()
    {
        ++whoAsked;
        GameManager.instance.UnPause();
    }

    void Update()
    {
        if (GameManager.instance.PausePressed())
        {
            GameManager.instance.UnPause();
        }
    }
}
