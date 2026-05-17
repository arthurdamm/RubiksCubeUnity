using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class Logger
{
    private static Dictionary<string, int> _logOnceStrings = new();
    
    public static void LogFields(object obj)
    {
        var sb = new StringBuilder();
        foreach (var prop in obj.GetType().GetProperties())
        {
            sb.Append($"{prop.Name}: {prop.GetValue(obj)}, ");
        }
        Debug.Log(sb);
    }

    public static void LogOnce(string str)
    {
        if (!_logOnceStrings.ContainsKey(str))
        {
            _logOnceStrings[str] = 1;
            Debug.Log(str);
        }
    }
}

