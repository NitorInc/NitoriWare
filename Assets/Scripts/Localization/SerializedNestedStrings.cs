using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

//Class is used like a dicationary : instance[key] = value
//Key can have nested values denoted by ".", automatically organizing them hierarchically (i.e. "microgame.ChenFood.command")
//Uses homebrew minimal serialization
//Serialization uses tabs for group indentation, key=value for string value lines
//ToString() returns serialized data

public class SerializedNestedStrings
{
    private class StringData
    {
        public string value;
        public Dictionary<string, StringData> subData;

        public StringData()
        {
            subData = new Dictionary<string, StringData>(StringComparer.OrdinalIgnoreCase);
        }
    }

    private Dictionary<string, StringData> data;

    public SerializedNestedStrings()
    {
        data = new Dictionary<string, StringData>(StringComparer.OrdinalIgnoreCase);
    }

    public object getData()
    {
        return data;
    }

    public string this[string index]
    {
        get
        {
            return string.IsNullOrEmpty(index) ? null : getString(data, new List<string>(index.Split('.')));
        }
        set
        {
            setString(data, new List<string>(index.Split('.')), value);
        }
    }

    private string getString(Dictionary<string, StringData> baseObject, List<string> keys)
    {
        if (keys.Count == 1)
        {
            //We are at deepest nesting level
            return baseObject.ContainsKey(keys[0]) ? baseObject[keys[0]].value : null;
        }
        else
        {
            //We are not at deepest nesting level
            string currentKey = keys[0];
            keys.RemoveAt(0);
            var thing = baseObject[currentKey];
            return baseObject.ContainsKey(currentKey) ? getString((baseObject[currentKey].subData), keys) : null;
        }
    }

    private void setString(Dictionary<string, StringData> baseObject, List<string> keys, string value)
    {
        if (keys.Count == 1)
        {
            //We are at deepest nesting level
            if (!baseObject.ContainsKey(keys[0]))
                baseObject[keys[0]] = new StringData();
            baseObject[keys[0]].value = value;
        }
        else if (baseObject.ContainsKey(keys[0]))
        {
            //Not at deepest nesting level but dictionary with same name exists
            Dictionary<string, StringData> existingObject = baseObject[keys[0]].subData;
            keys.RemoveAt(0);
            setString(existingObject, keys, value);
        }
        else
        {
            //Not at deepest nesting level and dictionary with same name does not exist
            StringData newData = new StringData();
            string currentKey = keys[0];
            keys.RemoveAt(0);
            baseObject[currentKey] = newData;
            setString(newData.subData, keys, value);
        }
    }

    public override string ToString()
    {
        string result = serialize(data, 0);
        return result == "" ? result : result.Substring(0, result.Length - 1);
    }

    string serialize(Dictionary<string, StringData> baseObject, int nestLevel)
    {
        //Minimalist serialization, tabs for group hierarchy, "key=value" at lowest level
        string result = "";
        foreach (KeyValuePair<string, StringData> entry in baseObject)
        {
            result += new string('\t', nestLevel);
            result += entry.Key + (string.IsNullOrEmpty(entry.Value.value) ? "" : "=" + entry.Value.value) + "\n";
            if (entry.Value.subData.Count > 0)
            {
                result += serialize(entry.Value.subData, nestLevel + 1);
            }
        }
        return result;
    }

    public static SerializedNestedStrings deserialize(string serializedData)
    {
        serializedData = serializedData.Replace(((char)13).ToString(), ""); //Remove any instances of carriage return

        List<string> lines = new List<string>(serializedData.Split('\n'));
        SerializedNestedStrings newData = new SerializedNestedStrings();

        string currentKeyBase = "";
        for (int i = 0; i < lines.Count; i++)
        {
            string line = lines[i];
            int tabCount = getTabCount(line),
                dotCount = currentKeyBase.Split('.').Length - 1;
            while (tabCount < dotCount)
            {
                //Key is up from previous line in hierarchy
                //Remove "{key}." from current hierarchy string to go up one level
                if (dotCount == 1)
                    currentKeyBase = "";
                else
                    currentKeyBase = Regex.Replace(currentKeyBase, @"(.*)\.[^\.]*.$", @"$1.");      //Replace last ".[string]." with "."
                dotCount--;
            }
            if (line.Contains("="))
            {
                //line is string value, add to deserialized data
                string key = Regex.Replace(line.Split('=')[0], @"^\t*", ""),    //String between leading tabs and "="
                    value = Regex.Replace(line, @"^[^=]*=(.*)", @"$1");         //String after first "="
                newData[currentKeyBase + key] = value;
            }
            if (i < lines.Count - 1 && getTabCount(lines[i + 1]) > tabCount)
            {
                //line is beginning of a group because next line has more tabs (and exists), increase nest level by adding line to keystring base
                currentKeyBase += Regex.Replace(line.Split('=')[0], @"^\t*", "") + ".";     //String between leading tabs and "=" (if there is one)

            }
        }

        return newData;
    }

    //public List<string> flatten()
    //{
    //    return flatten(data);
    //}

    //List<string> flatten(Dictionary<string, StringData> data)
    //{
    //    var returnList = from subdata in data
    //                     where !string.IsNullOrEmpty(subdata.Value.value)
    //                     select subdata;
    //    var returnList = data.Where(a => !string.IsNullOrEmpty(a.Value.value)).Select(a => a.Value.value).ToList();
    //    foreach (var subList in data.Select(a => a.Value.subData).Where(a => a != null && a.Any()))
    //    {
    //        returnList.AddRange(flatten(subList));
    //    }
    //    return returnList;
    //}

    static int getTabCount(string line)
    {
        return Regex.Match(line, @"^\t*").Value.Length;
    }
}