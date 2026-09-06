using UnityEngine;
using RetroConsole.Extented;

namespace RetroConsole.Console.Commands
{
    [AddComponentMenu("RetroConsole/Terminal/Log")]
    public class Log: TerminalCommand, IOrder
    {
        #region Variables
        [SerializeField]
        private GameObject currentTerminal;

        [SerializeField]
        private bool inWork = false;

        #endregion

        #region Unity functions
        private void OnApplicationQuit() =>
            ResetVars();

        #endregion

        #region API
        public override void Init()
        {
            if (inWork != false || currentTerminal == null)
            {
                buffer.SetFormat(string.Empty);
                buffer.ClearBuffer();

                inWork = true;
                currentTerminal = master.gameObject;
                Application.logMessageReceived += Getter;
            }
            else
            {
                buffer.PrintLine("<color=red>There can't be more then one active logger!\nClose the window with active logger and try again</color>");
                OnExit();
            }
        }

        public override void OnInputEnter(string input)
        {
            //This void is plug
        }

        public override void OnExit()
        {
            buffer.PrintLine("The logger was Interrupted!");
            Application.logMessageReceived -= Getter;

            buffer.SetOrder(master);

            buffer.SetFormat($"unity@{Application.productName}");
        }

        public override void OnCtrlC()
        {
            ResetVars();
            OnExit();
        }

        #endregion

        #region Internal functions
        private void Getter (string _messege, string _stack, LogType _logType)
        {
            switch (_logType)
            {
                case LogType.Error:
                    buffer.PrintLine($"<color=red>[ERROR]{_messege} in {_stack}</color>");
                    break;
                case LogType.Warning:
                    buffer.PrintLine($"<color=yellow>[WARNING]{_messege} in {_stack}</color>");
                    break;
                case LogType.Log:
                    buffer.PrintLine($"<color=green>[LOG]{_messege} in {_stack}</color>");
                    break;
                case LogType.Assert:
                    buffer.PrintLine($"<color=red>[ASSERT]{_messege} in {_stack}</color>");
                    break;
                case LogType.Exception:
                    buffer.PrintLine($"<color=red>[EXCEPTION]{_messege} in {_stack}</color>");
                    break;
            }

        }

        private void ResetVars()
        {
            currentTerminal = null;
            inWork = false;

            input = string.Empty;
            separatedinput = null;
        }

        #endregion
    }
}
