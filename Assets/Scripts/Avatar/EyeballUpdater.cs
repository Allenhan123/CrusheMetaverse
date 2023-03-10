using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NUWA.Character
{
    public class EyeballUpdater : TextureUpdater
    {
        [SerializeField] Renderer otherEye;
        [SerializeField] Renderer leftEye;
        //private AssetBundle lastMeshAb;
        protected override void Start()
        {
            base.Start();
            if (otherEye == null) Debug.LogError("Other eye is missing!");
        }

        public override bool UpdateColor(Color color)
        {
            base.UpdateColor(color);
            otherEye.sharedMaterial.SetColor(textureColor, color);
            return true;
        }

        /// <summary>
        /// 更新Feature
        /// </summary>
        /// <param name="featureObj"></param>
        public override void UpdateFeature(Object featureObj, System.Action<bool> handleUpdateComplete = null)
        {
            if (featureObj == null)
            {
                Debug.LogErrorFormat("EyeballUpdater::UpdateFeature skinObj is empty");
                return;
            }

            if (_lastTexture != null)
            {
                UnityEngine.Resources.UnloadAsset(_lastTexture);
                _lastTexture = null;
            }

            Texture2D texture = featureObj as Texture2D;
            _lastTexture = texture;

            otherEye.material.SetTexture(textureName, texture);
            leftEye.material.SetTexture(textureName, texture);

            if (handleUpdateComplete != null)
            {
                handleUpdateComplete(true);
            }
        }
    }
}

