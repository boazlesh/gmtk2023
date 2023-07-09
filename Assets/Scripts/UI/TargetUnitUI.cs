using UnityEngine;

namespace Assets.Scripts
{
    public class TargetUnitUI : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer _spriteRenderer;

		private Unit _targetUnit;

		public Unit TargetUnit => _targetUnit;

		public void SetTarget(Unit targetUnit)
		{
			_targetUnit = targetUnit;
			_spriteRenderer.sprite = targetUnit.IconSprite;
            _spriteRenderer.flipX = !targetUnit.IsPlayerUnit;
        }
	}
}