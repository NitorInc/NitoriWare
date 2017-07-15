using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class VolumeSlider : MonoBehaviour
{

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private PrefsHelper.VolumeType type;
#pragma warning restore 0649



	void Start()
	{
        slider.value = PrefsHelper.getVolumeRaw(type);
	}

    public void setValue()
    {
        PrefsHelper.setVolume(type, slider.value);
    }
}
