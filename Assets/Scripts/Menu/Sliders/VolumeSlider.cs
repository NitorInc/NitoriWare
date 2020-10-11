using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class VolumeSlider : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private PrefsHelper.VolumeType type;
    [SerializeField]
    private bool applyOnMouseUp;
    [SerializeField]
    private AudioSource onChangeSource;
    [SerializeField]
    private AudioClip onChangeClip;
#pragma warning restore 0649

    private bool queueChange;

	void Start()
	{
        slider.value = PrefsHelper.GetVolumeSetting(type);
        queueChange = false;
	}

    void Update()
    {
        slider.interactable = !GameMenu.shifting;
        if (queueChange && Input.GetMouseButtonUp(0))
            updateValue();
    }

    public void setValue()
    {
        if (applyOnMouseUp)
            queueChange = true;
        else
            updateValue();
    }

    void updateValue()
    {
        PrefsHelper.setVolume(type, slider.value);
        AudioManager.instance.SetVolume(type, slider.value);
        if (onChangeSource != null)
        {
            onChangeSource.Stop();
            if (queueChange)
                onChangeSource.PlayOneShot(onChangeClip);
        }
        queueChange = false;
    }
}
