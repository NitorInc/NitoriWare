using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskPuzzleMaskFragment : MonoBehaviour {

    public MaskPuzzleGrabbableFragmentsManager fragmentsManager;

    public MaskPuzzleFragmentGroup fragmentGroup;

    void Start()
    {
        fragmentGroup = new MaskPuzzleFragmentGroup(this);
        fragmentGroup.assignedCamera = Instantiate(Camera.main);
        fragmentGroup.assignedCamera.GetComponent<AudioListener>().enabled = false;
        fragmentGroup.assignedCamera.clearFlags = CameraClearFlags.Depth;
        fragmentGroup.assignedCamera.cullingMask = 1 << gameObject.layer;
        fragmentGroup.assignedCamera.depth = 0;
    }

    // Called every frame
    // Move and rotate the mask when victory achieved
    void Update()
    {
        if (MicrogameController.instance.getVictory())
        {
            if (Time.time > fragmentsManager.victoryStartTime + fragmentsManager.victoryMoveTime)
            {
                // Animation time elapsed, set animated variables to the final values
                transform.position = fragmentsManager.victoryGoal;
                transform.eulerAngles = fragmentsManager.victoryRotation;
                fragmentsManager.backgroundImage.color = Color.white;
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
                fragmentsManager.backgroundImage.color = Color.Lerp(
                    fragmentsManager.victoryStartBgColor,
                    Color.white,
                    timeFactor * 4
                );
            }
        }
    }
}
