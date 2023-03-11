using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NUWA.Character
{
    public class HeadUpdater : MonoBehaviour
    {
        private Renderer _meshRenderer;
        protected void Start()
        {
            if(_meshRenderer == null)
            {
                _meshRenderer = GetComponent<Renderer>();
            }            
        }

        public void SyncMaterials(SkinnedMeshRenderer bodyMesh)
        {
            Debug.LogWarning("------SyncMaterials-----");
            if (bodyMesh != null && _meshRenderer != null)
            {
                _meshRenderer.sharedMaterials = bodyMesh.sharedMaterials;
            }
            
        }
    }
}
