using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomGame_Camera : MonoBehaviour {
    public DoomGame_Player player;
    void OnRenderImage (RenderTexture source, RenderTexture destination) {
        if (player.dead)
            player.dead_lerp = Mathf.Clamp (player.dead_lerp + Time.deltaTime * 2, 0, 1);
        player.screen.SetFloat ("_Amount", player.dead_lerp);
        player.screen.SetFloat ("_Flash", player.shake);
        Graphics.Blit (source, destination, player.screen);
    }
}