using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class DollDanceSequence : MonoBehaviour
{

    public enum Move { Idle, Up, Down, Left, Right }

    [Header("Number of sequential dance moves")]
    [SerializeField]
    int moveCount;
    
    List<Move> validMoves;
    Stack<Move> sequence;

    void Awake()
    {
        this.sequence = new Stack<Move>();

        this.validMoves = Enum.GetValues(typeof(Move)).Cast<Move>().ToList();
        this.validMoves.Remove(Move.Idle);

        ResetSlots(this.moveCount);
    }

    void ResetSlots(int moveCount)
    {
        // Clear slots
        this.moveCount = moveCount;
        sequence.Clear();

        // Fill all of the sequence slots randomly
        System.Random random = new System.Random();
        Move previousMove = Move.Idle;
        for (int i = 0; i < this.moveCount; i++)
        {
            List<Move> available = new List<Move>(this.validMoves);
            available.Remove(previousMove);

            Move newMove = available[random.Next(available.Count)];
            sequence.Push(newMove);
            previousMove = newMove;
        }
    }
    
    public List<Move> CopySequence()
    {
        return new List<Move>(this.sequence);
    }

    public Move Process(Move move)
    {
        if (move == sequence.Peek())
            return sequence.Pop();
        else
            return Move.Idle;
    }
    
    public bool IsComplete()
    {
        return sequence.Count == 0;
    }

}
