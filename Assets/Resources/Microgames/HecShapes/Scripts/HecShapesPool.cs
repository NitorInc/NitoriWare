using System.Collections.Generic;
using UnityEngine;

public class HecShapesPool : MonoBehaviour
{
    
    [SerializeField]
    HecShapesSlottable slottableTemplate;

    [SerializeField]
    List<HecShapesCelestialBody> availableShapes;
    [SerializeField]
    List<Transform> startPositions;

    List<HecShapesCelestialBody> shapes;
    int correctIndex;

    void Awake()
    {
        this.shapes = new List<HecShapesCelestialBody>();
        while (availableShapes.Count > 0)
        {
            int index = Random.Range(0, availableShapes.Count);
            this.shapes.Add(availableShapes[index]);
            availableShapes.RemoveAt(index);

            if (this.shapes.Count == this.startPositions.Count)
                break;
        }

        this.correctIndex = Random.Range(0, this.shapes.Count);
    }

    void Start()
    {
        for (int i = 0; i < this.shapes.Count; i++)
        {
            HecShapesSlottable slottable = Instantiate(
                    this.slottableTemplate,
                    this.startPositions[i]);

            if (i == this.correctIndex)
            {
                slottable.correct = true;
            }

            slottable.SetShape(this.shapes[i]);
        }
    }

    public HecShapesSlottable.Shape GetCorrectShape()
    {
        return this.shapes[this.correctIndex].shape;
    }
    
}
