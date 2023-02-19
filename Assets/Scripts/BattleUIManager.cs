/*
@Author - Craig
@Description - 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class BattleUIManager : MonoBehaviour
{
    public State state;
    public GameObject playerSelectPrefab;
    public GameObject playerSelect;
    public GameObject enemySelectPrefab;
    [FormerlySerializedAs("enemySelect")] public GameObject entitySelect;
    public static BattleUIManager instance;

    private Dictionary<Player, bool> attacked = new Dictionary<Player, bool>();
    private bool isEnemySelect = true;
    
    public Sprite weaknessSprite;
    public Sprite strengthSprite;
    public Dictionary<StatusEffectType, Sprite> statusEffectSprites = new Dictionary<StatusEffectType, Sprite>();
    
    private Move learnMove;
    public GameObject learnMovePrefab;
    public GameObject learnMoveUI;
    public Move GetLearnMove()
    {
        return learnMove;
    }

    void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        playerSelect = Instantiate(playerSelectPrefab, transform);
        entitySelect = Instantiate(enemySelectPrefab, transform);
        learnMoveUI = Instantiate(learnMovePrefab, transform);
        learnMoveUI.SetActive(false);
        entitySelect.SetActive(false);
        gameObject.SetActive(false);
    }
    
    public enum State
    {
        PLAYER_SELECT, MOVE_SELECT, TARGET_SELECT, ENEMY_TURN, ANIMATION, LEARN_MOVE, LEARN_MOVE_REPLACE
    }

    public void Reset()
    {
        attacked.Clear();
        foreach (Player player in GameManager.instance.players)
        {
            attacked.Add(player, false);
        }
        state = State.PLAYER_SELECT;
        ResetSelections();
        learnMoveUI.SetActive(false);
    }

    public void StartBattle()
    {
        learnMove = GameManager.instance.battleEnemies[0].moves[0];
    }

    private void ResetSelections()
    {
        selectedPlayer = 0;
        if (!allAttacked())
        {
            do
            {
                selectedPlayer++;
                selectedPlayer %= GameManager.instance.players.Count;
            } while (attacked[GameManager.instance.players[selectedPlayer]]);
        }

        selectedEntity = 0;
        MoveListUI.instance.locked = true;
        MoveListUI.instance.hasSelection = false;
        MoveListUI.instance.selected = 0;
        entitySelect.SetActive(false);
    }

    public int selectedPlayer = 0;
    [FormerlySerializedAs("selectedEnemy")] public int selectedEntity = 0;
    

    private void CancelMoveSelect()
    {
        state = State.PLAYER_SELECT;
        MoveListUI.instance.locked = true;
        MoveListUI.instance.hasSelection = false;
        MoveListUI.instance.selected = 0;
    }

    private void EnterMoveSelect()
    {
        state = State.MOVE_SELECT;
        MoveListUI.instance.locked = false;
        MoveListUI.instance.hasSelection = true;
        MoveListUI.instance.selected = 0;
    }

    private void EnterEntitySelect()
    {
        Move selected = MoveListUI.instance.GetSelectedMove();
        state = State.TARGET_SELECT;
        MoveListUI.instance.locked = true;
        selectedEntity = 0;
        isEnemySelect = selected.selectionType == Move.SelectionType.ALL || selected.selectionType == Move.SelectionType.ENEMY_ONLY || selected.selectionType == Move.SelectionType.ALL_ENEMY;
        entitySelect.SetActive(true);
    }

    private void CancelEntitySelect()
    {
        state = State.MOVE_SELECT;
        MoveListUI.instance.locked = false;
        entitySelect.SetActive(false);
    }

    private void HandleDeadEntities()
    {
        foreach (Enemy e in GameManager.instance.battleEnemies)
        {
            if (e.health <= 0)
            {
                Destroy(e.gameObject);
            }
        }
        GameManager.instance.battleEnemies.RemoveAll(e => e.health <= 0);
        if (GameManager.instance.battleEnemies.Count == 0)
        {
            EndBattle();
        }
        
        bool shouldFindNewMain = false;
        foreach (Player p in GameManager.instance.players)
        {
            if (p.health <= 0)
            {
                Destroy(p.gameObject);
                if (p.mainCharacter)
                {
                    shouldFindNewMain = true;
                }
            }
        }
        GameManager.instance.players.RemoveAll(p => p.health <= 0);
        if (GameManager.instance.players.Count == 0)
        {
            //display game over screen
            GameManager.instance.GameOver();
        }
        if (shouldFindNewMain)
        {
            GameManager.instance.players[0].mainCharacter = true;
        }
    }

    public void NextPlayerAttack()
    {
        HandleDeadEntities();
        ResetSelections();
        if (state != State.LEARN_MOVE)
        {
            state = State.PLAYER_SELECT;
        }
    }

    public void EndBattle()
    {
        if (Random.Range(0f, 1f) < 0.99f)
        {
            EnterLearnMove();
        }
        else
        {
            GameManager.instance.EndBattle();
        }
    }

    private void ResetAttacked()
    {
        attacked.Clear();
        foreach (Player player in GameManager.instance.players)
        {
            attacked[player] = false;
        }
    }

    IEnumerator EnemyTurn()
    {
        for (int i = 0; i < GameManager.instance.battleEnemies.Count; i++)
        {
            Enemy enemy = GameManager.instance.battleEnemies[i];
            
            List<Move> usableMoves = new();
            foreach (Move move in enemy.moves)
            {
                if (enemy.CanUseMove(move))
                {
                    usableMoves.Add(move);
                }
            }
            if (usableMoves.Count == 0)
            {
                continue;
            }
            
            Move selectedMove = usableMoves[Random.Range(0, usableMoves.Count)];
            List<Entity> target = selectedMove.selectionType == Move.SelectionType.ALL_ENEMY ?
                GameManager.instance.players.Cast<Entity>().ToList() :
                new List<Entity>{GameManager.instance.players[Random.Range(0, GameManager.instance.players.Count)]};
            EnemyMoveText.instance.SetText(enemy.enemyInfo.name + " used " + selectedMove.name);
            
            var anim = new BumpAnimation(enemy, target[0]);
            yield return anim.Start();
            enemy.UseMove(selectedMove, target);
            yield return anim.End();
            //yield return new WaitForSeconds(1);
            HandleDeadEntities();
        }

        TickStatusEffects();
        HandleDeadEntities();
        EnterPlayerTurn();
    }

    private void TickStatusEffects()
    {
        foreach (Enemy enemy in GameManager.instance.battleEnemies)
        {
            enemy.TickStatusEffects();
        }
        foreach (Player player in GameManager.instance.players)
        {
            player.TickStatusEffects();
        }
    }

    private void EnterEnemyTurn()
    {
        ResetAttacked();
        ResetSelections();
        state = State.ENEMY_TURN;
        playerSelect.SetActive(false);
        MoveListUI.instance.transform.parent.gameObject.SetActive(false);
        EnemyMoveText.instance.gameObject.SetActive(true);
        HandleDeadEntities();
        StartCoroutine(EnemyTurn());
    }

    private void EnterPlayerTurn()
    {
        ResetAttacked();
        HandleDeadEntities();
        state = State.PLAYER_SELECT;
        playerSelect.SetActive(true);
        selectedPlayer = 0;
        MoveListUI.instance.transform.parent.gameObject.SetActive(true);
        EnemyMoveText.instance.gameObject.SetActive(false);
    }

    private bool allAttacked()
    {
        foreach (Player player in GameManager.instance.players)
        {
            if (!attacked[player])
            {
                return false;
            }
        }

        return true;
    }

    public Player GetSelectedPlayer()
    {
        return GameManager.instance.players[selectedPlayer];
    }
    
    private void EnterLearnMove()
    {
        playerSelect.SetActive(true);
        selectedPlayer = 0;
        MoveListUI.instance.transform.parent.gameObject.SetActive(true);
        EnemyMoveText.instance.gameObject.SetActive(false);
        learnMoveUI.SetActive(true);
        learnMoveUI.GetComponent<LearnMoveUI>().SetText("You got a new move!  Who should learn " + learnMove.name + "?");
        MoveListUI.instance.locked = false;
        state = State.LEARN_MOVE;
    }

    private void CancelReplaceMove()
    {
        state = State.LEARN_MOVE;
        MoveListUI.instance.hasSelection = false;
        MoveListUI.instance.locked = true;
    }
    
    private void EnterReplaceMove()
    {
        state = State.LEARN_MOVE_REPLACE;
        MoveListUI.instance.hasSelection = true;
        MoveListUI.instance.locked = false;
    }

    private void UpdatePlayerSelect(bool respectAttacked)
    {
        bool up = GameManager.instance.Up();
        bool down = GameManager.instance.Down();
        int players = GameManager.instance.players.Count;
        if (up)
        {
            do
            {
                selectedPlayer--;
                selectedPlayer += players;
                selectedPlayer %= players;
            } while (attacked[GameManager.instance.players[selectedPlayer]] && respectAttacked);
        } else if (down)
        {
            do
            {
                selectedPlayer++;
                selectedPlayer %= players;
            } while (attacked[GameManager.instance.players[selectedPlayer]] && respectAttacked);
        }
        MoveListUI.instance.moveList = GameManager.instance.players[selectedPlayer].moves;
        playerSelect.transform.position = GameManager.instance.players[selectedPlayer].transform.position;
        
    }
    public void Update()
    {
        if (GameManager.instance.pause)
        {
            return;
        }
        bool up = GameManager.instance.Up();
        bool down = GameManager.instance.Down();
        bool confirm = GameManager.instance.Confirm();
        bool cancel = GameManager.instance.Cancel();
        switch (state)
        {
            case State.PLAYER_SELECT:
                UpdatePlayerSelect(true);
                if (confirm)
                {
                    EnterMoveSelect();
                }
                break;
            case State.MOVE_SELECT:
                if (cancel)
                {
                    CancelMoveSelect();
                } else if (confirm)
                {
                    EnterEntitySelect();
                }
                break;
            case State.TARGET_SELECT:
                bool left = GameManager.instance.Left();
                bool right = GameManager.instance.Right();

                if ((left || right) && MoveListUI.instance.GetSelectedMove().selectionType == Move.SelectionType.ALL)
                {
                    isEnemySelect = !isEnemySelect;
                    selectedEntity = 0;
                }
                
                int entities;
                if (isEnemySelect)
                {
                    entities = GameManager.instance.battleEnemies.Count;
                    entitySelect.transform.position = GameManager.instance.battleEnemies[selectedEntity].transform.position;
                }
                else
                {
                    entities = GameManager.instance.players.Count;
                    entitySelect.transform.position = GameManager.instance.players[selectedEntity].transform.position;
                }

                if (up)
                {
                    selectedEntity = (selectedEntity - 1 + entities) % entities;
                } else if (down)
                {
                    selectedEntity = (selectedEntity + 1) % entities;
                }

                if (cancel)
                {
                    CancelEntitySelect();
                } else if (confirm)
                {
                    Player player = GameManager.instance.players[selectedPlayer];
                    Entity target = isEnemySelect ? GameManager.instance.battleEnemies[selectedEntity] : GameManager.instance.players[selectedEntity];
                    Move move = MoveListUI.instance.GetSelectedMove();
                    state = State.ANIMATION;
                    attacked[player] = true;
                    StartCoroutine(PlayAttack(player, target, move));
                }
                break;
            case State.ANIMATION:
                break;
            case State.LEARN_MOVE:
                UpdatePlayerSelect(false);
                if (confirm)
                {
                    if (GetSelectedPlayer().moves.Count < 4)
                    {
                        GetSelectedPlayer().LearnMove(learnMove, 4);
                        GameManager.instance.EndBattle();
                    }
                    else
                    {
                        EnterReplaceMove();
                    }
                }
                break;
            case State.LEARN_MOVE_REPLACE:
                if (cancel)
                {
                    CancelReplaceMove();
                }

                if (confirm)
                {
                    GetSelectedPlayer().LearnMove(learnMove, MoveListUI.instance.selected);
                    GameManager.instance.EndBattle();
                }

                break;
        }
    }

    private IEnumerator PlayAttack(Player player, Entity target, Move move)
    {
        var anim = new BumpAnimation(player, target);
        yield return anim.Start();
        if (MoveListUI.instance.GetSelectedMove().selectionType == Move.SelectionType.ALL_ENEMY)
        {
            player.UseMove(move, GameManager.instance.battleEnemies.Cast<Entity>().ToList());
        }
        else
        {
            player.UseMove(move, target);
        }
        
        yield return anim.End();
        
        if (allAttacked())
        {
            EnterEnemyTurn();
        }
        else
        {
            NextPlayerAttack();
        }
    }
}
