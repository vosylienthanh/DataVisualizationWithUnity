using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;

public class CSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    /// <summary>
    /// Source: https://bravenewmethod.com/2014/09/13/lightweight-csv-reader-for-unity/
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static List<Dictionary<string, object>> ReadCSVFile(string fileName)
    {
        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
        //var data = Resources.Load<TextAsset>(fileName);
        var data = File.ReadAllText(fileName);

        var lines = Regex.Split(data, LINE_SPLIT_RE);

        if (lines.Length <= 1)
        {
            return list;
        }

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (int i = 0; i < lines.Length; ++i)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);

            if (values.Length == 0 || values[0] == "")
            {
                continue;
            }

            var entry = new Dictionary<string, object>();
            for (int j = 0; j < values.Length && j < header.Length; ++j)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");

                object finalValue = value;
                int n;
                float f;

                if (int.TryParse(value, out n))
                {
                    finalValue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalValue = f;
                }
                entry[header[j]] = finalValue;
            }

            list.Add(entry);
        }

        return list;
    }
}
