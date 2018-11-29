using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    
    [SerializeField]
    protected string _targetStage;
    public string targetStage
    {
        get { return _targetStage; }
        set { targetStage = value; }
    }

    public virtual void press()
    {
        SceneManager.LoadScene(_targetStage);
    }
}
