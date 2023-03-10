using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NUWA.Character
{
    public class SkinColorUpdater : FeatureUpdater
    {
        [SerializeField] protected string Color = "_SkinColor";
        public override bool UpdateColor(Color color)
        {
            if (mMeshRenderer != null)
            {
                mMeshRenderer.material.SetColor(Color, color);
            }

            return true;
        }

        public override void UpdateFeature(Object featureObj, System.Action<bool> handleUpdateComplete = null)
        {
            if (handleUpdateComplete != null)
            {
                handleUpdateComplete(true);
            }
        }

        public virtual Color GetSkinColor()
        {
            if (mMeshRenderer == null)
            {
                return UnityEngine.Color.white;
            }
            return mMeshRenderer.material.GetColor(Color);
        }
    }
}

