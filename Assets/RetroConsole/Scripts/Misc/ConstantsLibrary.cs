namespace RetroConsole.Utility
{
    public static class ConstantsLibrary
    {
        #region String and char constants
        public static readonly string EmptyField = string.Empty;
        public static readonly string ConsolePath = $"{UnityEngine.Application.dataPath}\\RConsole";
        public static readonly string logPath = $"{UnityEngine.Application.dataPath}\\RConsole\\log";
        public static readonly string[] ConsoleDirs = {"\\log", "\\conf" };
        public static readonly string[] FileRes = { ".bin", ".dll", ".txt", ".sys", ".log", ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".bat", ".sh", ".cfg", ".rdk", ".sdk" };
        public static readonly string[] rsFiles = { "\\historyrc.rdk", "\\shellrc.rdk", "\\bufferrc.rdk" };
        public static readonly char CommentPointer = '#';

        #endregion

        #region Enums

        public enum WindowStatus
        {
            Active,
            Backgroud,
            Hidden,
            Pinned
        }

        public enum WindowStyle
        {
            Simple,
            SimpleWithIcon,
            OnlyClosable,
            NoControlls
        }

        public enum FilesType
        {
            Unrecognized,
            Binary,
            Library,
            Text,
            SystemFile,
            Log,
            Image,
            MSDOSScript,
            UnixShellScript,
            Config,
            RetroSDKConfig,
            RetroSDKScript
            //Append more types
        }

        public enum FTD
        {
            File,
            Directory
        }
    }

    #endregion
}
