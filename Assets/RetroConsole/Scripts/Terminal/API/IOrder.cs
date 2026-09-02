namespace RetroConsole.Extented
{
    public interface IOrder
    {
        public abstract void Init();
        public abstract void OnInputEnter(string input);
        public abstract void OnExit();
        public abstract void OnArrowUp();
        public abstract void OnArrowDown();
    }
}
