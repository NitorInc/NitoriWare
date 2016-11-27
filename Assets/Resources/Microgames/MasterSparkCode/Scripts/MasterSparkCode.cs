using UnityEngine;
using System.Collections;

public class MasterSparkCode : MonoBehaviour {

    // Game Controller's properties
    public MasterSparkCodeCommandSequence InputSequence;
    public GameObject MarisaObject;
    public Sprite MarisaWin; // placeholders
    public Sprite MarisaLose; // placeholders

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!(MicrogameController.instance.getVictoryDetermined() && !MicrogameController.instance.getVictory()))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                ProcessKey(MasterSparkCodeCommandType.Up);
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                ProcessKey(MasterSparkCodeCommandType.Down);
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                ProcessKey(MasterSparkCodeCommandType.Left);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                ProcessKey(MasterSparkCodeCommandType.Right);
            else if (Input.GetKeyDown(KeyCode.Space))
                ProcessKey(MasterSparkCodeCommandType.Action);
        }

    }
    
    void ProcessKey(MasterSparkCodeCommandType c)
    {
        if(InputSequence.IsCorrectInput(c))
        {
            InputSequence.DequeueCommand();
            if (InputSequence.IsEmpty())
            {
                MicrogameController.instance.setVictory(true, true);
                MarisaObject.GetComponent<SpriteRenderer>().sprite = MarisaWin;
            }

        }
        else
        {
            MicrogameController.instance.setVictory(false, true);
            MarisaObject.GetComponent<SpriteRenderer>().sprite = MarisaLose;
        }

    }
}
