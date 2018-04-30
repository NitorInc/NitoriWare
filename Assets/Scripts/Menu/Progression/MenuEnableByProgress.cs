using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEnableByProgress : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private Operation operation;
    [SerializeField]
    private PrefsHelper.GameProgress compareTo;
#pragma warning restore 0649

    public enum Operation
    {
        GreaterThan,
        LessThan,
        EqualTo
    }
    

	void Awake()
	{
        gameObject.SetActive(requirementMet());
	}
	
	bool requirementMet()
    {
        PrefsHelper.GameProgress progress = PrefsHelper.getProgress();
        switch (operation)
        {
            case (Operation.GreaterThan):
                return progress > compareTo;
            case (Operation.LessThan):
                return progress < compareTo;
            case (Operation.EqualTo):
                return progress == compareTo;
            default:
                return false;
        }

    }
}
