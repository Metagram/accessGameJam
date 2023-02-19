/*
@Author - Craig
@Description - Let's player rebind their keys
*/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RebindButton : BasicUIListItem
{
    public static IEnumerator Rebind(string actionName)
    {
        yield return null;
        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                GameManager.instance.controls[actionName] = key;
                GameManager.instance.UpdateControlsFile();
                break;
            }
        }
    }
    public string actionName;
    

    public override void OnClick()
    {
        StartCoroutine(Rebind(actionName));
    }

    void Update()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = actionName + ": " + GameManager.instance.controls[actionName].HumanName();
    }
}
