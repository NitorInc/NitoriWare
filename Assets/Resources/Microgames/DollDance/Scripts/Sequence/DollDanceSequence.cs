using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class DollDanceSequence : MonoBehaviour
{

    public enum Move { Wrong, Up, Down, Left, Right }

    [SerializeField]
    int moveCount;

    [SerializeField]
    int repeats;

    List<Move> validMoves;
    Stack<Move> sequence;

    void Awake()
    {
        this.sequence = new Stack<Move>();

        this.validMoves = Enum.GetValues(typeof(Move)).Cast<Move>().ToList();
        this.validMoves.Remove(Move.Wrong);

        ResetSlots(this.moveCount);
    }

    void ResetSlots(int moveCount)
    {
        this.moveCount = moveCount;
        sequence.Clear();

        System.Random random = new System.Random();
        Move previousMove = Move.Wrong;
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
            return Move.Wrong;
    }

    public bool IsComplete()
    {
        return sequence.Count == 0;
    }

}
