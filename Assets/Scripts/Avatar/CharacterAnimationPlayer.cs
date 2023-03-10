using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NUWA.Character
{
    public class CharacterAnimationPlayer : MonoBehaviour
    {
        public Animator animator;
        // Start is called before the first frame update
        void Start()
        {
            if(animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }

        public void Play(Scenario scenario)
        {
            //    int characterIndex = GetComponentInParent<Character>().index;
            //    mBodyAnimation = GetComponent<Animation>();
            //    if (mBodyAnimation == null) Debug.LogError("Main animation is missing!");

            //    // Play animation
            //    if (scenario.animationPlayer != null)
            //        scenario.animationPlayer.Play();
            //    mBodyAnimation.Play(scenario.animations[characterIndex].bodyClip);
            //    eyeShapeUpdater.eyeShapeAnimation.Play(scenario.animations[characterIndex].eyeAnimations[eyeShapeUpdater.eyeShapeAsset.index]);
        }

        public void PlayFrameByFrame(Scenario scenario, OnUpdateFrame onUpdateFrame, int frameRate)
        {


            //    int characterIndex = GetComponentInParent<Character>().index;
            //    var bodyAnimation = GetComponent<Animation>();
            //    if (bodyAnimation == null) Debug.LogError("Main animation is missing!");

            //    // Play animation
            //    if (scenario.animationPlayer != null)
            //        StartCoroutine(scenario.animationPlayer.PlayFrameByFrame(null, null, frameRate));
            //    StartCoroutine(bodyAnimation.PlayFrameByFrame(scenario.animations[characterIndex].bodyClip, onUpdateFrame, frameRate));
            //    StartCoroutine(eyeShapeUpdater.eyeShapeAnimation.PlayFrameByFrame(scenario.animations[characterIndex].eyeAnimations[eyeShapeUpdater.eyeShapeAsset.index], null, frameRate));
        }

        public void StopPlayingFrameByFrame()
        {
            StopAllCoroutines();
        }
    }
}
