using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableByShowcaseMode : MonoBehaviour
{

    [SerializeField]
    private bool enabledInShowcaseMode;
    
	void Start() => gameObject.SetActive(GameController.instance.ShowcaseMode == enabledInShowcaseMode);
}
