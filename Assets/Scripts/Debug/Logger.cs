using System.Text;
using UnityEngine;

public static class Logger
{
    public static void LogFields(object obj)
    {
        var sb = new StringBuilder();
        foreach (var prop in obj.GetType().GetProperties())
        {
            sb.Append($"{prop.Name}: {prop.GetValue(obj)}, ");
        }
        Debug.Log(sb);
    }
}
