using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MasterSparkCodeCommandSequence : MonoBehaviour
{

    public Queue<MasterSparkCodeCommand> InputSequence = new Queue<MasterSparkCodeCommand>();
    public GameObject CommandPrefab;
    public AudioSource Audio;
    public int GameLevel;

    void Start()
    {
        Assert.IsTrue(CommandPrefab.GetComponent<MasterSparkCodeCommand>() != null);
        for (int i = 0; i < 2+GameLevel; i++)
        {
            Vector3 newPosition = new Vector3(-4 + (1.25f * i), 3.75f, 0);
            GameObject newObject = Instantiate(CommandPrefab, newPosition, Quaternion.identity) as GameObject;
            MasterSparkCodeCommand newCommand = newObject.GetComponent<MasterSparkCodeCommand>();
            newCommand.SetInput((MasterSparkCodeCommandType)Random.Range(0,(i==0)?4:5));
            if (i == 0)
                newCommand.ScaleSelf();
            InputSequence.Enqueue(newCommand);
        }

        // For speedups
        Audio.pitch = 1f;
    }

    public bool IsCorrectInput(MasterSparkCodeCommandType playerInput)
    {
        return playerInput == InputSequence.Peek().Input;
    }

    public bool IsEmpty()
    {
        return InputSequence.Count == 0;
    }

    public void DequeueCommand()
    {
        InputSequence.Dequeue().SetPressed();
        Audio.Play();
        if (!IsEmpty())
            InputSequence.Peek().ScaleSelf();
    }
     
}