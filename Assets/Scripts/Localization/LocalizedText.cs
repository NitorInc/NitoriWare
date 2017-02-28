using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
	[SerializeField]
	private Category category;
	[SerializeField]
	private string _key;
	public string key
	{
		get {return _key;}
		set { _key = value; setText(); }
	}

	private Text text;
	private TextMesh textMesh;

	private enum Category
	{
		None,
		CurrentMicrogame
	}

	void Start ()
	{
		text = GetComponent<Text>();
		textMesh = GetComponent<TextMesh>();
		setText();
	}

	/// <summary>
	/// Sets the key to load from and reloads the text with the new key
	/// </summary>
	/// <param name="key"></param>
	public void setKey(string key)
	{
		this._key = key;
		setText();
	}

	void setText()
	{
		if (LocalizationManager.instance == null)
			return;

		string fullKey = getPrefixString() + key, value = LocalizationManager.instance.getLocalizedValue(fullKey);
		if (value.Equals(LocalizationManager.NotFoundString))
		{

			Debug.LogWarning("No or empty localization value found for " + key + ", text not changed");
			return;
		}

		if (text != null)
			text.text = value;
		else if (textMesh != null)
			textMesh.text = value;
	}

	string getPrefixString()
	{
		switch(category)
		{
			case (Category.CurrentMicrogame):
				return "microgame." + gameObject.scene.name.Substring(0, gameObject.scene.name.Length - 1) + ".";
			default:
				return "";
		}
	}
}
