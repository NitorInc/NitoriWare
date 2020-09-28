using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageDebugKeys : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(gameObject.scene.buildIndex);
        if (Input.GetKeyDown(KeyCode.P))
            PrefsHelper.setProgress(PrefsHelper.GameProgress.StoryComplete);
        if (Input.GetKeyDown(KeyCode.S) && MicrogameController.instance != null)
            MicrogameController.instance.setVictory(true);
    }
}
