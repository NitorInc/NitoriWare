using UnityEngine;

public class HecShapesHolder : MonoBehaviour
{

    [SerializeField]
    SpriteRenderer hair;
    [SerializeField]
    SpriteRenderer eyes;

    [SerializeField]
    Color color;

    void Update()
    {
        this.hair.color = this.color;
        this.eyes.color = this.color;
    }

}
