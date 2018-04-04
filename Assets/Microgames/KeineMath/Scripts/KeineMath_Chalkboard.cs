using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeineMath_Chalkboard : MonoBehaviour {

    [Header("Number of terms")]
    [SerializeField]
    private int termCount = 2;

    [Header("Minimum size of terms")]
    [SerializeField]
    private int minTerm = 1;

    [Header("Maximum size of terms")]
    [SerializeField]
    private int maxTerm = 5;

    [Header("Operation to use")]
    [SerializeField]
    private string operation = "+";

    private List<int> termList = new List<int>();

    private GameObject term1;
    private GameObject term2;
    private GameObject term3;
    private List<GameObject> terms = new List<GameObject>();
    private GameObject operationSymbol;

    // Use this for initialization
    void Start () {
        term1 = GameObject.Find("Term1");
        terms.Add(term1);
        term2 = GameObject.Find("Term2");
        terms.Add(term2);
        term3 = GameObject.Find("Term3");
        terms.Add(term3);
        operationSymbol = GameObject.Find("Operation");
        generateProblem();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void generateProblem()
    {
        for(int i = 0; i < termCount; i++)
        {
            termList.Add(Random.Range(minTerm, (maxTerm + 1)));
        }
        if (operation.Equals("+"))
        {
            for(int i = 0; i < terms.Count; i++)
            {
                if (i < termCount)
                {
                    terms[i].GetComponent<KeineMath_Term>().setValue(termList[i]);
                } else
                {
                    terms[i].SetActive(false);
                }
            }
        } else if (operation.Equals("-"))
        {
            print("Subtraction not implemented!");
        } else
        {
            print("Invalid operation!");
        }
        float symbolx = operationSymbol.transform.position.x - (1f * Mathf.Max(termList.ToArray())) + 1;
        float symboly = operationSymbol.transform.position.y;
        operationSymbol.transform.position = new Vector3(symbolx, symboly, 0);
    }
}
