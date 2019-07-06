using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HeadersDictionary : SerializableDictionary<string, string>
{
    /// <summary>
    /// return list of "key=value"
    /// </summary>
    public List<string> GetFormmatedHeadersList() {
        if (this.Count == 0) return null;
        var list = new List<string>(this.Count);
        //cause allocation but this will not be used frequently
        foreach (var header in this) { list.Add($"{header.Key}={header.Value}"); }

        return list;
    }
}

public static partial class Utils
{
    /// <summary>
    /// Only for data look like this "data=data&info=info"
    /// </summary>
    /// <returns></returns>
    public static HeadersDictionary GetHeadersFromUriData(this string text) {
        if (!text.Contains("&")) return null;
        try {
            HeadersDictionary headers = new HeadersDictionary();

            foreach (var s in text.Split('&')) {
                var index = s.Split('=');
                headers.Add(index[0], index[1]);
            }
            return headers;
        }
        catch (Exception e) { Debug.LogError(e); }

        return null;
    }
}