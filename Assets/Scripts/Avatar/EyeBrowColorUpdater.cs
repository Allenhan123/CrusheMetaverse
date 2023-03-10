using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NUWA.Character
{
    public class EyeBrowColorUpdater : FeatureUpdater
    {
        public const string EYEBROW_COLOR = "_EyebrowColor";

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
                    mat.SetColor(EYEBROW_COLOR, color);
                    break;
                }
            }
            return true;
        }

        public override void UpdateFeature(UnityEngine.Object featureObj, Action<bool> handleUpdateComplete = null)
        {
            if (handleUpdateComplete != null)
            {
                handleUpdateComplete(true);
            }
            return;
        }
    }
}
