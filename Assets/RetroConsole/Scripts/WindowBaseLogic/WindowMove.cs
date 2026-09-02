using UnityEngine;
using UnityEngine.EventSystems;

namespace RetroConsole.Windows
{
    [AddComponentMenu("RetroSDK/Window/Window move")]
    public class WindowMove : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
    {
        public Window parentalWindow;

        public RectTransform m_RectTransform;

        private Vector3 mouseStartingPos;
        private bool m_IsDragging;

        private void Awake()
        {
            m_RectTransform = transform.parent.GetComponent<RectTransform>();
            Window window = GetComponentInParent<Window>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            mouseStartingPos = Input.mousePosition;
            m_IsDragging = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (m_IsDragging)
            {
                m_RectTransform.position -= mouseStartingPos - Input.mousePosition;
                mouseStartingPos = Input.mousePosition;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            parentalWindow.CheckOutScreen();
            m_IsDragging = false;
        }

        public void OnPointerDown(PointerEventData eventData) =>
            m_RectTransform.SetAsLastSibling();
    }
}