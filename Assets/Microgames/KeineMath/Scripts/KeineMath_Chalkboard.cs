using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeineMath_Chalkboard : MonoBehaviour {

    private GameObject chalkboardtext;
    private int answer;
    private string problem;
    private bool answered;

    // Use this for initialization
    void Start()
    {
        setProblem();
        chalkboardtext = this.transform.Find("ChalkboardText").gameObject;
        chalkboardtext.GetComponent<TextMesh>().text = problem;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void setProblem()
    {
        //TODO: Generate actual problem with actual answer, and display the problem rather than the answer
        int rand1 = Random.Range(1, 10);
        int rand2 = Random.Range(1, 10);
        problem = rand1.ToString() + " + " + rand2.ToString() + " = ?";
        answer = rand1 + rand2;
    }

    public void processAnswer(string answerString)
    {
        if (!answered)
        {
            answered = true;
            int playerAnswer = int.Parse(answerString);
            if (playerAnswer == answer)
            {
                MicrogameController.instance.setVictory(victory: true, final: true);
            }
            else
            {
                MicrogameController.instance.setVictory(victory: false, final: true);
            }
        }
    }
}
