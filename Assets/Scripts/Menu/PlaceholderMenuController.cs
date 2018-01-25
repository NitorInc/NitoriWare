using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaceholderMenuController : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private bool dontEnableCursor;
#pragma warning restore 0649

	void Start()
	{
        if (!dontEnableCursor)
            Cursor.visible = true;
	}
	
	void Update()
	{
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SceneManager.LoadScene("Nitori");
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SceneManager.LoadScene("Compilation");
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            SceneManager.LoadScene("CompilationFast");
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            SceneManager.LoadScene("CompilationMystery");
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            SceneManager.LoadScene("CompilationHard");
    }
}
