using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private bool _showMaxHealthText;

    public void SetMaxHealth(int maxHealth)
    {
        _slider.maxValue = maxHealth;
        _slider.value = maxHealth;
    }

    public void SetHealth(int health)
    {
        _slider.value = health;

        if (_showMaxHealthText)
        {
            _text.text = $"{health} / {_slider.maxValue}";

            return;
        }

        _text.text = health.ToString();
    }
}
