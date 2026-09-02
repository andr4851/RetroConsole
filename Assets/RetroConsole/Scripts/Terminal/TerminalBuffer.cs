using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static RetroConsole.Utility.ConstantsLibrary;

namespace RetroConsole.Extented
{
    [AddComponentMenu("RetroConsole/Application/Extentions/Terminal buffer")]
    public class TerminalBuffer : TMP_InputFieldMod
    {
        #region Variables
        [SerializeField]
        public string inputBuffer;
        public string outputBuffer;

        private string buffer = string.Empty, format;
        private int counter = 0;
        private bool makeNewLine = true, block = false;
        private string fullBuffer;
        private string currentHistoryPath = $"{logPath}{rsFiles[2]}";

        private ScrollRect rect;

        public IOrder order = null;

        #endregion

        #region Unity functions
        protected override void Awake()
        {
            rect = GetComponentInParent<ScrollRect>();

            onValueChanged.AddListener(OnInput);
            onEndEdit.AddListener(OnSubmit);

            onDeselect.AddListener(SetBlockTrue);
            onSelect.AddListener(SetBlockFalse);
        }

        private void Update()
        {
            if (caretPosition < Regex.Replace(text, "<.*?>", string.Empty).Length - counter)
                OnEnd();

            if (Input.GetKeyDown(KeyCode.UpArrow))
                order.OnArrowUp();

            if (Input.GetKeyDown(KeyCode.DownArrow))
                order.OnArrowDown();

            if (Input.GetKeyDown(KeyCode.End))
                OnEnd();
        }

        private void OnApplicationQuit() =>
            System.IO.File.AppendAllText(currentHistoryPath, fullBuffer);

        protected override void OnDestroy()
        {
            base.OnDestroy();
            System.IO.File.AppendAllText(currentHistoryPath, fullBuffer);
        }

        #endregion

        #region Buffer API
        public void BufferInit(string _format, string _print = "")
        {
            SetFormat(_format);

            if (_print != string.Empty)
                Print(_print);

            NewLine(format);

            buffer = text;
            counter = 0;
        }

        public void ClearBuffer()
        {
            buffer = string.Empty;
            text = "";
            counter = 0;

            makeNewLine = false;
        }

        public void Print(object input)
        {
            text += input.ToString();
            rect.verticalNormalizedPosition = 0;
            fullBuffer += input.ToString();
            outputBuffer = input.ToString();
        }

        public void PrintLine(object input)
        {
            text += $"\n{input.ToString()}";
            rect.verticalNormalizedPosition = 0;
            fullBuffer += input.ToString();
            outputBuffer = input.ToString();
        }

        public void SetOrder(IOrder _order) =>
            order = _order;

        public void SetFormat(string _format) =>
            format = _format;

        public void OnEnd() =>
            caretPosition = text.Length;

        public void InsertInput(string newInput)
        {
            SetInputBuffer();
            if (inputBuffer != string.Empty)
            {
                text = text.Remove(text.Length - counter, counter);
                text += newInput;
            }
            else
                text += newInput;

            caretPosition = text.Length;
            counter = newInput.Length;
        }
        public void SetReadOnly()
        {
            readOnly = true;
            interactable = false;
            DeactivateInputField();
        }

        public void ExecuteWithoutVisual(string command)
        {
            if (command == "")
            {
                inputBuffer = string.Empty;
                NewLine(format);
                buffer = text;

                counter = 0;

                if (!block)
                {
                    OnEnd();
                    ActivateInputField();
                }

                rect.verticalNormalizedPosition = 0;

                return;
            }

            order.OnInputEnter(command);

            if (makeNewLine)
                NewLine(format);
            else
            {
                NewLine(format, false);
                makeNewLine = true;
            }

            counter = 0;

            if (!block)
            {
                OnEnd();
                ActivateInputField();
            }

            rect.verticalNormalizedPosition = 0;
        }

        #endregion

        #region Text manipulation
        protected override void Backspace()
        {
            if (counter <= 0)
                return;

            if (caretPosition > Regex.Replace(text, "<.*?>", string.Empty).Length - counter)
                base.Backspace();
            
            if (caretPosition > Regex.Replace(text, "<.*?>", string.Empty).Length - counter)
                counter--;
        }

        protected override void DeleteKey()
        {
            if (counter <= 0)
                return;

            if (caretPosition > Regex.Replace(text, "<.*?>", string.Empty).Length - counter)
                base.DeleteKey();
            if(caretPosition != Regex.Replace(text, "<.*?>", string.Empty).Length && caretPosition > Regex.Replace(text, "<.*?>", string.Empty).Length - counter)
                counter--;
        }

        #endregion

        #region Internal function
        private void NewLine(string lineFormat, bool newLine = true)
        {
            if (newLine)
                text += $"\n{lineFormat}:";
            else
                text += $"{lineFormat}:";

        }

        private void OnInput(string s)
        {
            rect.verticalNormalizedPosition = 0;

            if (Input.GetKey(KeyCode.Backspace))
                return;

            if (Input.GetKey(KeyCode.Delete))
                return;

            counter++;
        }

        private void OnSubmit(string s)
        {
            SetInputBuffer();

            if (inputBuffer == "")
            {
                inputBuffer = string.Empty;
                NewLine(format);
                buffer = text;

                counter = 0;

                if (!block)
                {
                    OnEnd();
                    ActivateInputField();
                }

                rect.verticalNormalizedPosition = 0;

                return;
            }

            order.OnInputEnter(inputBuffer);
            inputBuffer = string.Empty;

            if (makeNewLine)
                NewLine(format);
            else
            {
                NewLine(format, false);
                makeNewLine = true;
            }
            buffer = text;

            counter = 0;

            if (!block)
            {
                OnEnd();
                ActivateInputField();
            }

            rect.verticalNormalizedPosition = 0;
        }

        private void SetInputBuffer() =>
            inputBuffer = text.Replace(buffer, string.Empty);

        private void SetBlockTrue(string s) =>
            block = true;

        private void SetBlockFalse(string s) => 
            block = false;
        
        #endregion
    }
}
