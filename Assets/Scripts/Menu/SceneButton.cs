using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    protected string targetStage;
#pragma warning restore 0649

    public virtual void press()
    {
        SceneManager.LoadScene(targetStage);
    }
}
