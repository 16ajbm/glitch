using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatActionUI : MonoBehaviour
{
    [SerializeField] private GameObject visualContainer;
    [SerializeField] private Button[] combatActionButtons;

    void OnEnable()
    {
        TurnManager.Instance.OnTurnStart.AddListener(OnBeginTurn);
        TurnManager.Instance.OnTurnEnd.AddListener(OnEndTurn);
    }

    void OnDisable()
    {
        TurnManager.Instance.OnTurnStart.RemoveListener(OnBeginTurn);
        TurnManager.Instance.OnTurnEnd.RemoveListener(OnEndTurn);
    }

    void OnBeginTurn(Character character)
    {
        if (!character.isPlayer)
        {
            return;
        }

        visualContainer.SetActive(true);


        for (int i = 0; i < combatActionButtons.Length; i++)
        {
            Debug.Log("CombatActionUI: " + i + " " + character.CombatActions.Count);

            if (i < character.CombatActions.Count)
            {
                combatActionButtons[i].gameObject.SetActive(true);
                CombatAction combatAction = character.CombatActions[i];

                combatActionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = combatAction.DisplayName;
                combatActionButtons[i].onClick.RemoveAllListeners();

                combatActionButtons[i].onClick.AddListener(() => OnClickCombatAction(combatAction));
            }
            else
            {
                combatActionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void OnEndTurn(Character character)
    {
        visualContainer.SetActive(false);
    }

    public void OnClickCombatAction(CombatAction combatAction)
    {
        TurnManager.Instance.CurrentCharacter.CastCombatAction(combatAction);
    }
}
