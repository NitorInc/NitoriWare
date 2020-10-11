using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemiCover_Remi_HealthBehaviour : MonoBehaviour {

    private float HP = 1;                           // Remilia's Health Points.
    public float burnSpeed;                         // How much will HP decrease when Remilia's collider is exposed to sunlight?
    public Vector2 burnBeatBounds;                   // Between what beats can remi be hurt?

    public ParticleSystem smokeParticles;
    private ParticleSystem smokeInstance;

    private SpriteRenderer[] sprite;
    private bool inmunity = false;

    public AudioSource burningSFX;
    public AudioClip defeatSFX;

    private BoxCollider2D[] colliders;

    // Use this for initialization
    void Start() {
        smokeInstance = (ParticleSystem)Instantiate(smokeParticles, transform.position, smokeParticles.transform.rotation);
        sprite = transform.GetChild(0).GetComponentsInChildren<SpriteRenderer>(true);
        burningSFX.pitch = 1f;
        colliders = GetComponents<BoxCollider2D>();
    }


    // Update is called once per frame
    void Update() {

        var beatsLeft = MicrogameController.instance.session.BeatsRemaining;
        if (beatsLeft <= burnBeatBounds.x && beatsLeft >= burnBeatBounds.y
            && !MicrogameController.instance.getVictoryDetermined() && !inmunity){
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

        manageEmissionSound();

    }

    private void manageEmissionSound()
    {
        if (inmunity)
            burningSFX.volume = 0;
        else
            burningSFX.volume = (1 - HP) * 1.5f;
    }

    // Decrease HP value if some colliders are outside of Umbrella's Shadow
    private void updateHP()
    {
        int collidersOutside = getCollidersOutside();
		if (MicrogameController.instance.session.BeatsRemaining < .5f)
			return;
		if (collidersOutside == 0)
			HP = Mathf.Min(HP + (burnSpeed * Time.deltaTime * .65f), 1f);
		else
			HP -= burnSpeed * Time.deltaTime * collidersOutside;
    }


    // Game is over
    private void GameOver()
    {
        MicrogameController.instance.setVictory(false, true);
        GetComponent<Animator>().SetInteger("MovementAnimation", 5);
        MicrogameController.instance.playSFX(defeatSFX);
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

        inmunity = inmunity_value;

    }

    int getCollidersOutside()
    {
        int collidersOutside = colliders.Length;
        foreach (var checkCollider in colliders)
        {
            if (isColliderUnderUmbrella(checkCollider))
                collidersOutside--;
        }
        return collidersOutside;
    }

    bool isColliderUnderUmbrella(BoxCollider2D collider)
    {
        float colliderHalfWidth = collider.bounds.extents.x;
        float yOffset = 3.75f;
        var hit = PhysicsHelper2D.visibleRaycast((Vector2)(new Vector3(collider.transform.position.x - colliderHalfWidth, yOffset, 0f)),
            Vector2.right, colliderHalfWidth * 2f);
        return hit && hit.collider.name.Equals("UmbrellaShadow");
    }

    private void changeSpriteColor(Color color)
    {
        foreach (SpriteRenderer sr in sprite)
        {
            sr.color = color;
        }
    }


}
