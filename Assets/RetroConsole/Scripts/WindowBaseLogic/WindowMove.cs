using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace RetroConsole.Windows
{
    [AddComponentMenu("RetroConsole/Window/Window move")]
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
            #if ENABLE_LEGACY_INPUT_MANAGER
            mouseStartingPos = Input.mousePosition;

            #endif

            #if ENABLE_INPUT_SYSTEM
            mouseStartingPos = Mouse.current.position.value;
            m_IsDragging = true;

            #endif
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (m_IsDragging)
            {
                #if ENABLE_LEGACY_INPUT_MANAGER
                m_RectTransform.position -= mouseStartingPos - Input.mousePosition;
                mouseStartingPos = Input.mousePosition;

                #endif

                #if ENABLE_INPUT_SYSTEM
                m_RectTransform.position -= mouseStartingPos - new Vector3 (Mouse.current.position.value.x, Mouse.current.position.value.y);
                mouseStartingPos = Mouse.current.position.value;
                
                #endif

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