using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace NUWA.Character
{
    public class Character : MonoBehaviour
    {
        public int index;
        public CharacterAnimationPlayer animationPlayer;
        //public Transform characterCenter;

        private Color mSkinColor;
        //[SerializeField] protected SkinnedMeshRenderer body;
        //[SerializeField] public FeatureUpdater[] featureUpdaters;
        //public Texture2D BodyDefultTexture2D;

        public bool ready
        {
            get;
            private set;
        }

        // 玩家性别
        public Sex CharacterSex
        {
            get;
            set;
        }

        private DynamicBone[] _dynamicBoneArr;

        public static Character user;

        protected virtual void Awake()
        {
            _dynamicBoneArr = this.gameObject.GetComponentsInChildren<DynamicBone>();
        }

        // Use this for initialization
        protected virtual void Start()
        {
            return;
        }

        //public CharacterAnimationPlayer CreateAnimationPlayer()
        //{
        //    return Instantiate(animationPlayer);
        //}

        public void SetDynamicBoneEnable(bool bEnable)
        {
            if (_dynamicBoneArr != null)
            {
                if (bEnable)
                {
                    StartCoroutine(DelayDynamicBoneEnable(bEnable));
                }
                else
                {
                    SetDynamicBoneArrEnable(bEnable);
                }

            }
        }

        private IEnumerator DelayDynamicBoneEnable(bool bEnable)
        {
            yield return null;
            SetDynamicBoneArrEnable(bEnable);
        }

        private void SetDynamicBoneArrEnable(bool bEnable)
        {
            if (_dynamicBoneArr == null)
            {
                return;
            }

            for (int i = 0; i < _dynamicBoneArr.Length; i++)
            {
                if (_dynamicBoneArr[i] != null)
                {
                    _dynamicBoneArr[i].enabled = bEnable;
                }
            }
        }

        /// <summary>
        /// 设置位置
        /// </summary>
        public void SetLocalPosition(Vector3 localPos)
        {
            this.transform.localPosition = localPos;
        }

        public void SetActive(bool isActive)
        {
            this.gameObject.SetActive(isActive);
        }
    }
}

