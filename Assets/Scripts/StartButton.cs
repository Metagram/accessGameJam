/*
@Author - Craig
@Description - 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : BasicUIListItem
{
    public override void OnClick()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
