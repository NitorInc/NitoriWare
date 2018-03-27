using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskPuzzleMaskFragment : MonoBehaviour {

    public MaskPuzzleGrabbableFragmentsManager fragmentsManager;

    public MaskPuzzleFragmentGroup fragmentGroup;

    void Start()
    {
        fragmentGroup = new MaskPuzzleFragmentGroup(this);
    }

    // Called every frame
    // Move and rotate the mask when victory achieved
    void Update()
    {
        if (MicrogameController.instance.getVictory())
        {
            VictoryAnimation();
        }
    }

    public void VictoryAnimation()
    {
        if (Time.time > fragmentsManager.victoryStartTime + fragmentsManager.victoryMoveTime)
        {
            // Animation time elapsed, set animated variables to the final values
            transform.position = fragmentsManager.victoryGoal;
            transform.eulerAngles = fragmentsManager.victoryRotation;
        }
        else
        {
            float timeFactor = (Time.time - fragmentsManager.victoryStartTime) /
                               fragmentsManager.victoryMoveTime;
            transform.position = Vector3.Lerp(
                fragmentsManager.victoryStartPosition,
                fragmentsManager.victoryGoal,
                1 - Mathf.Pow(1 - timeFactor, 2)
            );
            transform.eulerAngles = Vector3.Slerp(
                fragmentsManager.victoryStartRotation,
                fragmentsManager.victoryRotation,
                1 - Mathf.Pow(1 - timeFactor, 2)
            );
        }
    }
}
