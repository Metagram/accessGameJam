/*
@Author - Craig
@Description - 
*/

public class StatusEffect
{
    public StatusEffectType effectType;
    public int amount;
    public int duration;
    
    public StatusEffect(StatusEffectType effectType, int amount, int duration)
    {
        this.effectType = effectType;
        this.amount = amount;
        this.duration = duration;
    }

    // public StatusEffect Copy()
    // {
    //     return new StatusEffect(effectType, amount, duration);
    // }
    //
    // public override bool Equals(object obj)
    // {
    //     if (obj == null || GetType() != obj.GetType())
    //     {
    //         return false;
    //     }
    //
    //     StatusEffect other = (StatusEffect) obj;
    //     return effectType == other.effectType && amount == other.amount && duration == other.duration;
    // }
}
