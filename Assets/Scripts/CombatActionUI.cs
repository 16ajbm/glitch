using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatActionUI : MonoBehaviour
{
    #region Properties
    private bool abilityUsedThisTurn = false;
    #endregion

    #region UI
    [SerializeField] private GameObject visualContainer;
    [SerializeField] private Button[] combatActionButtons;
    #endregion

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
            return;

        // Reset flags & UI
        abilityUsedThisTurn = false;
        Debug.Log("Set combat action buttons interactable to true");
        SetCombatActionButtonsInteractable(true);
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
        if (abilityUsedThisTurn)
            return;

        abilityUsedThisTurn = true;
        Debug.Log("Set combat action buttons interactable to false");
        SetCombatActionButtonsInteractable(false);

        if (character.isPlayer)
        {
            Debug.Log($"OnClickCharcterCombatAction Character: {character.gameObject.name}, action: {combatAction.DisplayName}");
            beatRoller.Score(combatAction.Length, (scorePair) =>
            {
                float score = scorePair.Item1;
                float maxScore = scorePair.Item2;
                float modifier = 1 + score / maxScore;
                Debug.Log($"Modifier: {modifier}");
                combatAction = combatAction.applyModifier(combatAction, modifier);
                character.CastCombatAction(combatAction);
            });

            return;
        }

        TurnManager.Instance.CurrentCharacter.CastCombatAction(combatAction);
    }

    private void SetCombatActionButtonsInteractable(bool interactable)
    {
        foreach (var button in combatActionButtons)
            button.interactable = interactable;
    }
}
