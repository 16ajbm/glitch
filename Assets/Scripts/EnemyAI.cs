using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private AnimationCurve healChanceCurve;
    [SerializeField] private Character character;


    void OnEnable()
    {
        TurnManager.Instance.OnTurnStart.AddListener(OnBeginTurn);
    }

    void OnDisable()
    {
        TurnManager.Instance.OnTurnStart.RemoveListener(OnBeginTurn);
    }

    void OnBeginTurn(Character turnCharacter)
    {
        if (character == turnCharacter)
        {
            DetermineCombatAction();
        }
    }

    void DetermineCombatAction()
    {
        float healthPercentage = character.GetHealthPercentage();

        bool shouldHeal = Random.value < healChanceCurve.Evaluate(healthPercentage);

        CombatAction combatAction = null;

        if (shouldHeal && HasCombatActionOfType(CombatAction.Type.Heal))
        {
            combatAction = GetCombatActionOfType(CombatAction.Type.Heal);
        }
        else if (HasCombatActionOfType(CombatAction.Type.Attack))
        {
            combatAction = GetCombatActionOfType(CombatAction.Type.Attack);
        }

        if (combatAction != null)
        {
            character.CastCombatAction(combatAction);
        }
        else
        {
            TurnManager.Instance.EndCurrentTurn();
        }
    }

    bool HasCombatActionOfType(CombatAction.Type type)
    {
        return character.CombatActions.Exists(action => action.ActionType == type);
    }

    CombatAction GetCombatActionOfType(CombatAction.Type type)
    {
        List<CombatAction> availableActions = character.CombatActions.FindAll(action => action.ActionType == type);

        return availableActions[Random.Range(0, availableActions.Count)];
    }
}
