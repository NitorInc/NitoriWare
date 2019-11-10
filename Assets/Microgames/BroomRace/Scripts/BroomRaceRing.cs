using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomRaceRing : MonoBehaviour
{

    [SerializeField]
    private Animator rigAnimator;
    [SerializeField]
    private float ringActivateSpinSpeed = 2f;

    [SerializeField]
    private Vector2 spawnRange;
    [SerializeField]
    private FlipYMode flipYMode;
    private enum FlipYMode
    {
        Raw,
        Alternating,
        Random
    }

    [SerializeField]
    private Vector2 velXRange;
    [SerializeField]
    private Vector2 velYRange;
    [SerializeField]
    private VerticalSpeedMode verticalSpeedMode;
    private enum VerticalSpeedMode
    {
        Raw,
        OppositeOfY
    }
    
    private static FlipAlternateInstruction flipInstruction;
    private enum FlipAlternateInstruction
    {
        Awaiting,
        FlipSecond,
        FlipFirst
    }

    private Vector2 velocity;


    private void Awake()
    {
        flipInstruction = FlipAlternateInstruction.Awaiting;
    }

    private void Start() 
    {
        float spawnY = MathHelper.randomRangeFromVector(spawnRange);
        var flip = false;

        if (flipYMode == FlipYMode.Alternating)
        {
            if (flipInstruction == FlipAlternateInstruction.FlipSecond)
                flip = true;
            else if (flipInstruction == FlipAlternateInstruction.Awaiting)
            {
                flipInstruction = MathHelper.randomBool() ? FlipAlternateInstruction.FlipFirst : FlipAlternateInstruction.FlipSecond;
                if (flipInstruction == FlipAlternateInstruction.FlipFirst)
                    flip = true;
            }
        }
        else if (flipYMode == FlipYMode.Random)
            flip = MathHelper.randomBool();

        if (flip)
            spawnY *= -1f;
        transform.position = new Vector3(transform.position.x, spawnY, transform.position.x);

        velocity = new Vector2(MathHelper.randomRangeFromVector(velXRange), MathHelper.randomRangeFromVector(velYRange));
        if (verticalSpeedMode == VerticalSpeedMode.OppositeOfY)
            velocity.y = Mathf.Abs(velocity.y) * -Mathf.Sign(transform.position.y);
    }

    public void activate()
    {
        rigAnimator.SetTrigger("Activate");
        rigAnimator.SetFloat("SpinSpeed", ringActivateSpinSpeed);
        velocity = Vector2.zero;
    }

    private void Update()
    {
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

}
