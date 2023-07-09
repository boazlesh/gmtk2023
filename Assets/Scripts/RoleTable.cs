using Assets.Scripts.Enums;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
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

        [SerializeField] private List<Ability> _abilities;

        private Dictionary<Verb, Image> _verbToImage;
        private Dictionary<Role, Image> _roleToImage;
        private Dictionary<string, Image> _abilityToImage;
        private Dictionary<string, Ability> _abilityMap;

        public event Action<Ability> OnAbilityHovered;
        public event Action<Ability> OnAbilityUnhovered;

        private void Awake()
        {
            _verbToImage = new Dictionary<Verb, Image>()
            {
                { Verb.Offensive, _offensiveHighlight },
                { Verb.Defensive, _defensiveHighlight },
                { Verb.Special, _specialHighlight },
            };

            _roleToImage = new Dictionary<Role, Image>()
            {
                { Role.Warrior, _warriorRowHighlight },
                { Role.Mage, _mageRowHighlight },
                { Role.Ranger, _rangerRowHighlight },
                { Role.Cleric, _clericRowHighlight },
            };

            _abilityToImage = new Dictionary<string, Image>()
            {
                { "Slash", _slashHighlight },
                { "Shield", _shieldHighlight },
                { "SlashFlurry", _slashFlurryHighlight },
                { "Fireball", _fireballHighlight },
                { "ForceField", _forceFieldHighlight },
                { "LightningNuke", _lightningNukeHighlight },
                { "ArrowRain", _arrowRainHighlight },
                { "RopeSwitch", _ropeSwitchHighlight },
                { "CritMark", _critMarkHighlight },
                { "StaffHit", _staffHitHighlight },
                { "Heal", _healHighlight },
                { "MultiHeal", _multiHealHighlight }
            };

            _abilityMap = _abilities.ToDictionary(x => x.name);

            foreach (string abilityName in _abilityToImage.Keys)
            {
                Ability ability = _abilityMap[abilityName];

                Hoverable hoverable = _abilityToImage[abilityName].GetComponentInParent<Hoverable>();

                hoverable.OnHoverEnter.AddListener(() =>
                {
                    if (OnAbilityHovered != null)
                    {
                        OnAbilityHovered(ability);
                    }
                });

                hoverable.OnHoverExit.AddListener(() =>
                {
                    if (OnAbilityUnhovered != null)
                    {
                        OnAbilityUnhovered(ability);
                    }
                });
            }
        }

        public void HighlightRole(Role role)
		{
            _roleToImage[role].enabled = true;
		}

        public void HighlightVerb(Verb verb)
        {
            _verbToImage[verb].enabled = true;
        }

        public void HighlightAbility(Ability ability)
        {
            _abilityToImage[ability.name].enabled = true;
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