using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateDiscordPresence : MonoBehaviour
{
    public bool updateDetails = true;
    public string detailsKey;
    public string defaultDetails;
    public bool updateState;
    public string stateKey;
    public string defaultState;
    public bool resetTimeStamp;

    void Start ()
    {
        updateStatus();
	}

    public void updateStatus()
    {
        if (updateDetails && updateState)
        {
            GameController.instance.discord.updatePresence(
                TextHelper.getLocalizedText(detailsKey, defaultDetails),
                TextHelper.getLocalizedText(stateKey, defaultState),
                resetTimeStamp);
        }
        else if (updateDetails)
        {
            GameController.instance.discord.updatePresenceDetails(
                TextHelper.getLocalizedText(detailsKey, defaultDetails),
                resetTimeStamp);
        }
        else if (updateState)
        {
            GameController.instance.discord.updatePresenceDetails(
                TextHelper.getLocalizedText(stateKey, defaultState),
                resetTimeStamp);
        }

    }
}
