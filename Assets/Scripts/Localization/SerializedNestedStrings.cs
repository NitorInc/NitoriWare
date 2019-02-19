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
            subData = new Dictionary<string, StringData>();
        }
    }

    private StringData rootData;

    public SerializedNestedStrings()
    {
        rootData = new StringData();
    }

    public object getData()
    {
        return rootData;
    }

    public string this[string index]
    {
        get
        {
            return string.IsNullOrEmpty(index) ? null : getString(rootData, new List<string>(index.Split('.')));
        }
        set
        {
            setString(rootData, new List<string>(index.Split('.')), value);
        }
    }

    private string getString(StringData baseData, List<string> keys)
    {
        if (keys.Count == 1)
        {
            //We are at deepest nesting level
            return baseData.subData.ContainsKey(keys[0]) ? baseData.subData[keys[0]].value : null;
        }
        else
        {
            //We are not at deepest nesting level
            string currentKey = keys[0];
            keys.RemoveAt(0);
            return baseData.subData.ContainsKey(currentKey) ? getString((baseData.subData[currentKey]), keys) : null;
        }
    }

    private void setString(StringData baseData, List<string> keys, string value)
    {
        if (keys.Count == 1)
        {
            //We are at deepest nesting level
            if (!baseData.subData.ContainsKey(keys[0]))
                baseData.subData[keys[0]] = new StringData();
            baseData.subData[keys[0]].value = value;
        }
        else if (baseData.subData.ContainsKey(keys[0]))
        {
            //Not at deepest nesting level but dictionary with same name exists
            StringData existingData = baseData.subData[keys[0]];
            keys.RemoveAt(0);
            setString(existingData, keys, value);
        }
        else
        {
            //Not at deepest nesting level and dictionary with same name does not exist
            StringData newData = new StringData();
            string currentKey = keys[0];
            keys.RemoveAt(0);
            baseData.subData[currentKey] = newData;
            setString(newData, keys, value);
        }
    }

    public override string ToString()
    {
        string result = serialize(rootData, 0);
        return result == "" ? result : result.Substring(0, result.Length - 1);
    }

    string serialize(StringData baseData, int nestLevel)
    {
        //Minimalist serialization, tabs for group hierarchy, "key=value" at lowest level
        string result = "";
        foreach (KeyValuePair<string, StringData> entry in baseData.subData)
        {
            result += new string('\t', nestLevel);
            result += entry.Key;
                result +=
                (!string.IsNullOrEmpty(entry.Value.value) ? "=" + (string)entry.Value.value : "")
                + "\n";
            if (entry.Value.subData.Count > 0)
            {
                result += serialize(entry.Value, nestLevel + 1);
            }
        }
        return result;
    }

    public static SerializedNestedStrings deserialize(string serializedData, SerializedNestedStrings existingStrings = null)
    {
        serializedData = serializedData.Replace(((char)13).ToString(), ""); //Remove any instances of carriage return
        if (existingStrings == null)
            existingStrings = new SerializedNestedStrings();

        deserializeData(existingStrings.rootData, serializedData.Split('\n'), 0, 0);
        return existingStrings;
    }

    private static int deserializeData(StringData baseData, string[] lines, int startLine, int tabCount)
    {
        string line = lines[startLine];

        for (int i = startLine; i < lines.Length; i++)
        {
            line = lines[i];
            if (getTabCount(line) < tabCount)   //We haven't finished recursing and line tab count is less than expected
                return i - 1;   //Return previous line so we we can reevaulate this line on previous level
            
            string key = Regex.Replace(lines[i].Split('=')[0], @"^\t*", "");    //String between leading tabs and "="
            StringData newData = new StringData();
            baseData.subData[key] = newData;
            if (line.Contains("="))
            {
                //line is string value, add to deserialized data
                string value = Regex.Replace(line, @"^[^=]*=(.*)", @"$1");         //String after first "="
                newData.value = value;
            }

            if (i + 1 < lines.Length)
            {
                int nextLineTabCount = getTabCount(lines[i + 1]);
                if (nextLineTabCount > tabCount)
                {
                    //Next line has more tabs than this one, recurse into next level
                    i = deserializeData(newData, lines, i + 1, tabCount + 1);
                }
                else if (nextLineTabCount < tabCount)
                {
                    //Next line is no longer on or beneath this level, return what line we're on
                    return i;
                }
            }
        }
        return lines.Length;
    }

    static int getTabCount(string line)
    {
        return Regex.Match(line, @"^\t*").Value.Length;
    }
}