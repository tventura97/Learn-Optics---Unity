using UnityEngine;

using System;
using System.Collections;

namespace DigitalRuby.AnimatedLineRenderer
{
    [RequireComponent(typeof(AnimatedLineRenderer))]
    public class PhasorScript : MonoBehaviour
    {
        /// <summary>
        /// Action that will be called whenever objects are hit by the phasor
        /// </summary>
        [HideInInspector]
        public Action<RaycastHit2D[]> HitCallback;

        [Tooltip("Source of the phasor")]
        public GameObject Source;

        [Tooltip("Target to fire at")]
        public GameObject Target;

        [Tooltip("Sound to make when the phasor fires")]
        public AudioSource FireSound;

        private AnimatedLineRenderer lineRenderer;
        private bool firing;
        private bool endingFiring;
        private int endFireToken;

        private void Start()
        {
            lineRenderer = GetComponent<AnimatedLineRenderer>();
        }

        private void Update()
        {
            if (CanEndFire())
            {
                RaycastHit2D[] hits = Physics2D.CircleCastAll(lineRenderer.StartPoint, lineRenderer.EndWidth * 0.5f,
                    lineRenderer.EndPoint - lineRenderer.StartPoint, Vector3.Distance(lineRenderer.EndPoint, lineRenderer.StartPoint));
                if (hits != null && hits.Length != 0)
                {
                    EndFire(hits[0].point);
                    if (HitCallback != null)
                    {
                        HitCallback(hits);
                    }
                }
            }
        }

        private bool CanEndFire()
        {
            return (firing && !endingFiring && !lineRenderer.Resetting);
        }

        private void EndFire(Vector3? endPoint)
        {
            endFireToken++;
            endingFiring = true;
            lineRenderer.ResetAfterSeconds(0.2f, endPoint, () =>
            {
                firing = false;
                endingFiring = false;
            });
        }

        private IEnumerator EndFireDelay(float delay, int token)
        {
            if (delay > 0.0f)
            {
                yield return new WaitForSeconds(delay);
            }

            if (endFireToken == token)
            {
                EndFire(null);
            }
        }

        /// <summary>
        /// Fire the phasor, using the source and target parameters of this class
        /// </summary>
        /// <returns>True if able to fire, false if already firing</returns>
        public bool Fire()
        {
            return Fire(Source.transform.position, Target.transform.position);
        }

        /// <summary>
        /// Fire the phasor, using the target specified
        /// </summary>
        /// <param name="target">Target to fire at</param>
        /// <returns>True if able to fire, false if already firing</returns>
        public bool Fire(Vector3 target)
        {
            return Fire(Source.transform.position, target);
        }

        /// <summary>
        /// Fire the phasor at a target
        /// </summary>
        /// <param name="target">Target to fire at</param>
        /// <returns>True if able to fire, false if already firing</returns>
        public bool Fire(Vector3 source, Vector3 target)
        {
            if (firing)
            {
                return false;
            }

            firing = true;
            lineRenderer.Enqueue(source);
            lineRenderer.Enqueue(target);
            StartCoroutine(EndFireDelay(lineRenderer.SecondsPerLine, ++endFireToken));
            if (FireSound != null)
            {
                //FireSound.Play();
            }
            return true;
        }
    }
}