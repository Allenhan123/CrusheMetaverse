using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NUWA.Character
{
    [System.Serializable]
    public abstract class FeatureUpdater : MonoBehaviour
    {
        public FeatureType featureType;
        public virtual Renderer mMeshRenderer
        {
            get
            {
                if (_meshRenderer == null)
                    _meshRenderer = GetComponent<Renderer>();
                return _meshRenderer;
            }
            set
            {
                _meshRenderer = value;
            }
        }
        private Renderer _meshRenderer;

        protected virtual void Start()
        {

        }


        public abstract bool UpdateColor(Color color);
        public abstract void UpdateFeature(UnityEngine.Object featureObj, System.Action<bool> handleUpdateComplete = null);
    }
}


