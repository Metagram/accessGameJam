
using UnityEngine;

public class SoulDealerDialogue2 : MonoBehaviour
{
    void Update()
    {
        bool cancel = GameManager.instance.Cancel();
        if (cancel)
        {
            FindObjectOfType<SoulDealerDialogue1>(true).gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
