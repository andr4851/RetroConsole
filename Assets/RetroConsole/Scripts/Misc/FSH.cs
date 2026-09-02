using System.IO;
using System.Collections.Generic;
using UnityEngine;
using static RetroConsole.Utility.ConstantsLibrary;

namespace RetroConsole.Utility
{
    public class FSH
    {
        [RuntimeInitializeOnLoadMethod]
        static void FileSystemCheck()
        {
            if (!Directory.Exists(ConsolePath))
            {
                Debug.LogAssertion("The main cataloge is not exist! Creating ...");
                Directory.CreateDirectory(ConsolePath);
                Debug.Log("The main cataloge created!");
            }

            foreach (string dir in ConsoleDirs)
            {
                if (!Directory.Exists(ConsolePath + dir))
                {
                    Debug.LogAssertion("The" + dir + "cataloge is not exist! Creating ...");
                    Directory.CreateDirectory(ConsolePath + dir);
                    Debug.Log("The " + dir + " cataloge created!");
                }
            }

            CheckRootsTermialRsFiles();
        }

        static string[] ConfigTemplare(string confName, string[] _params)
        {
            List<string> list = new List<string>();
            list.Add("#That config created automaticly, by FSH util");
            list.Add("[Name]");
            list.Add($"{confName}");
            list.Add("[Params]");

            foreach(string param in _params)
            {
                list.Add(param);
            }

            return list.ToArray();
        }

        static void CheckRootsTermialRsFiles()
        {
            if (!File.Exists($"{ConsolePath}{ConsoleDirs[0]}{rsFiles[0]}"))
                File.Create($"{ConsolePath}{ConsoleDirs[0]}{rsFiles[0]}").Close();
            if (!File.Exists($"{ConsolePath}{ConsoleDirs[0]}{rsFiles[2]}"))
                File.Create($"{ConsolePath}{ConsoleDirs[0]}{rsFiles[2]}").Close();
            if (!File.Exists($"{ConsolePath}{ConsoleDirs[1]}{rsFiles[1]}"))
                File.Create($"{ConsolePath}{ConsoleDirs[1]}{rsFiles[1]}").Close();
        }
    }
}
