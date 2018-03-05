using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSpin : MonoBehaviour
{
    public int sumithought1 = 0; //First shape Sumireko thinks of. Randomly generated.
    public int sumithought2 = 0; //Second shape Sumireko thinks of. Hinges off first shape.
    public int correctcardschosen = 0; //Used in CardClick to keep track of how many cards were chosen to determine victory state. Victory state = 2.
    public bool gameenabled = true; //So player can't play if game is over
    public byte safetymeasure; //So the same card can't be clicked twice

    // Use this for initialization
    void Start()
    {
        //choose cards
        sumithought1 = Random.Range(3, 6); //Pick number from 3 to 5.

        print("She thought: " + sumithought1); //----debug

        if (sumithought1 != 5) //Square shape can end up overlapping with the others if the result is 5 and 2 is subtracted to reach 3 (the square). This is in place to not allow that.
        {
            sumithought2 = sumithought1 - 2; //All results but 5 subtract 2 since there's no danger of them overlapping.
        }
        else
        {
            sumithought2 = sumithought1 - 3; //If the result is 5, subtract 3 instead (O shape) so the Square isn't rendered on top of the Star.
        }

        print("and: " + sumithought2); //----debug

        /*draw cards
         * CARD LIST
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

        CreateObject("Microgames/SumiWheel/Prefabs/Card" + sumithought1);
        CreateObject("Microgames/SumiWheel/Prefabs/Card" + sumithought2);

        //Wheel's starting angle.
        int wheelspin = Random.Range(0, 101);
        var rotationVector = transform.rotation.eulerAngles;
        rotationVector.z = wheelspin;
        transform.rotation = Quaternion.Euler(rotationVector);

        print("Wheel angle: " + wheelspin); //--------debug

    }


    //Class to draw cards
    public void CreateObject(string Cardspawn)
    {
        GameObject newObject = GameObject.Instantiate(Resources.Load(Cardspawn)) as GameObject;
    }


    // Update is called once per frame
    void Update ()
    {
        //Determine difficulty, if easy, half speed, if normal, regular speed, if hard, double speed


        //Wheel spinning
        transform.Rotate(Vector3.forward / 2);
    }
}
