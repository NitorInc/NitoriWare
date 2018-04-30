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
        performance = FindObjectOfType<DollDancePerformance>();

        // Find sequence object and begin the performance
        performance.StartSequence(GetComponent<DollDanceSequence>());
    }
    
    public void Victory()
    {
        MicrogameController.instance.setVictory(true, true);

        if (victoryEffects != null)
            victoryEffects.SetActive(true);
    }

    public void Defeat()
    {
        MicrogameController.instance.setVictory(false, true);

        if (defeatEffects != null)
            defeatEffects.SetActive(true);
    }

}
