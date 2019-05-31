using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FoodRoast
{
    public enum PotatoCookedState { Uncooked, Cooked, Burnt }

    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public class FoodRoast_Potato : MonoBehaviour
    {

        [SerializeField]
        private Vector2 launchVelXRange;
        [SerializeField]
        private Vector2 launchVelYRange;
        [SerializeField]
        private float launchGravity;
        [SerializeField]
        private SpriteRenderer xSpriteRenderer;
        [SerializeField]
        private AudioClip readySound;
        [SerializeField]
        private AudioClip pickSound;
        [SerializeField]
        private AudioClip failSound;
        [SerializeField]
        private Animator rigAnimator;

        bool picked = false;


        private SpriteRenderer _SpriteRenderer;
        public SpriteRenderer SpriteRenderer
        {
            get
            {
                if (_SpriteRenderer == null)
                    _SpriteRenderer = GetComponent<SpriteRenderer>();
                return _SpriteRenderer;
            }
        }

        private void Update()
        {
            if (MicrogameController.instance.getVictoryDetermined() && !MicrogameController.instance.getVictory())
            {
                rigAnimator.SetInteger("State", 2);
                enabled = false;
            }
        }

        private ParticleSystem[] _Particles;
        public ParticleSystem[] Particles
        {
            get
            {
                if (_Particles == null)
                    _Particles = GetComponentsInChildren<ParticleSystem>();
                return _Particles;
            }
        }

        [System.Serializable]
        private struct PotatoSpritePack
        {
            public Sprite[] Pack;
        }

        [Header("Potato Sprites (0 - Uncooked, 1 - Cooked, 2 - Burnt)")]
        [SerializeField]
        private PotatoSpritePack[] PotatoSprites;
        private Sprite GetSprite => PotatoSprites[(int)PotatoCookedState].Pack[PotatoAnimationState];
        private int GetPackLength => PotatoSprites[0].Pack.Length;

        [Header("Animation States")]
        [SerializeField]
        private float StateDuration = 0.33f;
        [SerializeField] private int PotatoAnimationState = 0;
        [ReadOnly] [SerializeField] private PotatoCookedState PotatoCookedState = 0;

        private void Start()
        {
            var rb2d = GetComponent<Collider2D>();
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                var otherCol2d = transform.parent.GetChild(i).GetComponent<Collider2D>();
                if (otherCol2d != rb2d)
                    Physics2D.IgnoreCollision(rb2d, otherCol2d);
            }

            PotatoCookedState = PotatoCookedState.Uncooked;
            StartCoroutine(AnimationCoroutine());
            StartCoroutine(CookingCoroutine());
        }

        private IEnumerator AnimationCoroutine()
        {
            float time = 0;
            while (!MicrogameController.instance.getVictoryDetermined() && !picked)
            {
                SpriteRenderer.sprite = GetSprite;
                while (time < StateDuration)
                {
                    yield return null;
                    time += Time.deltaTime;
                }
                time -= StateDuration;
                PotatoAnimationState = (PotatoAnimationState + 1) % GetPackLength;
            }
        }

        // Timer till cooked potatoes
        private IEnumerator CookingCoroutine()
        {
            yield return new WaitForSeconds(FoodRoast_Controller.Instance.GetCookTime());
            if (!MicrogameController.instance.getVictoryDetermined() && !picked)
                CookedProcedure();
            yield return new WaitForSeconds(FoodRoast_Controller.Instance.GetPotatoBurnTime);
            if (!MicrogameController.instance.getVictoryDetermined() && !picked)
                BurntProcedure();
        }

        // Procedure that turns uncooked to cooked potatoes
        private void CookedProcedure()
        {
            PotatoCookedState = PotatoCookedState.Cooked;
            SpriteRenderer.sprite = GetSprite;

            ChangeParticlesStartColorAlpha(.05f);
            foreach (var particle in Particles)
                particle.Play();

            rigAnimator.SetInteger("State", 1);
            MicrogameController.instance.playSFX(readySound, AudioHelper.getAudioPan(transform.position.x));
        }

        // Procedure that turns uncooked to cooked potatoes
        private void BurntProcedure()
        {
            PotatoCookedState = PotatoCookedState.Burnt;
            SpriteRenderer.sprite = GetSprite;

            ChangeParticlesStartColorAlpha(.4f);
            foreach (var particle in Particles)
                particle.Play();


            rigAnimator.SetInteger("State", 2);
            FoodRoast_VictoryController.Instance.setVictory(false);
            MicrogameController.instance.playSFX(failSound, AudioHelper.getAudioPan(transform.position.x));

        }

        private void ChangeParticlesStartColorAlpha(float alpha)
        {
            foreach (var particle in Particles)
            {
                var main = particle.main;
                var colorStruct = main.startColor;
                var color = colorStruct.color;

                color.a = alpha;
                colorStruct.color = color;
                main.startColor = colorStruct;
            }
        }

        // Procedure when the potato is clicked
        private void OnMouseDown()
        {
            if (MicrogameController.instance.getVictoryDetermined() || picked)
                return;
            if (PotatoCookedState == PotatoCookedState.Cooked)
            {
                Pick(true);
            }
            else if (PotatoCookedState == PotatoCookedState.Uncooked)
            {
                Pick(false);
            }
            foreach (var particle in Particles)
            {
                particle.Stop();
                particle.transform.SetParent(null);
            }

        }

        void Pick(bool isReady)
        {
            picked = true;
            rigAnimator.SetInteger("State", 3);
            if (isReady)
            {
                FoodRoast_Controller.Instance.AddCookedPotato();
                var rb2d = GetComponent<Rigidbody2D>();
                var flingVel = new Vector2(MathHelper.randomRangeFromVector(launchVelXRange), MathHelper.randomRangeFromVector(launchVelYRange));
                rb2d.bodyType = RigidbodyType2D.Dynamic;
                rb2d.velocity = flingVel;
                rb2d.gravityScale = launchGravity;
                MicrogameController.instance.playSFX(pickSound, AudioHelper.getAudioPan(transform.position.x));
            }
            else
            {
                FoodRoast_Controller.Instance.AddUncookedPotato();
                MicrogameController.instance.playSFX(failSound, AudioHelper.getAudioPan(transform.position.x));
                xSpriteRenderer.enabled = true;
            }
            
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.name.ToLower().Contains("floor"))
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }
}

