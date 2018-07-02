using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AkiPotatoes
{
    public class AkiPotatoes_Potato : MonoBehaviour
    {
        [Header("Component References")]
        [SerializeField]
        private SpriteRenderer SpriteRenderer;

        [Header("Ripe Variables")]
        [SerializeField]
        private float TimeTillRipeness = 2.0f;

        [SerializeField]
        private bool Ripe = false;

        // Color Debug Variables
        private static Color NotRipeColor = new Color(240 / 255f, 230 / 255f, 140 / 255f);
        private static Color RipeColor = new Color(139 / 255f, 69 / 255f, 19 / 255f);

        private void Start()
        {
            if (SpriteRenderer == null)
            {
                SpriteRenderer = GetComponent<SpriteRenderer>();
            }
            SpriteRenderer.color = NotRipeColor;

            StartCoroutine(WaitTillRipenessCoroutine());
        }

        // Timer till cooked potatoes
        private IEnumerator WaitTillRipenessCoroutine()
        {
            yield return new WaitForSeconds(TimeTillRipeness);

            RipenessProcedure();
        }

        // Procedure that turns uncooked to cooked potatoes
        private void RipenessProcedure()
        {
            Ripe = true;

            SpriteRenderer.color = RipeColor;
        }

        // Procedure when the potato is clicked
        private void OnMouseDown()
        {
            if (Ripe)
            {
                AkiPotatoes_Controller.singleton.AddCookedPotato();
            }

            Destroy(gameObject);
        }
    }
}
