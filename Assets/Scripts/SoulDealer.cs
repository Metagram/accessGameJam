
using System;
using UnityEngine;

public class SoulDealer : MonoBehaviour
{
    private bool playerInRange;
    public GameObject soulDealerUI;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            playerInRange = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            playerInRange = false;
        }
    }

    void Update()
    {
        bool confirm = GameManager.instance.Confirm();
        bool cancel = GameManager.instance.Cancel();
        if (playerInRange && confirm)
        {
            soulDealerUI.SetActive(true);
        }
        
        bool active = soulDealerUI.activeSelf;
        Player.frozen = active;
        //else if (playerInRange && cancel)
        //{
        //    soulDealerUI.SetActive(false);
        //}
    }
}
