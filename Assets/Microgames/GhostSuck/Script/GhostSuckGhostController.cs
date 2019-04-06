using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSuckGhostController : MonoBehaviour {
    [SerializeField]
    private float ghostcount;
    public Transform[] ghostrig;
    public Transform[] ghostsprite;
    public Vector2 ghostcolorRange;
    // Use this for initialization
    void Start() {
        ghostcount = ghostrig.Length;
        for (int i = 0; i < ghostrig.Length; ghostrig[i++].gameObject.SetActive(true))
        {
            int tries = 1000;
            do
            {
                ghostrig[i].transform.position = new Vector3(Random.Range(-4.5f, 4.5f), Random.Range(0f, 4f), transform.position.z);
                tries--;
            }
            while ((Mathf.Abs(ghostrig[i].transform.position.x - CameraHelper.getCursorPosition().x) < 2f)
                && tries > 0);

            if (tries <= 0)
                print("TOO MANY GHOST POSITION TRIES");

        }
        for (int i = 0; i < ghostrig.Length; ghostrig[i++].gameObject.SetActive(true))
        {
            SpriteRenderer ghostSprite = ghostsprite[i].GetComponent<SpriteRenderer>();
            ghostSprite.color = new HSBColor(Random.Range(ghostcolorRange.x, ghostcolorRange.y), 1f, 1f).ToColor();
            ghostSprite.sortingOrder += i + 1;
        }
        }
    bool isInCenter(Vector3 position)
    {
        return Mathf.Abs(position.x) < 2.5f && Mathf.Abs(position.y) < 1.5f;
    }
    bool isNearOtherGhost(int index, float threshold)
    {
        for (int i = 0; i < index; i++)
        {
            if (((Vector2)ghostrig[i].transform.position - (Vector2)ghostrig[index].transform.position).magnitude < threshold)
                return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update() {

    }
    //win game once certain number of ghosts are defeated
    void killaghost()
    {
        ghostcount = ghostcount - 1f;
        if (ghostcount == 0f)
        {
            MicrogameController.instance.setVictory(true, true);
        }
}
}
