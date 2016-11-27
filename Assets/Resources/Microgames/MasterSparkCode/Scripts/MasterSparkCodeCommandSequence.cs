using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MasterSparkCodeCommandSequence : MonoBehaviour
{

    public Queue<MasterSparkCodeCommand> InputSequence = new Queue<MasterSparkCodeCommand>();
    public GameObject CommandPrefab;
    public int GameLevel;

    void Awake()
    {
        Assert.IsTrue(CommandPrefab.GetComponent<MasterSparkCodeCommand>() != null);
        for (int i = 0; i < GameLevel * 2; i++)
        {
            Vector3 newPosition = new Vector3(-4 + (1 * i), 4, 0);
            GameObject newObject = Instantiate(CommandPrefab, newPosition, Quaternion.identity) as GameObject;
            MasterSparkCodeCommand newCommand = newObject.GetComponent<MasterSparkCodeCommand>();
            InputSequence.Enqueue(newCommand);
        }
    }

    public bool IsCorrectInput(MasterSparkCodeCommandType playerInput)
    {
        return playerInput == InputSequence.Peek().GetComponent<MasterSparkCodeCommand>().Input;
    }

    public bool IsEmpty()
    {
        return InputSequence.Count == 0;
    }

    public void DequeueCommand()
    {
        InputSequence.Dequeue().DestroySelf();
        foreach (var c in InputSequence.ToList())
            c.MoveSelf(-1);
    }
     
}