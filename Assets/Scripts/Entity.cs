/*
@Author - Craig
@Description - Handles all entities
*/

using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public int health;
    public int mana = 0;
    public List<StatusEffect> statusEffects = new List<StatusEffect>();
    public List<Move> moves = new List<Move>();

    public int GetHealth()
    {
        return health;
    }

    public abstract EntityInfo GetEntityInfo();

    public void UseMove(Move move, Entity target)
    {
        UseMove(move, new List<Entity> {target});
    }

    public void UseMove(Move move, List<Entity> target)
    {
        for (int i = 0; i < move.costs.Count; i++)
        {
            switch (move.costs[i])
            {
                case Move.Cost.MANA:
                    mana -= move.costAmounts[i];
                    break;
                case Move.Cost.HEALTH:
                    health -= move.costAmounts[i];
                    break;
            }
        }

        foreach (Entity entity in target) {
            for (int i = 0; i < move.effects.Count; i++)
                {
                    switch (move.effects[i])
                    {
                        case Move.Effect.HEAL:
                            entity.Heal(move.effectAmounts[i]);
                            break;
                        case Move.Effect.DAMAGE:
                            entity.Damage((int)(move.effectAmounts[i] *
                                                Mathf.Pow(1.3f, GetEffectAmount(StatusEffectType.STRENGTH))));
                            break;
                        case Move.Effect.APPLY_STRENGTH:
                            entity.ApplyStatusEffect(new StatusEffect(StatusEffectType.STRENGTH, move.effectAmounts[i],
                                1));
                            break;
                        case Move.Effect.APPLY_WEAKNESS:
                            entity.ApplyStatusEffect(new StatusEffect(StatusEffectType.WEAKNESS, move.effectAmounts[i],
                                1));
                            break;
                        case Move.Effect.APPLY_IMMUNITY:
                            entity.ApplyImmunity();
                            break;
                        case Move.Effect.APPLY_BLOCK_SELF:
                            ApplyStatusEffect(new StatusEffect(StatusEffectType.BLOCK, move.effectAmounts[i], 1));
                            break;
                    }
                }
            }
        }
    
    public void TickStatusEffects() {
        for (int i = 0; i < statusEffects.Count; i++) {
            statusEffects[i].duration--;
            if (statusEffects[i].duration <= 0) {
                statusEffects.RemoveAt(i);
                i--;
            }
        }
    }
    
    public bool CanUseMove(Move move)
    {
        for (int i = 0; i < move.costs.Count; i++)
        {
            switch (move.costs[i])
            {
                case Move.Cost.MANA:
                    if (move.costAmounts[i] > mana)
                    {
                        return false;
                    }
                    break;
                case Move.Cost.HEALTH:
                    if (move.costAmounts[i] > health)
                    {
                        return false;
                    }

                    break;
            }
        }

        return true;
    }

    private void ConsumeStatusEffect(StatusEffectType effectType, int amount)
    {
        foreach (StatusEffect effect in statusEffects)
        {
            if (effect.effectType == effectType)
            {
                int consumed = Math.Min(amount, effect.amount);
                effect.amount -= consumed;
                amount -= consumed;
            }
        }
        statusEffects.RemoveAll(e => e.amount <= 0);
    }
    public void Damage(int damage)
    {
        int block = GetEffectAmount(StatusEffectType.BLOCK);
        if (block > 0)
        {
            ConsumeStatusEffect(StatusEffectType.BLOCK, damage);
            damage -= block;
            damage = Math.Max(0, damage);
        }
        health -= (int) (Mathf.Pow(1.3f, GetEffectAmount(StatusEffectType.WEAKNESS)) * damage);
    }

    public void Heal(int amount)
    {
        health += amount;
        health = Math.Min(health, GetEntityInfo().health);
    }
    
    public void ApplyStatusEffect(StatusEffect effect) {
        statusEffects.Add(effect);
    }
    
    public int GetEffectAmount(StatusEffectType type)
    {
        int ans = 0;
        foreach (StatusEffect effect in statusEffects)
        {
            if (effect.effectType == type)
            {
                ans += effect.amount;
            }
        }

        return ans;
    }

    public void ApplyImmunity()
    {
        foreach (StatusEffect effect in statusEffects)
        {
            effect.amount--;
        }
        statusEffects.RemoveAll(e => e.amount <= 0);
    }
    
    public void StartBattle()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    protected virtual void Start()
    {
        foreach (Move move in GetEntityInfo().moves)
        {
            moves.Add(move);
        }
        health = GetEntityInfo().health;
    }

    public void LearnMove(Move move, int index)
    {
        if (index >= moves.Count)
        {
            moves.Add(move);
        }
        else
        {
            moves[index] = move;
        }
    }
}
