using UnityEngine;
using TMPro;

public class EnergySystem : MonoBehaviour
{
    public int maxEnergy = 10;          // 10 usos totales
    public int current;
    public TMP_Text uiText;             // opcional

    void Awake()
    {
        current = maxEnergy;
        UpdateUI();
    }

    public bool TryConsume(int amount)
    {
        if (current < amount) return false;
        current -= amount;
        UpdateUI();
        if (current <= 0) GameManager.I.GameOver();
        return true;
    }

    public void Restore(int amount)
    {
        current = Mathf.Clamp(current + amount, 0, maxEnergy);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (uiText) uiText.text = current.ToString();
    }
}
