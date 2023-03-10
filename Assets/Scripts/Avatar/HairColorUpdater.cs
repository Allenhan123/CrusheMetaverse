using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NUWA.Character
{
    public class HairColorUpdater : FeatureUpdater
    {

        public const string START_COLOR = "_StartColor";
        public const string END_COLOR = "_EndColor";

        public override bool UpdateColor(Color color)
        {
            if (mMeshRenderer == null || mMeshRenderer.sharedMaterials == null || mMeshRenderer.sharedMaterials.Length == 0)
            {
                Debug.LogError(string.Format("HairColorUpdater::UpdateColor skinMeshRender or skinMeshRender Materials is null  featureType => {0}", featureType.ToString()));
                return false;
            }
            foreach (var mat in mMeshRenderer.sharedMaterials)
            {
                if (mat != null)
                {
                    mat.SetColor(START_COLOR, color);
                    break;
                }
            }
            return true;
        }

        public bool UpdateEndColor(Color color)
        {
            foreach (var mat in mMeshRenderer.sharedMaterials)
            {
                if (mat != null)
                {
                    mat.SetColor(END_COLOR, color);
                    break;
                }

            }
            return true;
        }

        public override void UpdateFeature(UnityEngine.Object featureObj, System.Action<bool> handleUpdateComplete = null)
        {
            if (handleUpdateComplete != null)
            {
                handleUpdateComplete(true);
            }
            return;
        }
    }
}

