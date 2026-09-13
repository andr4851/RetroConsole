using UnityEngine;
using UnityEngine.InputSystem;
using RetroConsole.Extented;

namespace RetroConsole.Desktop
{
    public class Overlay : MonoBehaviour
    {
        [SerializeField]
        private GameObject Window;

        [SerializeField]
        private bool isVisible = false;
        private TerminalBuffer buffer;
        private CanvasGroup canvas;

        public static Overlay instance;

        

        private void Awake()
        {
            if (instance != null)
                Destroy(gameObject);
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);

                buffer = Window.GetComponentInChildren<TerminalBuffer>();
                canvas = GetComponent<CanvasGroup>();

                canvas.alpha = 0f;
                canvas.blocksRaycasts = false;
            }    
        }

        private void Update()
        {
            #if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                if (!isVisible)
                {
                    isVisible = true;
                    buffer.OverlayMessege(1);
                    canvas.alpha = 1;
                    canvas.blocksRaycasts = true;
                }
                else
                {
                    isVisible = false;
                    buffer.OverlayMessege(2);
                    canvas.alpha = 0;
                    canvas.blocksRaycasts = false;
                }
            }     

            #endif


            #if ENABLE_INPUT_SYSTEM
            if (Keyboard.current.backquoteKey.wasPressedThisFrame)
            {
                if (!isVisible)
                {
                    isVisible = true;
                    buffer.OverlayMessege(1);
                    canvas.alpha = 1;
                    canvas.blocksRaycasts = true;
                }
                else
                {
                    isVisible = false;
                    buffer.OverlayMessege(2);
                    canvas.alpha = 0;
                    canvas.blocksRaycasts = false;
                }
            }

            #endif

        }
    }
}
