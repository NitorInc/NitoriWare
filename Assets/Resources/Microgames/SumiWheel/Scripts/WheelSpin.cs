using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSpin : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        //choose cards
        int sumithought1 = Random.Range(3, 6);

        print(sumithought1); //----debug

        int sumithought2;

        if (sumithought1 != 5)
        {
            sumithought2 = sumithought1 - 2;
        }
        else
        {
            sumithought2 = sumithought1 - 3;
        }
         
        print(sumithought2); //----debug

        /*draw cards
        1=Plus, 2=O, 3=Square, 4=Wave, 5=Star*/

        CreateObject("Microgames/SumiWheel/Prefabs/Card" + sumithought1);
        CreateObject("Microgames/SumiWheel/Prefabs/Card" + sumithought2);

        //Wheel's starting angle
        int wheelspin = Random.Range(0, 101);

        print(wheelspin); //--------debug

        var rotationVector = transform.rotation.eulerAngles;
        rotationVector.z = wheelspin;
        transform.rotation = Quaternion.Euler(rotationVector);
    }

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
