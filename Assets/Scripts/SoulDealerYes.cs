
using Unity.VisualScripting;
using UnityEngine;

public class SoulDealerYes : BasicUIListItem
{
    public GameObject nextDialogue;
    public override void OnClick()
    {
        GetComponentInParent<SoulDealerDialogue1>().gameObject.SetActive(false);
        nextDialogue.SetActive(true);
    }
}
