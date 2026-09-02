using System;
using UnityEngine;
using UnityEngine.UI;
using static RetroConsole.Utility.ConstantsLibrary;
using RetroConsole.Desktop;

namespace RetroConsole.Windows
{
    [AddComponentMenu("RetroSDK/Window/Window")]
    public class Window : MonoBehaviour
    {
        #region Variables

        public GameObject resizeArea, ico, statusBarObj;
        [SerializeField]
        private GameObject CloseButton, fullscreenButton, minimizeButton, hideButton;

        [Space(10)]
        [Header("Window sizes")]
        public Vector2 originalSize;
        public Vector2 minSize = new (150, 150), maxSize = new (300, 300);


        [Space(20)]

        [Header("Window preferences")]
        public WindowStatus windowStatus;
        [SerializeField]
        private WindowStyle windowStyle;

        [SerializeField]
        public string title = "Untitled";

        [SerializeField]
        private Sprite iconImage = null, defaultIcon;
        [SerializeField]
        private Image icoHardRef;


        [SerializeField]
        private bool fullscreen = false, resizable = true, statusBar = false;

        [Space(20)]

        [SerializeField, NonReorderable]
        private Sprite[] windowVar = new Sprite[4];

        private readonly Sprite[] usingSprites = new Sprite[2];

        private Image windowBackgroundMain, icoImg;

        private readonly int pixeSafeZone = 40;

        private float bufferedWidth, bufferedHeight, bufferedPosX, bufferedPosY;

        private StatusBar statusBarScript;
        private WindowResize windowResize;

        //[HideInInspector]
        public int index = 0;

        public event Action<object> SendMessege;
        public event Action Clear;

        #endregion

        #region Unity functions
        private void Awake()
        {
            windowBackgroundMain = GetComponent<Image>();

            if(icoHardRef == null)
                icoImg = ico.GetComponent<Image>();
            statusBarScript = GetComponentInChildren<StatusBar>();
            windowResize = GetComponentInChildren<WindowResize>();

            if (!resizable)
            {
                resizeArea.SetActive(false);
                usingSprites[0] = windowVar[0];
                usingSprites[1] = windowVar[1];
                windowBackgroundMain.sprite = usingSprites[0];
            }
            else
            {
                resizeArea.SetActive(true);
                usingSprites[0] = windowVar[2];
                usingSprites[1] = windowVar[3];
                windowBackgroundMain.sprite = usingSprites[0];
            }

            if (statusBar)
                statusBarObj.SetActive(true);
            else
                statusBarObj.SetActive(false);

            if (iconImage != null)
                if (icoHardRef != null)
                    icoHardRef.sprite = iconImage;
                else
                icoImg.sprite = iconImage;
            else
                if (icoHardRef != null)
                    icoHardRef.sprite = defaultIcon;
                else
                    icoImg.sprite = defaultIcon;

            switch (windowStyle)
            {
                case WindowStyle.Simple:
                    ico.SetActive(false);
                    break;
                case WindowStyle.SimpleWithIcon:
                    ico.SetActive(true);
                    break;
                case WindowStyle.OnlyClosable:
                    ico.SetActive(false);
                    hideButton.SetActive(false);
                    fullscreenButton.SetActive(false);
                    minimizeButton.SetActive(false);
                    break;
                case WindowStyle.NoControlls:
                    ico.SetActive(false);
                    hideButton.SetActive(false);
                    fullscreenButton.SetActive(false);
                    minimizeButton.SetActive(false);
                    CloseButton.SetActive(false);
                    break;
            }
        }

        private void Update()
        {
            if (transform.GetSiblingIndex() == GetComponentInParent<Workzone>().transform.childCount - 1)
            {
                //if (windowDesign == WindowDesign.LunaBlue && fullscreen != false)
                //    windowBackgroundMain.sprite = usingSprites[4];
                //else
                //    windowBackgroundMain.sprite = usingSprites[0];
                windowStatus = WindowStatus.Active;
            }
            else
            {
                //if (windowDesign == WindowDesign.LunaBlue && fullscreen != false)
                //    windowBackgroundMain.sprite = usingSprites[5];
                //else
                //    windowBackgroundMain.sprite = usingSprites[1];
                
                windowStatus = WindowStatus.Backgroud;
            }
            if (!fullscreen)
            {
                bufferedHeight = GetComponent<RectTransform>().sizeDelta.y;
                bufferedWidth = GetComponent<RectTransform>().sizeDelta.x;
                bufferedPosX = GetComponent<RectTransform>().transform.localPosition.x;
                bufferedPosY = GetComponent<RectTransform>().transform.localPosition.y;
            }
        }

        #endregion

        #region Window API function
        public void CheckOutScreen()
        {
            GetComponent<RectTransform>().pivot = new Vector2 (0.5f, 0.5f);

            if (transform.position.x > Screen.width)
            {
                transform.position = new Vector3(Screen.width - pixeSafeZone, transform.position.y);
            }
            else if (transform.position.x <= 0)
            {
                transform.position = new Vector3(pixeSafeZone, transform.position.y);
            }
            if (transform.position.y > Screen.height)
            {
                transform.position = new Vector3(transform.position.x, Screen.height - pixeSafeZone);
            }
            else if (transform.position.y <= 0)
            {
                transform.position = new Vector3(transform.position.x, pixeSafeZone);
            }

            GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);
        }

        /// <summary>
        /// Makes this window fullscreen or else return it to previuse size
        /// </summary>
        public void Fullscreen()
        {
            fullscreen = !fullscreen;

            if (fullscreen)
            {
                transform.localPosition = new Vector3(0f, 0f);
                GetComponent<RectTransform>().sizeDelta = GetComponentInParent<Workzone>().sizeDelta;
            }
            else
            {
                transform.localPosition = new Vector3(bufferedPosX, bufferedPosY);
                GetComponent<RectTransform>().sizeDelta = new Vector2(bufferedWidth, bufferedHeight);
            }
        }

        /// <summary>
        /// Close this window and delete this object from the scene
        /// </summary>
        public void Close() =>
            Destroy(gameObject);

        /// <summary>
        /// Convert your data and print it in status bar
        /// </summary>
        /// <param name="data">Your data, what is goint to be shows in status bar</param>
        public void SendMessegeToStatusBar(object data)
        {
            if (statusBar)
                SendMessege?.Invoke(data);
        }

        /// <summary>
        /// Just remove all text from status bar
        /// </summary>
        public void ClearStatusBar() =>
            Clear?.Invoke();

        #endregion
    }
}
