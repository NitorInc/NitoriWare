using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.NueAbduct
{
    public class NueAbductVictimShadowShrink : MonoBehaviour
    {
        [SerializeField]
        private float shrinkTime = .5f;
        [SerializeField]
        private NueAbductVictimBehavior victim;
        [SerializeField]
        private Transform animal;
        [SerializeField]
        private bool affectScale = true;
        [SerializeField]
        private bool affectAlpha = true;

        private SpriteRenderer spriteRenderer;
        private float initialY;
        private Vector3 initialScale;
        private float initialAlpha;
        private float shrinkDuration;

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            initialY = transform.position.y;
            initialScale = transform.localScale;
            initialAlpha = getAlpha();
            shrinkDuration = shrinkTime;
        }

        void LateUpdate()
        {
            if (victim.currState == NueAbductVictimBehavior.State.Sucked)
            {
                transform.position = new Vector3(transform.position.x, initialY, transform.position.z);

                shrinkDuration -= Time.deltaTime;
                if (shrinkDuration <= 0f)
                {
                    shrinkDuration = 0f;
                    enabled = false;
                }
                float visibilityMult = shrinkDuration / shrinkTime;
                if (affectScale)
                    transform.localScale = initialScale * visibilityMult;
                if (affectAlpha)
                    setAlpha(initialAlpha * visibilityMult);
            }
            else
                initialY = transform.position.y;

        }

        float getAlpha()
        {
            return spriteRenderer.color.a;
        }

        void setAlpha(float alpha)
        {
            var color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }

}