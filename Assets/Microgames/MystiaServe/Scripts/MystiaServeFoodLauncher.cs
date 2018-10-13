using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MystiaServeFoodLauncher : MonoBehaviour
{
    [SerializeField]
    private Vector2 xSpeedRange;
    [SerializeField]
    private Vector2 ySpeedRange;
    [SerializeField]
    private Vector2 rotSpeedRange;

    public void launch()
    {
        var direction = Mathf.Sign(transform.root.localScale.x);
        var rigidBois = GetComponentsInChildren<Rigidbody2D>();
        foreach (var rigidBoi in rigidBois)
        {
            rigidBoi.bodyType = RigidbodyType2D.Dynamic;
            rigidBoi.transform.parent = null;
            rigidBoi.velocity = new Vector2(
                MathHelper.randomRangeFromVector(xSpeedRange) * direction,
                MathHelper.randomRangeFromVector(ySpeedRange));
            rigidBoi.angularVelocity = MathHelper.randomRangeFromVector(rotSpeedRange * direction);
        }
    }
}
