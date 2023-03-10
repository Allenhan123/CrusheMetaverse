using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NUWA.Character
{
    public class MeshUpdater : FeatureUpdater
    {
        [SerializeField]
        protected bool updateMaterials = true;
        [SerializeField]
        protected string colorProperty = "_Color";
        protected GameObject lastGameObject;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        private void OnDestroy()
        {
            if (lastGameObject != null)
            {
                Destroy(lastGameObject);
            }
        }

        /// <summary>
        /// 更新Feature
        /// </summary>
        /// <param name="featureObj"></param>
        public override void UpdateFeature(Object featureObj, System.Action<bool> handleUpdateComplete = null)
        {
            if (featureObj == null)
            {
                Debug.LogError("MeshUpdater::UpdateFeature featureObj is empty");
                return;
            }

            if (lastGameObject != null)
            {
                lastGameObject.SetActive(false);
                DestroyImmediate(lastGameObject, true);
                lastGameObject = null;
            }

            StartCoroutine(DelayUpdateFeature(featureObj, handleUpdateComplete));
        }

        private IEnumerator DelayUpdateFeature(Object featureObj, System.Action<bool> handleUpdateComplete = null)
        {
            yield return null;
            Debug.LogFormat("MeshUpdater::DelayUpdateFeature name => {0} featureType => {1}", featureObj.name, featureType.ToString());

            GameObject skinAsset = featureObj as GameObject;
            GameObject skinGo = null;
            if (skinAsset != null)
            {
                skinGo = GameObject.Instantiate<GameObject>(skinAsset);
                skinGo.SetActive(false);
            }

            if (skinGo == null)
            {
                Debug.LogErrorFormat("MeshUpdater::UpdateFeature skinGo is null");
                yield break;
            }

            lastGameObject = skinGo;
            //var newMesh = skinGo.GetComponent<Renderer>(); //旧代码
            var newMesh = skinGo.GetComponentInChildren<Renderer>();
            if (newMesh == null)
            {
                Debug.LogErrorFormat("MeshUpdater::UpdateFeature get mesh error new mesh is null");
                yield break;
            }

            SkinnedMeshRenderer newSkinMeshRender = newMesh as SkinnedMeshRenderer;
            SkinnedMeshRenderer skinMeshRender = mMeshRenderer as SkinnedMeshRenderer;

            if (newSkinMeshRender == null || skinMeshRender == null)
            {
                Debug.LogErrorFormat("MeshUpdater::UpdateFeature SkinMeshRender is null");
                yield break;
            }
            skinMeshRender.sharedMesh = newSkinMeshRender.sharedMesh;
            //if (updateMaterials)
            //{
            //    skinMeshRender.sharedMaterials = updateAllMaterrials(newMesh.sharedMaterials);
            //}
            //else
            //{
            //    skinMeshRender.sharedMaterials = updateAllMaterrials(skinMeshRender.sharedMaterials);
            //}

            if (handleUpdateComplete != null)
            {
                handleUpdateComplete(true);
            }

        }

        protected Material[] updateAllMaterrials(Material[] mats)
        {
            Material[] newMats = new Material[mats.Length];
            int i = 0;
            foreach (Material mat in mats)
            {
                if (mat != null)
                {
                    Material newM = new Material(mat);
                    //强制读取本地shader
                    string oldShaderName = newM.shader.name;
                    newM.shader = Shader.Find(oldShaderName);
                    newMats[i] = newM;
                }
                i++;
            }
            return newMats;

        }

        public override bool UpdateColor(Color color)
        {
            Debug.LogFormat("MeshUpdater::UpdateColor color => {0} featureType => {1}", color, featureType.ToString());
            if (mMeshRenderer == null || mMeshRenderer.sharedMaterials == null || mMeshRenderer.sharedMaterials.Length == 0)
            {
                return false;
            }

            foreach (var mat in mMeshRenderer.sharedMaterials)
            {
                if (mat != null)
                {
                    mat.SetColor(colorProperty, color);
                }

            }
            return true;
        }
    }
}

