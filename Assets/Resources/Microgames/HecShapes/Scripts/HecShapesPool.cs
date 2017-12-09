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

    MouseGrabbableGroup grabGroup;

    List<HecShapesCelestialBody> shapes;
    int correctIndex;

    void Awake()
    {
        this.grabGroup = GetComponent<MouseGrabbableGroup>();

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
        // Make the planets
        for (int i = 0; i < this.shapes.Count; i++)
        {
            // This is the planet's generic object
            HecShapesSlottable slottable = Instantiate(
                    this.slottableTemplate,
                    this.startPositions[i]);

            if (i == this.correctIndex)
                slottable.correct = true;

            // Instantiate the planet's sprite and collider holding object as a child
            var shape = Instantiate(this.shapes[i], slottable.transform);

            // Make the planet grabbable
            var grabbable = slottable.gameObject.AddComponent<MouseGrabbable>();
            grabbable.disableOnLoss = true;
            grabbable.disableOnVictory = true;

            grabbable._collider2D = shape.GetComponent<Collider2D>();

            // Add to grabbable group
            this.grabGroup.addGrabbable(grabbable, false);
        }
    }

    public HecShapesSlottable.Shape GetCorrectShape()
    {
        return this.shapes[this.correctIndex].shape;
    }
    
}
