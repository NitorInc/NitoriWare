using System.Collections.Generic;
using UnityEngine;

public class HecShapesPool : MonoBehaviour
{
    
    [SerializeField]
    HecShapesSlottable slottableTemplate;

    [SerializeField]
    List<GameObject> celestialBodies;
    [SerializeField]
    List<Transform> startPositions;

    void Start()
    {

    }
    
}
