﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KagerouCutController : MonoBehaviour {
    
    [SerializeField]
    private Sprite[] characterSprites; 

    [SerializeField]
    private Color[] hairColors;
    
//    [SerializeField]
//    private AudioClip[] soundTracks;
    
    [SerializeField]
    private int furballCount = 0;
   
    [SerializeField]
    private KagCutCharacter characterPrefab;
    [SerializeField] 
    private RazorController razor;
    [SerializeField]
    private GameObject background;

    [SerializeField]
    private float furspeed = 0.01f;
    [SerializeField]
    private float furDistance = 1.2f;

    [SerializeField]
    private float particleRate = 0.2f;
    
    public bool shouldExplode = false;

    private GameObject[] furballs;
    private KagCutCharacter character;

    // Use this for initialization
	void Start () {
        furballs = new GameObject[furballCount];
        // Set the sprites
        int randChar = Random.Range(0, characterSprites.Length);
        Sprite charSprite = characterSprites[randChar];
        Color hairColor = hairColors[randChar];
        //AudioClip music = soundTracks[randChar];

	    character = Instantiate(characterPrefab);
        character.GetComponent<SpriteRenderer>().sprite = charSprite; 
        
        GameObject furball = character.transform.Find("FurBall").gameObject;
        GameObject spriteObj = furball.transform.Find("Sprite").gameObject;
        GameObject furExplo = furball.transform.Find("FurExplosion").gameObject;
        spriteObj.GetComponent<SpriteRenderer>().color = hairColor;
        
        ParticleSystem.MainModule partMod = furball.GetComponent<ParticleSystem>().main;
        ParticleSystem.MinMaxGradient partColor = new ParticleSystem.MinMaxGradient(hairColor); 
        partMod.startColor = partColor;
        partMod = furExplo.GetComponent<ParticleSystem>().main;
        partMod.startColor = partColor;
       
        //AudioSource player = MicrogameController.instance.GetComponent<AudioSource>();
        //player.clip = music;
        //player.Play();

        float angle = -0.1f;
        float[] angles= new float[furballCount];
        for (int i=0; i<furballCount; i++){
            angle = Random.Range(angle-0.2f, angle-Mathf.PI/furballCount-0.1f);
            angles[i] = angle;
        }
        float centerShift = angles[0] + angles[furballCount-1] + Mathf.PI;
        for (int i=0; i<furballCount; i++){
            angles[i] -= centerShift/2+Random.Range(-0.1f, 0.1f);
            if (angles[i] < -Mathf.PI/2){
                angles[i] -= 0.3f;
            } else {
                angles[i] += 0.3f;
            }
        }

        for (int i=0; i<furballCount; i++){
            angle = angles[i];
            float x = razor.transform.position.x + furDistance * Mathf.Cos(angle);
            float y = razor.transform.position.y + furDistance * Mathf.Sin(angle);
            GameObject newFur = Instantiate(furball, new Vector3(x, y, 0), Quaternion.identity);
            FurBallController s = newFur.GetComponent<FurBallController>();
            s.speed = furspeed;
            s.particleRate = particleRate;
            s.shouldExplode = shouldExplode;
            newFur.GetComponent<Animator>().SetFloat("offset", Random.Range(0f, 1f));
            furballs[i] = newFur;
        }
        furball.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		bool ballsLeft = false;
        foreach (GameObject ball in furballs){
            if (ball.tag != "Finish"){
                ballsLeft = true;
                break;
            }
        }
        if (!ballsLeft) {
            character.GetComponent<Animator>().SetTrigger("Win");
            background.GetComponent<Animator>().SetTrigger("Win");
            MicrogameController.instance.setVictory(victory: true, final: true);
        }
	}
}
