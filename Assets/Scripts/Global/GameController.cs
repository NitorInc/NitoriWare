using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	public static GameController instance;

#pragma warning disable 0649
    [SerializeField]
    private bool disableCursor;
    [SerializeField]
	private MicrogameCollection _microgameCollection;
#pragma warning restore 0649

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

		Cursor.visible = !disableCursor;
        Application.targetFrameRate = 60;
	}
}
