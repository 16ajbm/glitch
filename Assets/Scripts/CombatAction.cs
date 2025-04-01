using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Combat Action", menuName = "New Combat Action")]
public class CombatAction : ScriptableObject
{
    public enum Type
    {
        Attack,
        Heal
    }

    public string DisplayName;
    public Type ActionType;

    [Header("Damage")]
    public int Damage;

    [Header("Heal")]
    public int HealAmount;

    [Header("Pattern Length")]
    public int Length = 1;

    public CombatAction applyModifier(CombatAction combatAction, float modifier)
    {
        // Instantiate a new CombatAction to avoid modifying the original
         CombatAction modifiableAction = Instantiate(combatAction);

        if (modifiableAction.ActionType == Type.Heal)
        {
            // Modifier can be directly applied to heal amount since it is a non-nullable integer
            modifiableAction.HealAmount = Math.Max(1, (int)(modifiableAction.HealAmount * modifier));
        }

        if (modifiableAction.ActionType == Type.Attack)
        {
            // Modifier can be directly applied to damage since it is a non-nullable integer
            modifiableAction.Damage = Math.Max(1, (int)(modifiableAction.Damage * modifier));
        }

        return modifiableAction;
    }
}
