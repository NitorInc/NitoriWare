using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
	[SerializeField]
	private string _key;
	public string key
	{
		get {return _key;}
		set { _key = value; setText(); }
	}

	private Text text;
	private TextMesh textMesh;

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

		if (text != null)
			text.text = LocalizationManager.instance.getLocalizedValue(_key);
		else if (textMesh != null)
			textMesh.text = LocalizationManager.instance.getLocalizedValue(_key);
	}
}
