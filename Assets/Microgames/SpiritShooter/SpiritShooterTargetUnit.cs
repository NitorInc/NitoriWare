using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritShooterTargetUnit : MonoBehaviour {

    public enum UnitType {
        Renko = 0,
        Akyuu,
        Ghost
    }

    bool bIsClickable = false;
    public UnitType type;
    public void SetType(UnitType type) {
        this.type = type;
    }
    public GameObject[] unitSprite;
    public bool IsEnemy {
        get {
            return (int)type > 1;
        }
    }

    public Animator parentAnimator;
    public Animator flipAnimator;
    public string flipClipName = "Flip";
    public string collapseClipName = "Collapse";

    bool bHasClicked = false;

    SpiritShooterObjectSpawner controller;
    public void Register(SpiritShooterObjectSpawner controller, SpiritShooterSpawnPoint point) {
        GetComponent<SpiritShooterTargetMover>().Initialize(point.startPoint, point.endPoint);
        this.controller = controller;
    }

    public void Activate(float time) {
        Invoke("StartMoving", time);
    }

    void StartMoving() {
        GetComponent<SpiritShooterTargetMover>().StartMoving();
    }

    void SetSprite() {
        unitSprite[(int)type].SetActive(true);
    }

    void Update() {
        if (!bIsClickable) {
            var state = flipAnimator.GetCurrentAnimatorStateInfo(0);
            if (state.IsName(flipClipName) && state.normalizedTime >= 1.0f) {
                bIsClickable = true;
            }
        }
    }

    public void Flip() {
        SetSprite();
        flipAnimator.Play(flipClipName);
    }

    public void Collapse() {
        parentAnimator.Play(collapseClipName);
    }

    public void Disable() {
        bHasClicked = true;
    }

    void OnMouseDown() {
        if (!bHasClicked) {
            if (bIsClickable) {
                Collapse();
                Disable();
                controller.NotifyOnClick(IsEnemy);
            }
        }
    }
}
