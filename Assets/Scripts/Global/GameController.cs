using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public const CursorLockMode DefaultCursorMode = CursorLockMode.None;

#pragma warning disable 0649
    [SerializeField]
    private bool disableCursor;
    [SerializeField]
    private SceneShifter _sceneShifter;
    [SerializeField]
    private Sprite[] controlSprites;
    [SerializeField]
    private UnityEvent onSceneLoad;
    [SerializeField]
    private DiscordController _discord;
    [SerializeField]
    private bool showcaseMode;
#pragma warning restore 0649

    private string startScene;
    
    public SceneShifter sceneShifter => _sceneShifter;
    public DiscordController discord => _discord;
    public bool ShowcaseMode => showcaseMode;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
			return;
        }

        startScene = gameObject.scene.name;
        transform.parent = null;
        DontDestroyOnLoad(transform.gameObject);
		instance = this;

		Cursor.visible = !disableCursor;
        Cursor.lockState = DefaultCursorMode;
        Application.targetFrameRate = 60;
        AudioListener.pause = false;
        SceneManager.sceneLoaded += onSceneLoaded;
        PrefsHelper.initiate();
    }

    private void Update()
    {
        //Debug features
        if (Debug.isDebugBuild)
        {
            //Shift+R to reset all prefs
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.R))
                PlayerPrefs.DeleteAll();
        }
    }

    void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (PauseManager.exitedWhilePaused)
        {
            AudioListener.pause = false;
            Time.timeScale = 1f;
            Cursor.visible = true;
            Cursor.lockState = DefaultCursorMode;

            PauseManager.exitedWhilePaused = false;
        }
        onSceneLoad.Invoke();
    }

    public Sprite getControlSprite(Microgame.ControlScheme controlScheme)
    {
        return controlSprites[(int)controlScheme];
    }

    public string getStartScene() => startScene;
}
