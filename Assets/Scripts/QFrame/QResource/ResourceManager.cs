using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace QFrame
{
    [DisallowMultipleComponent]
    public sealed class ResourceManager : MonoSingleton<ResourceManager>
    {
        private static bool _isInit = false;

        // ab 缓存
        private static Dictionary<string, AssetBundle> _assetBundleCache = new Dictionary<string, AssetBundle>();

        // 正在加载得ab 列表
        private static List<string> _loadingAssetBundles = new List<string>();

        void Awake()
        {
            Initialize();
        }

       

        private static void Initialize()
        {
            if (_isInit)
            {
                return;
            }

            _isInit = true;
        }

        /// <summary>
        /// 添加ab 到缓存
        /// </summary>
        /// <param name="abName">bundle名</param>
        /// <param name="ab">bundle</param>
        private void AddAssetBundle(string abName, AssetBundle ab)
        {
            if (!_assetBundleCache.ContainsKey(abName))
            {
                _assetBundleCache.Add(abName, ab);
            }

        }

        /// <summary>
        /// 从缓存中获得ab
        /// </summary>
        /// <param name="abName">bundle名</param>
        private AssetBundle GetAssetBundleFromCache(string abName)
        {
            AssetBundle ab = null;

            if (_assetBundleCache.TryGetValue(abName, out ab))
            {
                if (null == ab)
                {
                    if (HasAssetBundle(abName))
                    {
                        RemoveAssetBundle(abName);
                    }
                }
            }

            return ab;
        }

        /// <summary>
        /// 从缓存中删除ab
        /// </summary>
        /// <param name="abName"></param>
        private void RemoveAssetBundle(string abName)
        {
            if (_assetBundleCache.ContainsKey(abName))
            {
                _assetBundleCache.Remove(abName);
            }
        }

        /// <summary>
        /// 缓存中是否有ab
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        private bool HasAssetBundle(string abName)
        {
            return _assetBundleCache.ContainsKey(abName);
        }

        /// <summary>
        /// 同步加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="abName">bundle名</param>
        /// <param name="assetName">资源名</param>
        /// <returns></returns>
        public T LoadAsset<T>(string abName, string assetName)where T : UnityEngine.Object
        {
            if (!_isInit)
            {
                throw new SystemException("ResourceManager not Initialize");
            }

            if (string.IsNullOrEmpty(abName))
            {
                Debug.LogFormat("ResourceManager::LoadAsset abName is empty");
                return null;
            }

            if (string.IsNullOrEmpty(assetName))
            {
                Debug.LogFormat("ResourceManager::LoadAsset assetName is empty");
                return null;
            }

            return LoadAssetImpl<T>(abName, assetName);
        }

        /// <summary>
        /// 同步加载资源的实现
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName">资源名</param>
        /// <returns></returns>
        private T LoadAssetImpl<T>(string abName, string assetName)where T : UnityEngine.Object
        {
            AssetBundle ab = LoadAssetBundleImpl(abName, true);
            if (ab == null)
            {
                Debug.LogErrorFormat("ResourceManager::LoadAssetImpl Load AssetBundle error => {0}", abName);
                return null;
            }

            return ab.LoadAsset<T>(assetName);
        }

        /// <summary>
        /// 同步加载ab的实现
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        private AssetBundle LoadAssetBundleImpl(string abName, bool isFirstCall)
        {
            string abNameKey = QFrame.Path.GetFileName(abName);
            AssetBundle ab = GetAssetBundleFromCache(abNameKey);

            try
            {
                if (ab == null)
                {
                    ab = AssetBundle.LoadFromFile(abName);

                    AddAssetBundle(abNameKey, ab);

                    //todo 递归加载依赖的 ab
                    //var dependencys = assetData.GetDependencyAssetNames();
                    //foreach (var d in dependencys)
                    //{
                    //    LoadAssetBundleImpl(d, false);
                    //}
                }

                if (isFirstCall)
                {
                    return ab;
                }

            }
            catch (Exception e)
            {
                //    Debug.LogErrorFormat("ResourceManager::LoadAssetBundleImpl Load AssetBundle error => {0}", e.ToString());
            }

            return ab;
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <param name="callback"></param>
        public void LoadAssetAsync<T>(string abName, string assetName, Action<T> callback)where T : UnityEngine.Object
        {
            if (!_isInit)
            {
                throw new SystemException("ResourceManager not Initialize");
            }

            StartCoroutine(LoadAssetAsyncImpl(abName, assetName, callback));
        }

        /// <summary>
        /// 异步加载资源实现
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="abName"></param>
        /// <param name="assetName"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        private IEnumerator LoadAssetAsyncImpl<T>(string abName, string assetName, Action<T> callback)where T : UnityEngine.Object
        {
            AssetBundle assetBundle = null;
            T result = null;

            yield return StartCoroutine(LoadAssetBundleAsyncImpl(abName, (ab) =>
            {
                assetBundle = ab;
            }, true));
            if (assetBundle == null)
            {
                if (callback != null)
                {
                    callback(result);
                }
                yield break;
            }

            AssetBundleRequest request = assetBundle.LoadAssetAsync<UnityEngine.Object>(assetName);
            yield return request;
            result = (T)request.asset;

            if (callback != null)
            {
                callback(result);
            }
        }

        /// <summary>
        /// 异步加载AssetBundle的实现
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        private IEnumerator LoadAssetBundleAsyncImpl(string abName, Action<AssetBundle> callback, bool isFirstCall)
        {
            string abNameKey = QFrame.Path.GetFileName(abName);
            while (_loadingAssetBundles.Contains(abNameKey))
            {
                yield return null;
            }

            AssetBundle ab = GetAssetBundleFromCache(abNameKey);
            if (ab == null)
            {
                _loadingAssetBundles.Add(abNameKey);
                AssetBundleCreateRequest abCreateRequest = AssetBundle.LoadFromFileAsync(abName);
                yield return abCreateRequest;
                _loadingAssetBundles.Remove(abNameKey);

                ab = abCreateRequest.assetBundle;

                if (ab == null)
                {
                    //   Debug.LogErrorFormat("ResourceManager::LoadAssetBundleImpl load error => {0}", abName);
                    yield break;
                }

                AddAssetBundle(abNameKey, ab);

                //todo 递归异步加载依赖的 ab
                //var dependencys = assetData.GetDependencyAssetNames();
                //foreach (var d in dependencys)
                //{
                //    yield return StartCoroutine(LoadAssetBundleAsyncImpl(d, null, false));
                //}
            }

            if (isFirstCall && callback != null)
            {
                callback(ab);
            }
        }

        /// <summary>
        /// 卸载ab
        /// </summary>
        /// <param name="abName"></param>
        public void UnLoadAssetBundle(string abName, bool unLoadAsset = false)
        {
            string abNameKey = QFrame.Path.GetFileName(abName);
            AssetBundle ab = GetAssetBundleFromCache(abNameKey);
            if (ab != null)
            {
                ab.Unload(unLoadAsset);

                RemoveAssetBundle(abName);
            }
        }

        /// <summary>
        /// 卸载资源 仅能释放非GameObject和Component的资源，比如Texture、Mesh等真正的资源
        /// </summary>
        /// <param name="asset"></param>
        public void UnloadAsset(UnityEngine.Object asset)
        {
            // 释放相应的资源
            Resources.UnloadAsset(asset);
        }

        /// <summary>
        /// 资源清理 卸载有加载过的ab
        /// </summary>
        /// <param name="unLoadAll">true 清理加载过的资源引用和bundle, false 只清理bundle</param>
        public void Clear(bool unLoadAll = false)
        {
            if (_assetBundleCache != null)
            {
                foreach (var item in _assetBundleCache)
                {
                    if (item.Value != null)
                    {
                        item.Value.Unload(unLoadAll);
                    }
                }
            }

            _assetBundleCache.Clear();
            AssetBundle.UnloadAllAssetBundles(unLoadAll);
        }
    }
}