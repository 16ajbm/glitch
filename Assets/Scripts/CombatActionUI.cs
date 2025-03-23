using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatActionUI : MonoBehaviour
{
    [SerializeField] private GameObject visualContainer;
    [SerializeField] private Button[] combatActionButtons;

    // This is terribly gross and should be refactored. Having this here
    // is giving the CombatActionUI class too much responsibility. Should ideally be
    // handled by the TurnManager and use events to decouple the classes.
    #region Rhythm
    [SerializeField] private BeatRoller beatRoller;
    #endregion

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

            if (i < character.CombatActions.Count)
            {
                combatActionButtons[i].gameObject.SetActive(true);
                CombatAction combatAction = character.CombatActions[i];

                combatActionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = combatAction.DisplayName;
                combatActionButtons[i].onClick.RemoveAllListeners();

                combatActionButtons[i].onClick.AddListener(() => OnClickCharacterCombatAction(character, combatAction));
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

    public void OnClickCharacterCombatAction(Character character, CombatAction combatAction)
    {
        if (character.isPlayer)
        {
            beatRoller.Score(combatAction.Length, (score) =>
            {
                Debug.Log($"Score: {score}");
                Debug.Log($"Combat Action original damage: {combatAction.Damage}");
                combatAction = combatAction.applyModifier(combatAction, score);
                Debug.Log($"Combat Action modified damage: {combatAction.Damage}");
                character.CastCombatAction(combatAction);
            });

            return;
        }

        TurnManager.Instance.CurrentCharacter.CastCombatAction(combatAction);
    }
}
