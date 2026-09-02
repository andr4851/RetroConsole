using UnityEngine;
using UnityEngine.EventSystems;

namespace RetroConsole.Windows
{
    [AddComponentMenu("RetroSDK/Window/Window resize")]
    public class WindowResize : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        private Vector2 minSize, maxSize;

        private RectTransform rectTransform;
        private Vector2 currentPointerPosition, previousPointerPosition;

        private void Awake()
        {
            rectTransform = transform.parent.GetComponent<RectTransform>();
            minSize = transform.parent.GetComponent<Window>().minSize;
            maxSize = transform.parent.GetComponent<Window>().maxSize;
        }

        public void OnPointerDown(PointerEventData data)
        {
            rectTransform.SetAsLastSibling();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, data.position, data.pressEventCamera, out previousPointerPosition);
        }

        public void OnDrag(PointerEventData data)
        {
            if (rectTransform == null)
                return;

            Vector2 sizeDelta = rectTransform.sizeDelta;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, data.position, data.pressEventCamera, out currentPointerPosition);
            Vector2 resizeValue = currentPointerPosition - previousPointerPosition;

            sizeDelta += new Vector2(resizeValue.x, -resizeValue.y);
            sizeDelta = new Vector2(Mathf.Clamp(sizeDelta.x, minSize.x, maxSize.x), Mathf.Clamp(sizeDelta.y, minSize.y, maxSize.y));

            rectTransform.sizeDelta = sizeDelta;

            previousPointerPosition = currentPointerPosition;
        }
    }
}
