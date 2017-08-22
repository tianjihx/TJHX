using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

class FileReader
{
    public static Dictionary<string, object> LoadConfigFile(string content)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        var lines = content.Split('\n');
        foreach (var line in lines)
        {
            if (line.Contains('='))
            {
                var pair = line.Split('=');
                var key = pair[0].Trim();
                var value = pair[1].Trim();
                int valueInt;
                if (int.TryParse(value, out valueInt))
                {
                    dict.Add(key, valueInt);
                }
                else
                {
                    dict.Add(key, value);
                }
            }
        }
        return dict;
    }

    
}
