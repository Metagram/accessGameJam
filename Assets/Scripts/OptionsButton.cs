/*
@Author - Craig
@Description - 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsButton : BasicUIListItem
{
    public GameObject optionsBoxPrefab;
    public GameObject optionsBox;

    void Start()
    {
        optionsBox = Instantiate(optionsBoxPrefab, GameManager.instance.pauseMenu.transform);
    }
    public override void OnClick()
    {
        optionsBox.SetActive(true);
        optionsBox.GetComponentInChildren<OptionsBackButton>().pauseMenuBox = transform.parent.gameObject;
        transform.parent.gameObject.SetActive(false);
    }
}
