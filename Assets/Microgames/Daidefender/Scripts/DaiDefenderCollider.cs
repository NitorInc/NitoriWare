using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaiDefenderCollider : MonoBehaviour {

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject LastBullet;

    private bool Alive = true;

	void Start () {
        

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Alive = false;
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        animator.Play("Dai_Lose_Animation");
        MicrogameController.instance.setVictory(victory: false, final: true);
        
    }

   
    void Update()
    {
        if (GameObject.Find("LastBullet") == false && Alive == true) 
        {
            animator.Play("Dai_Win_Animation");
            MicrogameController.instance.setVictory(victory: true, final: true);
        }

    }
}

