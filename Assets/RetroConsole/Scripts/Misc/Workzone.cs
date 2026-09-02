using UnityEngine;

namespace RetroConsole.Desktop
{
    [AddComponentMenu("RetroSDK/Desktop/Workzone")]
    public class Workzone : MonoBehaviour
    {
        [HideInInspector]
        public Vector2 sizeDelta;

        private void Start()
        {
            sizeDelta = new Vector2(Screen.width / GetComponentInParent<Canvas>().GetComponent<RectTransform>().lossyScale.x, Screen.height / GetComponentInParent<Canvas>().GetComponent<RectTransform>().lossyScale.y);
        }
    }
}
