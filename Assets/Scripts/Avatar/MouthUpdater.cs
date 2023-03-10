using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NUWA.Character
{
    public class MouthUpdater : FeatureUpdater
    {
        [SerializeField] protected string Color = "_MouthColor";
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
    }
}

