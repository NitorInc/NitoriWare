using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicrogameText : MonoBehaviour
{
	[SerializeField]
	private string _key;
	public string key
	{
		get { return _key; }
		set { _key = value; setText(); }
	}

	private Text text;
	private TextMesh textMesh;

	void Start()
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
		this.key = key;
		setText();
	}

	void setText()
	{
		if (LocalizationManager.instance == null)
			return;

		string fullKey = "microgame." + gameObject.scene.name.Substring(0, gameObject.scene.name.Length - 1) + "." + key;
		if (text != null)
			text.text = LocalizationManager.instance.getLocalizedValue(fullKey);
		else if (textMesh != null)
			textMesh.text = LocalizationManager.instance.getLocalizedValue(fullKey);
	}
}
