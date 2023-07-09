using Assets.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class RoleTable : MonoBehaviour
    {
        [SerializeField] private Image _warriorRowHighlight;
		[SerializeField] private Image _mageRowHighlight;
		[SerializeField] private Image _rangerRowHighlight;
		[SerializeField] private Image _clericRowHighlight;

		public void HighlightRole(Role role)
		{
			switch (role)
			{
				case Role.Warrior:
					_warriorRowHighlight.enabled = true;
					break;
                case Role.Mage:
                    _mageRowHighlight.enabled = true;
                    break;
                case Role.Ranger:
                    _rangerRowHighlight.enabled = true;
                    break;
                case Role.Cleric:
                    _clericRowHighlight.enabled = true;
                    break;
            }
		}

        public void ClearHighlights()
        {
            _warriorRowHighlight.enabled = false;
            _mageRowHighlight.enabled = false;
            _rangerRowHighlight.enabled = false;
            _clericRowHighlight.enabled = false;
        }
	}
}