using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskPuzzleGrabbableFragmentsManager : MonoBehaviour {

    [Header("Mask Library")]
    [SerializeField]
    private GameObject maskLibrary;

    // Initialization - choose and prepare the mask that will be assembled by the player
    void Start ()
    {
        // Choose a random mask from the library
        GameObject chosenMask = maskLibrary.transform.GetChild(Random.Range(0, maskLibrary.transform.childCount)).gameObject;
        print("Chosen " + chosenMask.name + ". It has " + chosenMask.transform.childCount + " fragments.");

        // Make all the fragments of the chosen mask children of this object
        while (chosenMask.transform.childCount > 0)
        {
            print("Taking " + chosenMask.transform.GetChild(0).gameObject.name + " from the library");
            chosenMask.transform.GetChild(0).parent = transform;
        }

        // Disable the mask library - we won't need the other masks
        maskLibrary.SetActive(false);
	}
}
