using UnityEngine;

namespace RetroConsole.Desktop
{
    public class Overlay : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
