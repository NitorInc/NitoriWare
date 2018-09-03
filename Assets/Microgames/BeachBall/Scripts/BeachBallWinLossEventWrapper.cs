using System;
using UnityEngine;

public class BeachBallWinLossEventWrapper : MonoBehaviour
{

    public event EventHandler OnWin;
    public event EventHandler OnLoss;

    public const String DefaultGameObjectName = "WinLossEventWrapper";

    private bool fired;

    void Update()
    {
        if (!fired && MicrogameController.instance.getVictoryDetermined())
        {
            fired = true;
            if (MicrogameController.instance.getVictory())
                OnWin(this, new EventArgs());
            else
                OnLoss(this, new EventArgs());
        }
    }
    void OnDestroy()
    {
        OnWin = null;
        OnLoss = null;
    }
}
