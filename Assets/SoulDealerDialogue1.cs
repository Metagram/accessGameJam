using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulDealerDialogue1 : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        bool canel = GameManager.instance.Cancel();
        if (canel)
        {
            StartCoroutine(closeUI());
        }
    }

    public IEnumerator closeUI()
    {
        yield return null;
        GetComponentInParent<SoulDealerUI>().gameObject.SetActive(false);
    }
}
