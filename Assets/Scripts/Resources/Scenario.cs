using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NUWA.Character
{
    public class Scenario : MonoBehaviour
    {
        [System.Serializable]
        public class CharacterAnimation
        {
            public Transform parent;
            public AnimationClip bodyClip;
            public AnimationClip[] eyeAnimations;
        }

        public Animation animationPlayer;
    }

    public static partial class AnimationExtensions
    {
        public static void Play(this Animation anim, AnimationClip clip, float startTime = 0)
        {
            if (anim.clip != null)
            {
                anim.RemoveClip(anim.clip);
            }

            anim.AddClip(clip, clip.name);
            anim.clip = clip;
            AnimationState animState = anim[anim.clip.name];
            animState.time = startTime;
            anim.Play(clip.name);
        }
    }
}
