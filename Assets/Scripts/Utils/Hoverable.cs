using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Utils
{
    public class Hoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public UnityEvent OnHoverEnter;
        public UnityEvent OnHoverExit;

        private void Awake()
        {
            OnHoverEnter = new UnityEvent();
            OnHoverExit = new UnityEvent();
        }

        private void OnDestroy()
        {
            OnHoverEnter.RemoveAllListeners();
            OnHoverExit.RemoveAllListeners();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnHoverEnter?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnHoverExit?.Invoke();
        }
    }
}
