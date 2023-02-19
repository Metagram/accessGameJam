
using System;
using Unity.VisualScripting;
using UnityEngine;

public class PartySelectButton : BasicUIListItem
{
    public string memberName;
    public override void OnClick()
    {
        bool found = false;
        Player player = null;
        foreach (Player p in GameManager.instance.players)
        {
            if (p.characterInfo.name == memberName)
            {
                found = true;
                player = p;
                break;
            }
        }

        if (!found)
        {
            return;
        }

        bool wasMain = player.mainCharacter;
        
        GameManager.instance.players.RemoveAll(p => p == player);
        if (GameManager.instance.players.Count == 0)
        {
            GameManager.instance.GameOver();
        }

        if (wasMain)
        {
            GameManager.instance.players[0].mainCharacter = true;
            GameObject camera = player.GetComponentInChildren<PlayerCamera>().gameObject;
            camera.transform.SetParent(GameManager.instance.players[0].transform);
            Destroy(player.gameObject);
            GameManager.instance.players[0].gameObject.SetActive(true);
            GameManager.instance.mainPlayer = GameManager.instance.players[0];
            GameManager.instance.mainPlayer.transform.position = player.transform.position;
        }

        int healthToDistribute = 100;
        foreach (Player p in GameManager.instance.players)
        {
            int neededHealth = p.characterInfo.health - p.health;
            int givenHealth = Math.Min(neededHealth, healthToDistribute);
            p.Heal(givenHealth);
            healthToDistribute -= givenHealth;
        }
    }
}