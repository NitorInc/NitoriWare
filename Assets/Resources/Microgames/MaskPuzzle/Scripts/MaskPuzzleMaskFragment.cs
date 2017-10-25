using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskPuzzleMaskFragment : MonoBehaviour {

    private int snapCounter = 0;

    public void SnapToOtherFragments()
    {
        print(snapCounter++);
    }
}
