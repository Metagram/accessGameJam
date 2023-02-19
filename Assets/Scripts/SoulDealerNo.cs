
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SoulDealerNo : BasicUIListItem
{
    public override void OnClick()
    {
        StartCoroutine(closeUI());
    }
    
    public IEnumerator closeUI()
    {
        yield return null;
        GetComponentInParent<SoulDealerUI>().gameObject.SetActive(false);
    }
}
