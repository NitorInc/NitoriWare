using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

//Class is used like a dicationary : instance[key] = value
//Key can have nested values denoted by ".", automatically organizing them hierarchically (i.e. "microgame.ChenFood.command")
//Uses homebrew minimal serialization
//Serialization uses tabs for group indentation, key=value for string value lines
//ToString() returns serialized data
//No array support as of yet

class SerializedNestedStrings
{
	private Dictionary<string, object> data;

	public SerializedNestedStrings()
	{
		data = new Dictionary<string, object>();
	}

	public object getData()
	{
		return data;
	}

	public object this[string index]
	{
		get
		{
			return string.IsNullOrEmpty(index) ? null : getObject(data, new List<string>(index.Split('.')));
		}
		set
		{
			setObject(data, new List<string>(index.Split('.')), value);
		}
	}

	private object getObject(Dictionary<string, object> baseObject, List<string> keys)
	{
		if (keys.Count == 1)
		{
			//We are at deepest nesting level
			return baseObject.ContainsKey(keys[0]) ? baseObject[keys[0]] : null;
		}
		else
		{
			//We are not at deepest nesting level
			string currentKey = keys[0];
			keys.RemoveAt(0);
			return baseObject.ContainsKey(currentKey) ? getObject((Dictionary<string, object>)(baseObject[currentKey]), keys) : null;
		}
	}

	private void setObject(Dictionary<string, object> baseObject, List<string> keys, object value)
	{
		if (keys.Count == 1)
		{
			//We are at deepest nesting level
			baseObject[keys[0]] = value;
		}
		else if (baseObject.ContainsKey(keys[0]))
		{
			//Not at deepest nesting level but dictionary with same name exists
			Dictionary<string, object> existingObject = (Dictionary<string, object>)(baseObject[keys[0]]);
			keys.RemoveAt(0);
			setObject(existingObject, keys, value);
		}
		else
		{
			//Not at deepest nesting level and dictionary with same name does not exist
			Dictionary<string, object> newBase = new Dictionary<string, object>();
			string currentKey = keys[0];
			keys.RemoveAt(0);
			baseObject[currentKey] = newBase;
			setObject(newBase, keys, value);
		}
	}

	public override string ToString()
	{
		string result = serialize(data, 0);
		return result == "" ? result : result.Substring(0, result.Length - 1);
	}

	string serialize(Dictionary<string, object> baseObject, int nestLevel)
	{
		//Minimalist serialization, tabs for group hierarchy, "key=value" at lowest level
		string result = "";
		foreach (KeyValuePair<string, object> entry in baseObject)
		{
			result += new string('\t', nestLevel);
			if (entry.Value.GetType() == typeof(string))
				result += entry.Key + "=" + (string)entry.Value + "\n";
			else
			{
				result += entry.Key + "\n";
				result += serialize((Dictionary<string, object>)(entry.Value), nestLevel + 1);
			}
		}
		return result;
	}

	public static SerializedNestedStrings deserialize(string serializedData)
	{
		List<string> lines = new List<string>(serializedData.Split('\n'));
		SerializedNestedStrings newData = new SerializedNestedStrings();

		string currentKeyBase = "";
		for (int i = 0; i < lines.Count; i++)
		{
			string line = lines[i];
			int tabCount = Regex.Match(line, @"^\t*").Value.Length,
				dotCount = currentKeyBase.Split('.').Length - 1;
			while (tabCount <  dotCount)
			{
				//Key is up from previous line in hierarchy
				//Remove "{key}." from current hierarchy string to go up one level
				if (dotCount == 1)
					currentKeyBase = "";
				else
					currentKeyBase = Regex.Replace(currentKeyBase, @"(.*)\.[^\.]*.$", @"$1.");
				dotCount--;
			}
			if (line.Contains("="))
			{
				//line is string value, add to deserialized data
				string key = Regex.Replace(line.Split('=')[0], @"^\t*", ""),
					value = Regex.Replace(line, @"^[^=]*=(.*)", @"$1");
				newData[currentKeyBase + key] = value;
			}
			else
			{
				//line is beginning of a group, increase nest level by adding line to keystring base
				currentKeyBase += Regex.Replace(line, @"^\t*", "") + ".";
				
			}
		}

		return newData;
	}
}