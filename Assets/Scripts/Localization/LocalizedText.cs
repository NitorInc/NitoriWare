using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
	[SerializeField]
	private Prefix keyPrefix;
	[SerializeField]
	private string _key;
    [SerializeField]
    private Parameter[] parameters;

	public string key
	{
		get {return _key;}
		set { _key = value; updateText(); }
    }

    [System.Serializable]
    public struct Parameter
    {
        public string value;
        public bool isKey;
        public string keyDefaultString;
    }

    private Text textComponent;
	private TextMesh textMesh;
    private string language;

	private enum Prefix
	{
		None,
		CurrentMicrogame
	}

	void Start ()
	{
		textComponent = GetComponent<Text>();
		textMesh = GetComponent<TextMesh>();
        language = "";
        updateText();
    }

    private void Update()
    {
        
        if (language != TextHelper.getLoadedLanguage())
            updateText();
    }

    /// <summary>
    /// Sets the key to load from and reloads the text with the new key
    /// </summary>
    /// <param name="key"></param>
    public void setKey(string key)
	{
		this._key = key;
		updateText();
	}

	public void updateText()
	{
        language = TextHelper.getLoadedLanguage();
        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(language))
            return;

		string value;
		if (keyPrefix == Prefix.CurrentMicrogame)
			value = TextHelper.getLocalizedMicrogameText(key, getText());
		else
			value = TextHelper.getLocalizedText(getPrefixedKey(), getText());

        string[] parameterStrings = new string[parameters.Length];
        for (int i = 0; i < parameters.Length; i++)
        {
            Parameter parameter = parameters[i];
            parameterStrings[i] = parameter.isKey ? TextHelper.getLocalizedText(parameter.value, parameter.keyDefaultString) : parameter.value;
        }
        value = string.Format(value, parameterStrings);

		setText(value);
	}

	private void setText(string text)
	{
		if (textComponent != null)
			textComponent.text = text;
		else if (textMesh != null)
			textMesh.text = text;
	}

	private string getText()
	{
		if (textComponent != null)
			return textComponent.text;
		if (textMesh != null)
			return textMesh.text;
		return "";
	}

	string getPrefixedKey()
	{
		switch(keyPrefix)
		{
			//Handled seperately
			//case (Prefix.CurrentMicrogame):
			//	return "microgame." + gameObject.scene.name.Substring(0, gameObject.scene.name.Length - 1) + ".";
			default:
				return key;
		}
	}
}
