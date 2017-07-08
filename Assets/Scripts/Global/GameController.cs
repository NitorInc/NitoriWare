using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections;

public class GameController : MonoBehaviour
{
	public static GameController instance;

#pragma warning disable 0649
    [SerializeField]
    private bool disableCursor;
    [SerializeField]
	private MicrogameCollection _microgameCollection;
    [SerializeField]
    private Sprite[] controlSprites;
    [SerializeField]
    private UnityEvent onSceneLoad;
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
        AudioListener.pause = false;
        SceneManager.sceneLoaded += onSceneLoaded;
    }

    void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (PauseManager.exitedWhilePaused)
        {
            AudioListener.pause = false;
            Time.timeScale = 1f;
            PauseManager.exitedWhilePaused = false;
            Cursor.visible = true;
        }
        onSceneLoad.Invoke();
    }

    public Sprite getControlSprite(MicrogameTraits.ControlScheme controlScheme)
    {
        return controlSprites[(int)controlScheme];
    }
}
