using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemiCover_Remi_HealthBehaviour : MonoBehaviour {

    private float HP = 1;                           // Remilia's Health Points.
    public float burnSpeed;                         // How much will HP decrease when Remilia's collider is exposed to sunlight?
    private int collidersOutside = 0;               // How many colliders are outside of Umbrella's shadow?

    public ParticleSystem smokeParticles;
    private ParticleSystem smokeInstance;

    private SpriteRenderer[] sprite;
    private bool inmunity = false;


    // Use this for initialization
    void Start() {
        smokeInstance = (ParticleSystem)Instantiate(smokeParticles, transform.position, smokeParticles.transform.rotation);
        sprite = GetComponentsInChildren<SpriteRenderer>(true);
    }


    // Update is called once per frame
    void Update() {

        if (!MicrogameController.instance.getVictoryDetermined() && !inmunity){
            updateHP();
            if (HP <= 0)
                GameOver(); 
        }
        
        // Emission
        manageEmission();
		if (MicrogameController.instance.getVictory())
            manageSpriteColor();

    }

	private void manageSpriteColor()
	{
		//Color lerps from white to light red based on HP, then pure red like normal on failure
		changeSpriteColor(new Color(1f, Mathf.Lerp(.5f, 1f, HP), Mathf.Lerp(.5f, 1f, HP)));
	}

    private void manageEmission()
    {
        var emission = smokeInstance.emission;
       
        smokeInstance.transform.position = transform.position + (Vector3.up * .5f);
        var smokeModule = smokeInstance.main;
		smokeModule.startSize = (((1 - HP) * 90) / 25)
			* (MicrogameController.instance.getVictory() ? 1f : 1.25f);	//Particle size intensifies on death

        emission.rateOverTime = (((1 - HP) * 2000) / 10)
			* (MicrogameController.instance.getVictory() ? 1f : 1.5f);	//Particle rate intensifies on death

    }


    // Decrease HP value if some colliders are outside of Umbrella's Shadow
    private void updateHP()
    {
		if (MicrogameTimer.instance.beatsLeft < .5f)
			return;
		if (collidersOutside == 0)
			this.HP = Mathf.Min(this.HP + (burnSpeed * Time.deltaTime * .65f), 1f);
		else
			this.HP -= burnSpeed * Time.deltaTime * collidersOutside;
    }


    // Game is over
    private void GameOver()
    {
        MicrogameController.instance.setVictory(false, true);
        GetComponent<Animator>().SetInteger("MovementAnimation", 5);
    }

    public void setInmunnity(bool inmunity_value)
    {

        if (inmunity_value)
            smokeInstance.enableEmission = false;

        else
        {
            smokeInstance.transform.position = transform.position + (Vector3.up * .5f);
            smokeInstance.enableEmission = true;
        }

        this.inmunity = inmunity_value;

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "UmbrellaShadow")
        {
            collidersOutside += 1;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "UmbrellaShadow" && collidersOutside != 0)
        {
            collidersOutside -= 1;
        }
    }


    private void changeSpriteColor(Color color)
    {
        foreach (SpriteRenderer sr in sprite)
        {
            sr.color = color;
        }
    }


}
