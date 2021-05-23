using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritShooterObjectSpawner : MonoBehaviour {

    SpiritShooterSpawnPoint[] spawnLocations;
    public GameObject unitProto;
    List<SpiritShooterTargetUnit> units;
    public float maxActivationTime = 3.0f;
    public float minActivationTime = 0.5f;

    public int totalUnits = 5;
    public int AllyCount {
        get {
            return totalUnits - enemyCount;
        }
    }
    public int enemyCount = 3;

    int shootdownCount = 0;
    
	// Use this for initialization
	void Start () {
        spawnLocations = FindObjectsOfType<SpiritShooterSpawnPoint>();
        units = new List<SpiritShooterTargetUnit>();
        Initialize();
        ActivateUnits();
	}

    void Initialize() {
        spawnLocations.Shuffle();
        for (int i = 0; i < totalUnits; i++) {
            var unit = GameObject.Instantiate(unitProto).GetComponentInChildren<SpiritShooterTargetUnit>();
            unit.Register(this, spawnLocations[i]);
            units.Add(unit);
        }
        units[0].SetType(SpiritShooterTargetUnit.UnitType.Renko);
        units[1].SetType(SpiritShooterTargetUnit.UnitType.Akyuu);
        for (int i = AllyCount; i < units.Count; i++) {
            units[i].SetType(SpiritShooterTargetUnit.UnitType.Ghost);
        }
        units.Shuffle();
    }

    void ActivateUnits() {
        for (int i = 0; i < units.Count; i++) {
            units[i].Activate(Random.Range(minActivationTime, maxActivationTime));
        }
    }
	
    public void NotifyOnClick(bool isEnemy) {
        if (!isEnemy) {
            for (int i = 0; i < units.Count; i++) {
                units[i].Disable();
            }
            MicrogameController.instance.setVictory(false);
        }
        else {
            shootdownCount++;
            if (shootdownCount >= enemyCount) {
                for (int i = 0; i < units.Count; i++) {
                    units[i].Disable();
                }
                MicrogameController.instance.setVictory(true);
            }
        }
    }
}
