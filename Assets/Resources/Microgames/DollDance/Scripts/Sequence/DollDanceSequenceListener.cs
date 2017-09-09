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
                if (Input.GetKeyDown(entry.Key))
                {
                    DollDanceSequence.Move move = this.performance.GetSequence().Process(entry.Value);
                    if (move == DollDanceSequence.Move.Wrong)
                    {
                        Destroy(this);
                        this.performance.Fail();
                    }
                    else
                    {
                        this.performance.Perform(move);
                    }
                }
            }
        }
        else
        {
            Destroy(this);
            this.performance.Succeed();
        }
    }
    
}
