using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class InfoDisplayManager : MonoBehaviour
    {
        [SerializeField] private Image _portraitImage;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Image _borderImage;
        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private TextMeshProUGUI _descriptionText;

        private void Awake()
        {
            Hide();
        }

        public void SetAbility(Ability ability)
        {
            _portraitImage.sprite = ability.IconSprite;
            _headerText.text = ability.name;
            _descriptionText.text = ability.Description;

            Show();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}