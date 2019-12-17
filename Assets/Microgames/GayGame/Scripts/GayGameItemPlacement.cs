using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GayGameItemPlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject handObject;
    [SerializeField]
    private Vector2 handSpawnRange;
    [SerializeField]
    private GameObject letterObject;
    [SerializeField]
    private Vector2 letterSpawnRange;

    void Awake ()
    {
        bool handOnLeft = MathHelper.randomBool();
        handObject.transform.position = new Vector3(
            MathHelper.randomRangeFromVector(handSpawnRange) * (handOnLeft ? -1f : 1f),
            handObject.transform.position.y,
            handObject.transform.position.z);
        letterObject.transform.position = new Vector3(
            MathHelper.randomRangeFromVector(letterSpawnRange) * (handOnLeft ? 1f : -1f),
            letterObject.transform.position.y,
            letterObject.transform.position.z);

        //if (!handOnLeft)
        //{
        //    handObject.transform.localScale = new Vector3(
        //        -handObject.transform.localScale.x,
        //        handObject.transform.localScale.y,
        //        handObject.transform.localScale.z);
        //    letterObject.transform.localScale = new Vector3(
        //        -letterObject.transform.localScale.x,
        //        letterObject.transform.localScale.y,
        //        letterObject.transform.localScale.z);
        //}

    }
}
