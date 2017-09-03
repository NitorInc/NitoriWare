using System.Collections;
using System.Linq;
﻿using UnityEngine;

public class KaguyaMemoryClickable : MonoBehaviour
{
    // Update() timers
    private int rotationTime;
    private int tickTime;

    // Object
    private int styleId;
    public Collider2D objectCollider;

    void Start()
    {
        rotationTime = -60;
        tickTime = 0;
    }

    public int GetStyleId()
    {
        return styleId;
    }

    public void SetStyleId(int styleId)
    {
        this.styleId = styleId;
    }

    void Update()
    {
        if (tickTime <= 120)
        {
            tickTime++;
        }

        else
        {
            if (Input.GetMouseButtonDown(0) && CameraHelper.isMouseOver(objectCollider) && !KaguyaMemoryTraits.clicked)
            {
                // Debug.Log("Click");
                KaguyaMemoryTraits.clicked = true;
                // Debug.Log("Correct id: " + KaguyaMemoryTraits.item);
                // Debug.Log("Selected id: " + GetStyleId());

                // win: correct item clicked
                if (KaguyaMemoryTraits.item == GetStyleId())
                {
                    MicrogameController.instance.setVictory(true, true);
                    // Debug.Log("Good job!");
                }

                // fail: wrong item clicked
                else
                {
                    MicrogameController.instance.setVictory(false, true);
                    // todo: make kaguya __angery__
                    // Debug.Log("Nope.");
                }

            }

            // while item not clicked, rotate them a bit
            if (!KaguyaMemoryTraits.clicked)
            {
                rotationTime++;

                if (rotationTime <= 0) transform.Rotate(0, 0, 1);
                else if (rotationTime <= 60) transform.Rotate(0, 0, -1);
                else rotationTime = -60;
            }
        }
    }
}
