using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClick : MonoBehaviour
{

    void OnMouseDown()
    {
        byte cardclick = 0; //Set this variable to the card.

        GameObject wheel = GameObject.Find("Wheel"); //Finds the Wheel object and whatever's on it.
        WheelSpin whlspn = wheel.GetComponent<WheelSpin>(); //Accesses the WheelSpin class.
        if (whlspn.gameenabled == true) //To check if the player hasn't actually finished the game. If false, he can't click anything else.
        {
        print("Sumithought 1 from cardclick script is: " + whlspn.sumithought1); 
        print("Sumithought 2 from cardclick script is: " + whlspn.sumithought2); //debug to prove that Sumireko's thoughts were loaded correctly.
        print("Before any of the cards are checked right now, safety measure is set to: " + whlspn.safetymeasure);
        print("And, right now, cardclick is: " + cardclick);

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

        //Gets the object of whatever was clicked and sets a variable according to what it was, so it's later checked to see if it corresponds with her thoughts.
            if (gameObject.name == ("Card1"))
            {
                if (whlspn.safetymeasure != 1)
                {
                    cardclick = 1; //Sets to Plus
                    whlspn.safetymeasure = 1;
                    gameObject.GetComponent<Renderer>().material.color = Color.gray;
                    
                }
                else
                {
                    print("Can't click this card"); //This is here for the sole reason of having something for this else.
                }
            }
            else if (gameObject.name == ("Card2"))
            {
                if (whlspn.safetymeasure != 2)
                {
                    cardclick = 2; //Sets to O
                    whlspn.safetymeasure = 2;
                    gameObject.GetComponent<Renderer>().material.color = Color.gray;
                }
                else
                {
                    print("Can't click this card"); //This is here for the sole reason of having something for this else.
                }
            }
            else if (gameObject.name == ("Card3"))
            {
                if (whlspn.safetymeasure != 3)
                {
                    cardclick = 3; //Sets to Square
                    whlspn.safetymeasure = 3;
                    gameObject.GetComponent<Renderer>().material.color = Color.gray;
                }
                else
                {
                    print("Can't click this card"); //This is here for the sole reason of having something for this else.
                }
            }
            else if (gameObject.name == ("Card4"))
            {
                if (whlspn.safetymeasure != 4)
                {
                    cardclick = 4; //Sets to Wave
                    whlspn.safetymeasure = 4;
                    gameObject.GetComponent<Renderer>().material.color = Color.gray;
                }
                else
                {
                    print("Can't click this card"); //This is here for the sole reason of having something for this else.
                }
            }
            else if (gameObject.name == ("Card5"))
            {
                if (whlspn.safetymeasure != 5)
                {
                    cardclick = 5; //Sets to Star
                    whlspn.safetymeasure = 5;
                    gameObject.GetComponent<Renderer>().material.color = Color.gray;
                }
                else
                {
                    print("Can't click this card"); //This is here for the sole reason of having something for this else.
                }
            }
            else if (gameObject.name == ("Card6"))
            {
                if (whlspn.safetymeasure != 6)
                {
                    cardclick = 6; //Sets to Purple Plus
                    whlspn.safetymeasure = 6;
                    gameObject.GetComponent<Renderer>().material.color = Color.gray;
                }
                else
                {
                    print("Can't click this card"); //This is here for the sole reason of having something for this else.
                }
            }
            else if (gameObject.name == ("Card7"))
            {
                if (whlspn.safetymeasure != 7)
                {
                    cardclick = 7; //Sets to Purple O
                    whlspn.safetymeasure = 7;
                    gameObject.GetComponent<Renderer>().material.color = Color.gray;
                }
                else
                {
                    print("Can't click this card"); //This is here for the sole reason of having something for this else.
                }
            }
            else if (gameObject.name == ("Card8"))
            {
                if (whlspn.safetymeasure != 8)
                {
                    cardclick = 8; //Sets to Purple Square
                    whlspn.safetymeasure = 8;
                    gameObject.GetComponent<Renderer>().material.color = Color.gray;
                }
                else
                {
                    print("Can't click this card"); //This is here for the sole reason of having something for this else.
                }
            }
            else if (gameObject.name == ("Card9"))
            {
                if (whlspn.safetymeasure != 9)
                {
                    cardclick = 9; //Sets to Purple Wave
                    whlspn.safetymeasure = 9;
                    gameObject.GetComponent<Renderer>().material.color = Color.gray;
                }
                else
                {
                    print("Can't click this card"); //This is here for the sole reason of having something for this else.
                }
            }
            else if (gameObject.name == ("Card10"))
            {
                if (whlspn.safetymeasure != 10)
                {
                    cardclick = 10; //Sets to Purple Star
                    whlspn.safetymeasure = 10;
                    gameObject.GetComponent<Renderer>().material.color = Color.gray;
                }
                else
                {
                    print("Can't click this card"); //This is here for the sole reason of having something for this else.
                }
            }

            AudioSource audio1 = gameObject.AddComponent<AudioSource>();
            audio1.PlayOneShot((AudioClip)Resources.Load("Microgames/SumiWheel/Sounds/click3"));

            print("Safety measure is now, after the click: " + whlspn.safetymeasure);
            print("The name of the object you just clicked is " + gameObject.name); //debug
            print("Therefore cardclick is set to " + cardclick);                    //debug

            //Win-Lose condition
            if (cardclick == 0)
            {
                print("Do nothing, because card click is 0");
            }
            else
            {
                if (whlspn.sumithought1 == cardclick)
                {
                    whlspn.correctcardschosen++; //Increment the cards chosen variable. If this increments to 2, game is won.
                                                 //ADD A SOUND EFFECT FOR THIS
                    print("Correct cards picked: " + whlspn.correctcardschosen); //debug

                }
                else if (whlspn.sumithought2 == cardclick)
                {
                    whlspn.correctcardschosen++; //Increment the cards chosen variable. If this increments to 2, game is won.
                                                 //ADD A SOUND EFFECT FOR THIS
                    print("Correct cards picked: " + whlspn.correctcardschosen); //debug
                }
                else
                {
                    print("You lost"); //debug
                    MicrogameController.instance.setVictory(victory: false, final: true); //Sets game as lost.
                    AudioSource audio = gameObject.AddComponent<AudioSource>();
                    audio.PlayOneShot((AudioClip)Resources.Load("Microgames/SumiWheel/Sounds/sumiincorrect")); //Plays beeping noise for failure.
                    GameObject sumi = GameObject.Find("Sumireko");
                    Animator sumianimate;
                    sumianimate = sumi.GetComponent<Animator>(); //Gets all the animations from Sumireko
                    sumianimate.SetTrigger("Failure"); //Sets Failure trigger to true which plays her Failure animation
                    whlspn.gameenabled = false;
                }

                if (whlspn.correctcardschosen == 2) //Checks to see if after you clicked a card, the correct cards chosen variable was incremented to 2, which implies a victory.
                {
                    MicrogameController.instance.setVictory(victory: true, final: true); //Sets game as won.
                    print("You won"); //debug
                    AudioSource audio = gameObject.AddComponent<AudioSource>();
                    audio.PlayOneShot((AudioClip)Resources.Load("Microgames/SumiWheel/Sounds/sumicorrect")); //Plays beeping noise for victory.
                    GameObject sumi = GameObject.Find("Sumireko");
                    Animator sumianimate;
                    sumianimate = sumi.GetComponent<Animator>(); //Gets all the animations from Sumireko
                    sumianimate.SetTrigger("Victory"); //Sets Victory trigger to true which plays her Victory animation
                    whlspn.gameenabled = false;
                }
            }
        }
        else //If you've won or failed, you can't play.
        {
            print("click rejected"); //This is here for the sole reason of having something for this else.
        }
    }
}
