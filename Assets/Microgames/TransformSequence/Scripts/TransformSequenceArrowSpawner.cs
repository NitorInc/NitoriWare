using UnityEngine;

public class TransformSequenceArrowSpawner : MonoBehaviour
{

    public GameObject arrow;
    public GameObject player;

    private float beat = 2*6 / 13f;
    private float timer = 0;

    private void Start()
    {
        timer = arrow.GetComponent<TransformSequenceArrow>().failTime;
    }

    void Update()
    {
        if (!MicrogameController.instance.getVictory())
        {
            player.GetComponent<Animator>().SetBool("Failed", true);
            return;
        }
        timer += Time.deltaTime;
        if (timer > beat)
        {
            timer -= beat;
            SpawnArrow();
        }
    }

    private void SpawnArrow()
    {
        GameObject newArrow = Instantiate(arrow, transform);
        TransformSequenceArrow arrowScript = newArrow.GetComponent<TransformSequenceArrow>();
        var angle = newArrow.transform.localEulerAngles;
        switch (Random.Range(0, 4))
        {
            case 0: // left
                arrowScript.key = KeyCode.LeftArrow;
                angle.z = 90;
                break;
            case 1: // right
                arrowScript.key = KeyCode.RightArrow;
                angle.z = -90;
                break;
            case 2: // up
                arrowScript.key = KeyCode.UpArrow;
                angle.z = 0;
                break;
            case 3: // down
                arrowScript.key = KeyCode.DownArrow;
                angle.z = 180;
                break;
        }
        newArrow.transform.eulerAngles = angle;
    }
}
