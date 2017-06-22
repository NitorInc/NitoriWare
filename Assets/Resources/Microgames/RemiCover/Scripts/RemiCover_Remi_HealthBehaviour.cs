using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemiCover_Remi_HealthBehaviour : MonoBehaviour {

    [SerializeField]
    private float HP = 1;                           // Remilia's Health Points.
    public float burnSpeed;                         // How much will HP decrease when Remilia's collider is exposed to sunlight?

    [SerializeField]
    private int collidersOutside = 0;               // How many colliders are outside of Umbrella's shadow?

    private bool continueUpdate = true;

    private GameObject remiliaSprite = null;
    public ParticleSystem smokeParticles;
    private ParticleSystem smokeInstance;
    private ParticleSystem.MainModule smokeInstanceModule;
	private SpriteRenderer remiSpriteRenderer;

    // Use this for initialization
    void Start() {
        HP = 1;
        remiliaSprite = transform.Find("RemiSprite").gameObject;
        smokeInstance = (ParticleSystem)Instantiate(smokeParticles, remiliaSprite.transform.position, smokeParticles.transform.rotation);
        var emission = smokeInstance.emission;
        collidersOutside = 0;
		remiSpriteRenderer = remiliaSprite.GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update() {
        if (continueUpdate)
        {
            updateHP();
            if (HP <= 0) GameOver();
        }

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
       
        smokeInstance.transform.position = remiliaSprite.transform.position + (Vector3.up * .5f);
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
        continueUpdate = false;
        gameObject.SendMessage("characterHasDied");
        MicrogameController.instance.setVictory(false, true);
        changeSpriteColor(Color.red);                                   // ONLY FOR DEBUGING
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
        remiSpriteRenderer.color = color;
    }


}
