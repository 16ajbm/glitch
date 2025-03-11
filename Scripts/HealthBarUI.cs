using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image healthFiller;
    [SerializeField] private Character character;

    void OnEnable()
    {
        character.OnHealthChange += OnHealthUpdate;
    }

    void OnDisable()
    {
        character.OnHealthChange -= OnHealthUpdate;
    }

    void Start()
    {
        OnHealthUpdate();
    }

    void OnHealthUpdate()
    {
        healthFiller.fillAmount = character.GetHealthPercentage();
    }
}
