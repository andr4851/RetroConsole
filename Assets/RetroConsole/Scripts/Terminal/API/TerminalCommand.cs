using UnityEngine;
using RetroConsole.Extented;

namespace RetroConsole.Console
{
    [AddComponentMenu("RetroSDK/Terminal/Terminal command")]
    public class TerminalCommand : MonoBehaviour, IOrder
    {
        #region Variables
        public string format = "Test format";

        public string input;
        public string[] separatedinput;

        protected TerminalBuffer buffer;
        protected TerminalMaster master;

        #endregion

        #region API
        public virtual void Init()
        {
            buffer.SetFormat(format);
            buffer.PrintLine("This the init text");
        }

        public virtual void OnInputEnter(string input)
        {
            buffer.PrintLine("This is the demo output of external command, overide the <color=yellow>OnInputEnter(string input)</color> function in your child script for making your own output");
            OnExit();
        }

        public virtual void OnExit()
        {
            buffer.PrintLine("When command ends its execution the <color=yellow>OnExit()</color> functions calls and return controll to Termial master script");
            buffer.SetOrder(master);

            buffer.SetFormat($"unity@{Application.productName}");
        }
        public void OnArrowUp() =>
            buffer.InsertInput("Arrow up");

        public void OnArrowDown() =>
            buffer.InsertInput("Arrow down");

        #endregion

        #region References
        public void SetTerminalReference(TerminalMaster _master) =>
            master = _master;

        public void SetBufferReference(TerminalBuffer _buffer) =>
            buffer = _buffer;

        #endregion

    }
}
