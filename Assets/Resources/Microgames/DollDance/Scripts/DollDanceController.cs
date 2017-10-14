using UnityEngine;

public class DollDanceController: MonoBehaviour
{
    
    [Header("Scene objects")]

    [SerializeField]
    GameObject victoryEffects;

    [SerializeField]
    GameObject defeatEffects;

    DollDancePerformance performance;
    

    void Start()
    {
        this.performance = FindObjectOfType<DollDancePerformance>();

        // Find sequence object and begin the performance
        this.performance.StartSequence(this.GetComponent<DollDanceSequence>());
    }
    
    public void Victory()
    {
        this.victoryEffects.SetActive(true);
        //performance.getAliceShadeComponent().setShaded(false);

        MicrogameController.instance.setVictory(true, true);
    }

    public void Defeat()
    {
        this.defeatEffects.SetActive(true);
        //performance.getAliceShadeComponent().setShaded(false);

        MicrogameController.instance.setVictory(false, true);
    }

}
