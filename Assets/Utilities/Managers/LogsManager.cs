///
/// Copyright (c) 2024 Vikas Reddy Thota
///

using UnityEngine;
using GameUtilities;

namespace GameManagers
{

    /// <summary>
    /// Class can be use to add logs
    /// it make easy to turn off the logs for different environment
    /// </summary>
    public class LogsManager
    {

        [System.Diagnostics.Conditional(UtilitiesConstants.EnableLogs)]
        public static void Print(object message) => Debug.Log(message);
        [System.Diagnostics.Conditional(UtilitiesConstants.EnableLogs)]
        public static void Print(object message, Color color)
        {
            string colorCode = Utility.GetHexCodeFromColor(color);
            Debug.Log($"<color={colorCode}> {message} </color>");
        }


        [System.Diagnostics.Conditional(UtilitiesConstants.EnableLogs)]
        public static void PrintException(System.Exception ex) => Debug.LogException(ex);
        [System.Diagnostics.Conditional(UtilitiesConstants.EnableLogs)]
        public static void PrintException(object ex) => Debug.Log(ex);
    }
}