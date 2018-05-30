using System.Collections.Generic;
using UnityEngine;

public class DollDanceSequenceListener : MonoBehaviour
{

    Dictionary<KeyCode, DollDanceSequence.Move> inputMap;
    
    DollDancePerformance performance;
    
    void Start()
    {
        performance = GetComponent<DollDancePerformance>();

        inputMap = new Dictionary<KeyCode, DollDanceSequence.Move>()
        {
            { KeyCode.UpArrow, DollDanceSequence.Move.Up },
            { KeyCode.DownArrow, DollDanceSequence.Move.Down },
            { KeyCode.LeftArrow, DollDanceSequence.Move.Left },
            { KeyCode.RightArrow, DollDanceSequence.Move.Right },
        };
    }

    void Update()
    {
        if (!performance.GetSequence().IsComplete())
        {
            foreach (KeyValuePair<KeyCode, DollDanceSequence.Move> entry in inputMap)
            {
                KeyCode input = entry.Key;
                DollDanceSequence.Move selectedMove = entry.Value;

                if (Input.GetKeyDown(input))
                {
                    DollDanceSequence.Move move = performance.GetSequence().Process(selectedMove);
                    if (move == DollDanceSequence.Move.Idle)
                    {
                        performance.Perform(selectedMove);
                        performance.Fail();
                    }
                    else
                    {
                        performance.Perform(move);
                    }
                }
            }
        }
        else
        {
            performance.Succeed();
        }
    }
    
}
