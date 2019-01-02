using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageGameOverGameModeLocalize : MonoBehaviour
{
	void Start ()
    {
        var stageName = gameObject.scene.name;
        var localizedName = TextHelper.getLocalizedText($"menu.gamemode.{stageName}".ToLower(), stageName);
        GetComponent<Text>().text = "Mode: " + localizedName;
    }
}
