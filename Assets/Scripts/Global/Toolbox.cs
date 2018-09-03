using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Needed to make sure Unity loads our ScriptableObjects
public class Toolbox : MonoBehaviour
{
    [SerializeField]
    private List<ScriptableObject> scriptableObjects;
}
