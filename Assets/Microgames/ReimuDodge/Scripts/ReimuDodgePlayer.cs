using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgePlayer : MonoBehaviour
{
    
    // This will happen when the player's hitbox collides with a bullet
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Test that this works
        print("Player was hit!");
    }
    
    
}
