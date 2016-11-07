using UnityEngine;
using System.Collections;

public class MicrogameInfoParser : MonoBehaviour
{
	private MicrogameInfo[] info;
	private string[] text;
	private int lineIndex;

	public struct MicrogameInfo
	{
		public string name;
		public string[] commands;
		public MicrogameController.ControlScheme[] controlSchemes;
	}


	void Start ()
	{
		parseData();
	}

	void parseData()
	{
		TextAsset textAsset = Resources.Load("MicrogameInfo") as TextAsset;
		text = textAsset.text.Split('\n');
		lineIndex = 0;

		int microgameCount = int.Parse(getInfoFromLine(getNextLine()));
		info = new MicrogameInfo[microgameCount];

		for (int i = 0; i < microgameCount; i++)
		{
			info[i].controlSchemes = new MicrogameController.ControlScheme[3];
			info[i].commands = new string[3];

			string line = getNextLine();
			while (line == "")
			{
				line = getNextLine();
			}

			for (int j = 0; j < 3; j++)
			{
				info[i].name = line;
			}

			line = getNextLine();
			if (line == "1")
			{
				for (int j = 0; j < 3; j++)
				{
					info[i].controlSchemes[j] = getControlSchemeFromLine(getNextLine());
					info[i].commands[j] = getInfoFromLine(getNextLine());
					lineIndex++;
				}
			}
			else
			{
				MicrogameController.ControlScheme controlScheme = getControlSchemeFromLine(line);
				string command = getInfoFromLine(getNextLine());
				for (int j = 0; j < 3; j++)
				{
					info[i].controlSchemes[j] = controlScheme;
					info[i].commands[j] = command;
				}
			}
			
		}
	}

	public MicrogameInfo getMicrogameInfo(string name)
	{
		for (int i = 0; i < info.Length; i++)
		{
			if (info[i].name == name)
			{
				return info[i];
			}
		}
		Debug.Log("Microgame " + name + " not found! Make sure you add it to the list in Assets/Resources and increase Count!");
		return info[0];
	}

	string getNextLine()
	{
		string line = text[lineIndex++];
		if (line.Length > 0 && (int)line.ToCharArray()[line.Length - 1] == 13)
			return line.Remove(line.Length - 1);
		else
			return line;
	}

	string getInfoFromLine(string line)
	{
		string[] lines = line.Split(':');

		string newLine = "";
		newLine += lines[1].Substring(1, lines[1].Length - 1);

		for (int i = 2; i < lines.Length; i++)
		{
			newLine += lines[i];
		}

		return newLine;
	}

	MicrogameController.ControlScheme getControlSchemeFromLine(string line)
	{
		line = getInfoFromLine(line);
		switch(line.ToLower())
		{
			case("keys"):
				return MicrogameController.ControlScheme.Touhou;
			case("mouse"):
				return MicrogameController.ControlScheme.Mouse;
			default:
				return (MicrogameController.ControlScheme)0;
		}
	}
}
