using UnityEngine;

public class DollDanceController: MonoBehaviour
{

    [Header("A random sequence of moves")]
    [SerializeField]
    DollDanceSequence sequence;
    
    [Header("Scene objects")]

    [SerializeField]
    GameObject victoryEffects;

    [SerializeField]
    GameObject defeatEffects;

    DollDancePerformance performance;
    

    void Start()
    {
        this.performance = FindObjectOfType<DollDancePerformance>();
        this.performance.StartSequence(this.sequence);
    }
    
    public void Victory()
    {
        this.victoryEffects.SetActive(true);
    }

    public void Defeat()
    {
        this.defeatEffects.SetActive(true);
    }

}
