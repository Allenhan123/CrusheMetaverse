using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NUWA.Character
{

    public class TextureUpdater : FeatureUpdater
    {
        [SerializeField] protected string textureName = "_MainTex";
        [SerializeField] protected string textureColor = "_Color";
        protected UnityEngine.Object _lastTexture;
        protected Color _lastColor;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
        }

        public override bool UpdateColor(Color color)
        {
            if (mMeshRenderer != null)
            {
                mMeshRenderer.material.SetColor(textureColor, color);
            }

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
                Debug.LogErrorFormat(string.Format("TextureUpdater::UpdateFeature featureObj is empty featureType => {0}", featureType.ToString()));
                return;
            }

            if (_lastTexture != null)
            {
                UnityEngine.Resources.UnloadAsset(_lastTexture);
                _lastTexture = null;
            }

            Texture texture = featureObj as Texture;
            _lastTexture = texture;
            if (texture != null)
            {
                mMeshRenderer.material.SetTexture(textureName, texture);
            }

            if (handleUpdateComplete != null)
            {
                handleUpdateComplete(true);
            }
        }
    }
}

