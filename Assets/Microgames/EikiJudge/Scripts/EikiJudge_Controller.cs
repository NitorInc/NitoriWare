using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EikiJudge_Controller : MonoBehaviour
{
    public static EikiJudge_Controller controller;

    [SerializeField]
    private GameObject soulPrefab;
    [Header("Set the number of souls to appear")]
    public int soulsNumber;
    [Header("check this if a soul must be late")]
    public bool lastSoulIsLate = false;

    public List<EikiJudge_SoulController> soulsList = new List<EikiJudge_SoulController>();
    private Vector3 spawnPosition;

    private Direction judgementDirection;

    public bool allSoulsReady = false;
    public bool wasted = false;
    public bool gameWon = false;

    private void Awake()
    {
        controller = this;
    }

    private void Start()
    {
        SpawnSouls();
    }

    private void Update()
    {
        // If all souls have been sent AND game isn't lost
        if (soulsList.Count == 0 && !wasted && !gameWon)
        {
            gameWon = true;
            WinGame();
        }
    }

    // Souls spawner
    public void SpawnSouls()
    {
        for (int i = 0; i < soulsNumber; i++)
        {
            spawnPosition = new Vector3(0f, -6f, i * -1f);
            GameObject tempSoul = Instantiate(soulPrefab, spawnPosition, Quaternion.identity);
            soulsList.Add((EikiJudge_SoulController)tempSoul.GetComponent(typeof(EikiJudge_SoulController)));
            soulsList[i].soulListPosition = i;
        }
        allSoulsReady = true;
    }

    public bool SendJudgement(Direction judgementDirection)
    {
        if (soulsList.Count > 0 && soulsList[0].Ready)
        {
            // get next soul and judge
            soulsList[0].SendTheSoul(judgementDirection, soulsList.Count <= 1);

            // delete from list
            soulsList.RemoveAt(0);

            return true;
        }

        return false;
    }

    // TODO: Maybe add some win/lose animations ?
    public void WinGame()
    {
        MicrogameController.instance.setVictory(victory: true, final: true);
    }
    public void LoseGame()
    {
        wasted = true;
        MicrogameController.instance.setVictory(victory: false, final: true);
    }
    
    public enum Direction { none, left, right }

}
