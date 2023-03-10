using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using BestHTTP.JSON.LitJson;
using QFrame;
using UnityEngine;
using QFramework;

namespace NUWA.Character
{
    public class AssembleCharacter : MonoBehaviour
    {
        // 角色
        public Character character;

        // FeatureUpdate数组
        [SerializeField]
        public FeatureUpdater[] featureUpdaters;

        [SerializeField]
        private Sex _sex;

        private Dictionary<FeatureType, int> _featureSwichDic = null;

        private void Awake()
        {

        }

        /// <summary>
        /// 更新角色Feature
        /// </summary>
        public void UpdateFeature(FeatureUpdater featureUpdater, UnityEngine.Object skinObj, System.Action<bool> handleUpdateComplete = null)
        {
            if (null == featureUpdater)
            {
                return;
            }
            featureUpdater.UpdateFeature(skinObj, handleUpdateComplete);
        }

        /// <summary>
        /// 更新角色Feature Color
        /// </summary>
        public void UpdateFeatureColor(FeatureUpdater featureUpdater, Color color)
        {
            if (null == featureUpdater)
            {
                return;
            }
            featureUpdater.UpdateColor(color);
        }



        public FeatureUpdater GetFeatureUpdater(FeatureType featureType)
        {
            int featureIndex = GetCharacterFeatureIndex(featureType);
            if (featureIndex >= 0 && featureIndex < featureUpdaters.Length)
            {
                if (featureUpdaters[featureIndex] != null)
                {
                    return featureUpdaters[featureIndex];
                }
            }
            return null;
        }

        /// <summary>
        /// 获取feature 索引
        /// </summary>
        /// <param name="featureType">FeatureType</param>
        /// <returns></returns>
        public int GetCharacterFeatureIndex(FeatureType featureType)
        {
            if (_featureSwichDic == null)
            {
                _featureSwichDic = new Dictionary<FeatureType, int>();
                _featureSwichDic.Add(FeatureType.hairColor, 0);
                _featureSwichDic.Add(FeatureType.hair, 1);
                _featureSwichDic.Add(FeatureType.eyebrow, 2);
                _featureSwichDic.Add(FeatureType.eyebrowColor, 3);
                _featureSwichDic.Add(FeatureType.eyeball, 4);
                _featureSwichDic.Add(FeatureType.ear, 5);
                _featureSwichDic.Add(FeatureType.skinColor, 6);
                _featureSwichDic.Add(FeatureType.skinTexture, 7);
                _featureSwichDic.Add(FeatureType.mouth, 8);
                _featureSwichDic.Add(FeatureType.bodyTattoo, 9);
                _featureSwichDic.Add(FeatureType.headTattoo, 10);
            }

            if (_featureSwichDic.ContainsKey(featureType))
            {
                return _featureSwichDic[featureType];
            }
            return 0;
        }
    }
}

