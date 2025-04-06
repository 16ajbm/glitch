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

    IEnumerator AttackOpponent(CombatAction combatAction)
    {
        // This is going to be a bulky, dirty function... sorry!
        if (combatAction.ID == "fel_fire")
        {
            while (transform.position != target.transform.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 50 * Time.deltaTime);
                yield return null;
            }

            // Raise the animation to appear about the middle of the character
            Vector3 animationOffset = new Vector3(0, 1.0f, 0);

            GameObject animation = Instantiate(combatAction.AnimationPrefab, target.transform.position + animationOffset, Quaternion.identity);

            // Not required but parenting it to the target would follow the target's movement if needed
            animation.transform.SetParent(target.transform, worldPositionStays: true);


            target.TakeDamage(combatAction.Damage);

            while (transform.position != startPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPosition, 25 * Time.deltaTime);
                yield return null;
            }
        }

        TurnManager.Instance.EndCurrentTurn();
    }

    void Die()
    {
        OnDie?.Invoke(this);
        Destroy(gameObject);
    }
}
