using System.Collections.Generic;
using UnityEngine;

public class DollDanceSequenceListener : MonoBehaviour
{

    Dictionary<KeyCode, DollDanceSequence.Move> inputMap;
    
    DollDancePerformance performance;
    
    void Start()
    {
        this.performance = GetComponent<DollDancePerformance>();

        this.inputMap = new Dictionary<KeyCode, DollDanceSequence.Move>()
        {
            { KeyCode.UpArrow, DollDanceSequence.Move.Up },
            { KeyCode.DownArrow, DollDanceSequence.Move.Down },
            { KeyCode.LeftArrow, DollDanceSequence.Move.Left },
            { KeyCode.RightArrow, DollDanceSequence.Move.Right },
        };
    }

    void Update()
    {
        if (!this.performance.GetSequence().IsComplete())
        {
            foreach (KeyValuePair<KeyCode, DollDanceSequence.Move> entry in this.inputMap)
            {
                KeyCode input = entry.Key;
                DollDanceSequence.Move selectedMove = entry.Value;

                if (Input.GetKeyDown(input))
                {
                    DollDanceSequence.Move move = this.performance.GetSequence().Process(selectedMove);
                    if (move == DollDanceSequence.Move.Idle)
                    {
                        this.performance.Perform(selectedMove);
                        this.performance.Fail();
                    }
                    else
                    {
                        this.performance.Perform(move);
                    }
                }

                if (Input.GetKeyUp(input))
                {
                    this.performance.Release(selectedMove);
                }
            }
        }
        else
        {
            this.performance.Succeed();
        }
    }
    
}
