using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NUWA.Character
{

    public delegate void OnUpdateFrame(bool hasNextFrame);
    public class GifScenario : Scenario
    {
        public Camera recordCamera;
    }

    public static partial class AnimationExtensions
    {
        public static IEnumerator PlayFrameByFrame(this Animation anim, AnimationClip clip, OnUpdateFrame onUpdateFrame = null, int frameRate = 24)
        {
            // Setup animation
            if (clip != null)
            {
                anim.AddClip(clip, clip.name);
                anim.clip = clip;
            }
            AnimationState animState = anim[anim.clip.name];
            animState.speed = 0;
            anim.Play();

            yield return null;

            // Play frame by frame
            float frameTime = 1f / frameRate;
            int frameCount = (int)(frameRate * animState.length);
            for (int i = 1; i <= frameCount; i++)
            {
                if (onUpdateFrame != null) onUpdateFrame(true);
                animState.time = frameTime * i;
                //Debug.Log("onUpdateFrame invoked, time : " + animState.time + ", Time : " + Time.time);
                yield return null;
            }

            if (onUpdateFrame != null) onUpdateFrame(false);
        }
    }
}
