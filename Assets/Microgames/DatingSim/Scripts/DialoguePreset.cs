using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is a dummy class that presents a data set similar to localized version,
// and the game starts here.
public class DialoguePreset : MonoBehaviour {

    Dictionary<string, string> dummyTextCollection = new Dictionary<string, string>() {

        {"Microgame.DatingSim.SuikaFullName", "Suika Inuki"},
        {"Microgame.DatingSim.SuikaStart", "Heeeeey! You’re free aren’cha? Let’s grab some-thing to eat."},
        {"Microgame.DatingSim.SuikaWinOption0", "Race you to the food stall!"},
        {"Microgame.DatingSim.SuikaWinResponse0", "Loser has to pay!"},
        {"Microgame.DatingSim.SuikaWinOption1", "Yeah, let’s get some grub!"},
        {"Microgame.DatingSim.SuikaWinResponse1", "Yay!"},
        {"Microgame.DatingSim.SuikaLoseOption0", "Didn’t you just eat?!"},
        {"Microgame.DatingSim.SuikaLoseResponse0", "Boo. It’s no fun eating alone."},
        {"Microgame.DatingSim.SuikaLoseOption1", "Get lost, fatso!"},
        {"Microgame.DatingSim.SuikaLoseResponse1", "Well you’re not the skinniest yourself, jerk!"},

        {"Microgame.DatingSim.YoumuFullName", "Youmu Konpaku"},
        {"Microgame.DatingSim.YoumuStart", "I grew these flowers for you."},
        {"Microgame.DatingSim.YoumuWinOption0", "They’re beautiful!"},
        {"Microgame.DatingSim.YoumuWinResponse0", "Thank you~"},
        {"Microgame.DatingSim.YoumuWinOption1", "[Player choice]"},
        {"Microgame.DatingSim.YoumuWinResponse1", "[Character Response]"},
        {"Microgame.DatingSim.YoumuLoseOption0", "Aaah, a ghost!"},
        {"Microgame.DatingSim.YoumuLoseResponse0", "You have got to be kidding me."},
        {"Microgame.DatingSim.YoumuLoseOption1", "I hate flowers!"},
        {"Microgame.DatingSim.YoumuLoseResponse1", "You don’t deserve them anyway."},

        {"Microgame.DatingSim.YuukaFullName", "Yuuka Kazami"},
        {"Microgame.DatingSim.YuukaStart", "You weren’t seeing other girls were you?"},
        {"Microgame.DatingSim.YuukaWinOption0", "Of course not!"},
        {"Microgame.DatingSim.YuukaWinResponse0", "Good, you better not be."},
        {"Microgame.DatingSim.YuukaWinOption1", "You’re the only one for me!"},
        {"Microgame.DatingSim.YuukaWinResponse1", "Thought so."},
        {"Microgame.DatingSim.YuukaLoseOption0", "Well..."},
        {"Microgame.DatingSim.YuukaLoseResponse0", "What do you mean, “Well”?"},
        {"Microgame.DatingSim.YuukaLoseOption1", "I’m in love with Reimu!"},
        {"Microgame.DatingSim.YuukaLoseResponse1", "Think again."},

        {"Microgame.DatingSim.PatchyFullName", "Patchouli Knowledge"},
        {"Microgame.DatingSim.PatchyStart", "I hope you like my tea"},
        {"Microgame.DatingSim.PatchyWinOption0", "Wow it's good"},
        {"Microgame.DatingSim.PatchyWinResponse0", "Thank you!"},
        {"Microgame.DatingSim.PatchyWinOption1", "More pls"},
        {"Microgame.DatingSim.PatchyWinResponse1", "Right away!"},
        {"Microgame.DatingSim.PatchyLoseOption0", "It's bad"},
        {"Microgame.DatingSim.PatchyLoseResponse0", "Oh... okay"},
        {"Microgame.DatingSim.PatchyLoseOption1", "*Trip and spill hot tea everywhere*"},
        {"Microgame.DatingSim.PatchyLoseResponse1", "EEYYAAAAAAAAAAaahh!!"},
    };
    string dummyPrefix = "Microgame.DatingSim.";
    string[] characterKeywords = {
        "Suika",
        "Youmu",
        "Yuuka",
        //"Patchy"
    };

    public delegate void OnSelection(int index);
    public static event OnSelection OnCharacterSelection;

    public int overrideCharacterSelection = -1;

    private void Awake()
    {
        OnCharacterSelection = null;
    }

    // Use this for initialization
    void Start () {
        int index = Random.Range(0, characterKeywords.Length);
        if (overrideCharacterSelection != -1)
            index = overrideCharacterSelection;
        string currChar = characterKeywords[index];
        // Game starts on character selection
        OnCharacterSelection(index);
    }

    public string GetOption(int charIndex, bool isWin, bool isOption, int targetIndex) {
        string key = dummyPrefix + characterKeywords[charIndex] + (isWin ? "Win" : "Lose") + (isOption ? "Option" : "Response") + targetIndex.ToString();
        return dummyTextCollection[key]; 
    }

    public string GetFullName(int index) {
        return dummyTextCollection[dummyPrefix + characterKeywords[index] + "FullName"];
    }

    public string GetStartingDialogue(int index) {
        return dummyTextCollection[dummyPrefix + characterKeywords[index] + "Start"];
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
}
