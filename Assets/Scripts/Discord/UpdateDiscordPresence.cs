using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateDiscordPresence : MonoBehaviour
{
    public bool updateDetails = true;
    public string detailsKey;
    public string defaultDetails;
    public bool updateState = true;
    public string stateKey;
    public string defaultState;
    public DiscordController.TimeStampType startTimestamp;

    void Start ()
    {
        updateStatus();
	}

    public void updateStatus()
    {
        string details = string.IsNullOrEmpty(detailsKey) ? defaultDetails : TextHelper.getLocalizedText(detailsKey, defaultDetails);
        string state = string.IsNullOrEmpty(stateKey) ? defaultState : TextHelper.getLocalizedText(stateKey, defaultState);
        if (updateDetails && updateState)
        {
            GameController.instance.discord.updatePresence(
                state,
                details,
                startTimestamp);
        }
        else if (updateDetails)
        {
            GameController.instance.discord.updatePresence(
                details: details,
                startTimeStamp: startTimestamp);
        }
        else if (updateState)
        {
            GameController.instance.discord.updatePresence(
                state: state,
                startTimeStamp: startTimestamp);
        }

    }
}
