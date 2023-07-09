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

        [SerializeField] private Image _offensiveHighlight;
        [SerializeField] private Image _defensiveHighlight;
        [SerializeField] private Image _specialHighlight;

        [SerializeField] private Image _slashHighlight;
        [SerializeField] private Image _shieldHighlight;
        [SerializeField] private Image _slashFlurryHighlight;

        [SerializeField] private Image _fireballHighlight;
        [SerializeField] private Image _forceFieldHighlight;
        [SerializeField] private Image _lightningNukeHighlight;

        [SerializeField] private Image _arrowRainHighlight;
        [SerializeField] private Image _ropeSwitchHighlight;
        [SerializeField] private Image _critMarkHighlight;

        [SerializeField] private Image _staffHitHighlight;
        [SerializeField] private Image _healHighlight;
        [SerializeField] private Image _multiHealHighlight;

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

        public void HighlightVerb(Verb verb)
        {
            switch (verb)
            {
                case Verb.Offensive:
                    _offensiveHighlight.enabled = true;
                    break;
                case Verb.Defensive:
                    _defensiveHighlight.enabled = true;
                    break;
                case Verb.Special:
                    _specialHighlight.enabled = true;
                    break;
            }
        }

        public void HighlightAbility(Ability ability)
        {
            switch (ability.name)
            {
                case "Slash":
                    _slashHighlight.enabled = true;
                    break;
                case "Shield":
                    _shieldHighlight.enabled = true;
                    break;
                case "SlashFlurry":
                    _slashFlurryHighlight.enabled = true;
                    break;

                case "Fireball":
                    _fireballHighlight.enabled = true;
                    break;
                case "ForceField":
                    _forceFieldHighlight.enabled = true;
                    break;
                case "LightningNuke":
                    _lightningNukeHighlight.enabled = true;
                    break;

                case "ArrowRain":
                    _arrowRainHighlight.enabled = true;
                    break;
                case "RopeSwitch":
                    _ropeSwitchHighlight.enabled = true;
                    break;
                case "CritMark":
                    _critMarkHighlight.enabled = true;
                    break;

                case "StaffHit":
                    _staffHitHighlight.enabled = true;
                    break;
                case "Heal":
                    _healHighlight.enabled = true;
                    break;
                case "MultiHeal":
                    _multiHealHighlight.enabled = true;
                    break;
            }
        }

        public void ClearHighlights()
        {
            _warriorRowHighlight.enabled = false;
            _mageRowHighlight.enabled = false;
            _rangerRowHighlight.enabled = false;
            _clericRowHighlight.enabled = false;

            _offensiveHighlight.enabled = false;
            _defensiveHighlight.enabled = false;
            _specialHighlight.enabled = false;

            _slashHighlight.enabled = false;
            _shieldHighlight.enabled = false;
            _slashFlurryHighlight.enabled = false;

            _fireballHighlight.enabled = false;
            _forceFieldHighlight.enabled = false;
            _lightningNukeHighlight.enabled = false;

            _arrowRainHighlight.enabled = false;
            _ropeSwitchHighlight.enabled = false;
            _critMarkHighlight.enabled = false;

            _staffHitHighlight.enabled = false;
            _healHighlight.enabled = false;
            _multiHealHighlight.enabled = false;
        }
	}
}