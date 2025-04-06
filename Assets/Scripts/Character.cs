using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    #region Properties
    public int currentHealth;
    public bool isPlayer;
    public int maxHealth;
    #endregion

    #region Combat
    public List<CombatAction> CombatActions = new List<CombatAction>();
    [SerializeField] private Character target;
    #endregion

    #region UI
    private Vector3 startPosition;
    #endregion

    #region Events
    public event UnityAction OnHealthChange;
    public static event UnityAction<Character> OnDie;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
    }

    public void CastCombatAction(CombatAction combatAction)
    {
        if (combatAction.Damage > 0)
        {
            StartCoroutine(AttackOpponent(combatAction));
        }
        else if (combatAction.HealAmount > 0)
        {
            Heal(combatAction.HealAmount);
        }
    }

    public float GetHealthPercentage()
    {
        return (float)currentHealth / (float)maxHealth;
    }

    public void Heal(int HealAmount)
    {
        currentHealth += Math.Min(HealAmount, maxHealth - currentHealth);
        OnHealthChange?.Invoke();

        TurnManager.Instance.EndCurrentTurn();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnHealthChange?.Invoke();
        if (currentHealth <= 0) Die();
    }


    void Die()
    {
        OnDie?.Invoke(this);
        Destroy(gameObject);
    }

    IEnumerator AttackOpponent(CombatAction combatAction)
    {
        // This is going to be a bulky, dirty function... sorry!
        // Creating an ID was the simplest way to identify the animation to play
        // Couldn't use object name as it adds the suffix "(Clone)" when copied
        if (combatAction.ID == "fel_fire")
        {
            yield return MoveToPosition(target.transform.position, 50);

            // Raise the animation to appear about the middle of the character
            Vector3 animationOffset = new Vector3(0, 1.0f, 0);
            GameObject animation = Instantiate(combatAction.AnimationPrefab, target.transform.position + animationOffset, Quaternion.identity);

            // Not required but parenting it to the target would follow the target's movement if needed
            animation.transform.SetParent(target.transform, worldPositionStays: true);

            target.TakeDamage(combatAction.Damage);

            yield return MoveToPosition(startPosition, 25);
        }
        else
        {
            // Default animation if attack has no defined case to handle animation
            yield return MoveToPosition(target.transform.position, 50);

            target.TakeDamage(combatAction.Damage);

            yield return MoveToPosition(startPosition, 25);
        }

        TurnManager.Instance.EndCurrentTurn();
    }

    // Speed is an arbitrary value that will be multiplied by Time.deltaTime
    IEnumerator MoveToPosition(Vector3 targetPosition, float speed)
    {
        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
    }
}
