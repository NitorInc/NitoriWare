using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageDebugKeys : MonoBehaviour
{
    [SerializeField]
    private KeyCode restartKey = KeyCode.R;
    [SerializeField]
    private KeyCode winMicrogameKey = KeyCode.S;
    [SerializeField]
    private KeyCode completeStageKey = KeyCode.P;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(restartKey))
            SceneManager.LoadScene(gameObject.scene.buildIndex);
        if (Input.GetKeyDown(completeStageKey))
            PrefsHelper.setProgress(PrefsHelper.GameProgress.StoryComplete);
        if (Input.GetKeyDown(winMicrogameKey) && MicrogameController.instance != null)
            MicrogameController.instance.setVictory(true);
    }
}
