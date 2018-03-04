using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClick : MonoBehaviour
{
    //Set this variable to the card
    byte cardclick;

    void OnMouseDown()
    {
        GameObject thePlayer = GameObject.Find("Wheel5");
        WheelSpin whlspn = thePlayer.GetComponent<WheelSpin>();
        print("Sumithought 1 from cardclick script is: " + whlspn.sumithought1);
        print("Sumithought 2 from cardclick script is: " + whlspn.sumithought2);

        /* CARD LIST
         * 1 = Plus
         * 2 = O
         * 3 = Square
         * 4 = Wave
         * 5 = Star 
         * 6 = Purple Plus
         * 7 = Purple O
         * 8 = Purple Square
         * 9 = Purple Wave
         * 10 = Purple Star */


        if (gameObject.name == ("Card1"))
        {
            cardclick = 1; //Sets to Plus
        }
        else if (gameObject.name == ("Card2"))
        {
            cardclick = 2; //Sets to O
        }
        else if (gameObject.name == ("Card3"))
        {
            cardclick = 3; //Sets to Square
        }
        else if (gameObject.name == ("Card4"))
        {
            cardclick = 4; //Sets to Wave
        }
        else if (gameObject.name == ("Card5"))
        {
            cardclick = 5; //Sets to Star
        }
        else if (gameObject.name == ("Card6"))
        {
            cardclick = 6; //Sets to Purple Plus
        }
        else if (gameObject.name == ("Card7"))
        {
            cardclick = 7; //Sets to Purple O
        }
        else if (gameObject.name == ("Card8"))
        {
            cardclick = 8; //Sets to Purple Square
        }
        else if (gameObject.name == ("Card9"))
        {
            cardclick = 9; //Sets to Purple Wave
        }
        else if (gameObject.name == ("Card10"))
        {
            cardclick = 10; //Sets to Purple Star
        }

        print("The name of the object you just clicked is " + gameObject.name); //debug
        print("Therefore cardclick is set to " + cardclick);                    //debug

        //Win-Lose condition
        if (whlspn.sumithought1 == cardclick)
        {
            whlspn.correctcardschosen++;
            print("Correct cards picked: " + whlspn.correctcardschosen);

        }
        else if (whlspn.sumithought2 == cardclick)
        {
            whlspn.correctcardschosen++;
            print("Correct cards picked: " + whlspn.correctcardschosen);
        }
        else
        {
            print("You lost");
            MicrogameController.instance.setVictory(victory: false, final: true);
            AudioSource audio = gameObject.AddComponent<AudioSource>();
            audio.PlayOneShot((AudioClip)Resources.Load("Microgames/SumiWheel/Sounds/sumiincorrect"));
        }

        if (whlspn.correctcardschosen == 2)
        {
            MicrogameController.instance.setVictory(victory: true, final: true);
            print("You won");
            AudioSource audio = gameObject.AddComponent<AudioSource>();
            GetComponent<Animation>();
            audio.PlayOneShot((AudioClip)Resources.Load("Microgames/SumiWheel/Sounds/sumicorrect"));
        }
    }
}
