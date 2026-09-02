using UnityEngine;
using TMPro;
using static RetroConsole.Utility.ConstantsLibrary;

namespace RetroConsole.Windows
{
    [AddComponentMenu("RetroSDK/Window/StatusBar")]
    public class StatusBar : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text text;

        private void Awake()
        {
            text.text = EmptyField;
            GetComponentInParent<Window>().SendMessege += Print;
            GetComponentInParent<Window>().Clear += Clear;
        }

        public void Print(object data) =>
            text.text = data.ToString();

        public void Clear() =>
            text.text = EmptyField;
    }
}
