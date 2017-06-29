using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	public static GameController instance;

	[SerializeField]
	private MicrogameCollection _microgameCollection;
	public MicrogameCollection microgameCollection
	{
		get { return _microgameCollection; }
		set {}
	}

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(transform.gameObject);
		instance = this;

		Cursor.visible = false;
        Application.targetFrameRate = 60;
	}
}
