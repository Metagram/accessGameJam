/*
@Author - Craig
@Description - 
*/

using System.Collections.Generic;
using UnityEngine;

public class MoveListUI : MonoBehaviour
{
    public List<Move> moveList;

    public List<GameObject> lines; //bottom to top
    public int selected = 0;
    public bool locked = true;
    public bool hasSelection = false;
    public Sprite fireMoveSprite;
    public Sprite waterMoveSprite;
    public Sprite earthMoveSprite;
    public Sprite normalMoveSprite;

    public static MoveListUI instance;

    public Dictionary<Move.MoveType, Sprite> sprites;

    void Awake()
    {
        instance = this;
    }

    public Move GetSelectedMove()
    {
        return moveList[selected];
    }
    
    void Start()
    {
        for (int i = 0; i < moveList.Count; i++)
        {
            Deselect(i);
        }
        sprites = new Dictionary<Move.MoveType, Sprite>();
        sprites.Add(Move.MoveType.FIRE, fireMoveSprite);
        sprites.Add(Move.MoveType.WATER, waterMoveSprite);
        sprites.Add(Move.MoveType.EARTH, earthMoveSprite);
        sprites.Add(Move.MoveType.NORMAL, normalMoveSprite);
    }

    void Select(int i)
    {
        lines[i].GetComponent<MoveLine>().selected = true;
    }
    void Deselect(int i)
    {
        lines[i].GetComponent<MoveLine>().selected = false;
    }
    void Update()
    {
        for (int i = 0; i < moveList.Count; i++)
        {
            lines[i].GetComponent<MoveLine>().move = moveList[i];
            lines[i].SetActive(true);
        }
        for (int i = moveList.Count; i < lines.Count; i++)
        {
            lines[i].SetActive(false);
        }

        if (hasSelection)
        {
            Select(selected);
        }
        else
        {
            for (int i = 0; i < moveList.Count; i++)
            {
                Deselect(i);
            }
        }

        if (GameManager.instance.isBattle && !locked)
        {
            if (GameManager.instance.Up())
            {
                Deselect(selected);
                do
                {
                    selected = (selected + 1) % moveList.Count;
                } while (!BattleUIManager.instance.GetSelectedPlayer().CanUseMove(GetSelectedMove()));
                
                Select(selected);
            } else if (GameManager.instance.Down())
            {
                Deselect(selected);
                do
                {
                    selected = (selected - 1 + moveList.Count) % moveList.Count;
                } while (!BattleUIManager.instance.GetSelectedPlayer().CanUseMove(GetSelectedMove()));
                Select(selected);
            }
        }
    }
}
