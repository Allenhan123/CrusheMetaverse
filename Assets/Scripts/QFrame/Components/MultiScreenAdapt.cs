///<summary>
/// ******************************************************************************
/// ** 描述：适配脚本
/// ** 作者：Chen.Qiang
/// ** 历史: 
/// **       1.0   | Mr.Chen | 2017-04-01 | 初始
/// ******************************************************************************
///</summary> 


using UnityEngine;
using UnityEngine.UI;

namespace XFramework.UI
{
    [RequireComponent(typeof(CanvasScaler))]
    public class MultiScreenAdapt : MonoBehaviour
    {
        public Camera guiCamera;

        public float designWidth;
        public float designHeight;
        private CanvasScaler canvasScaler;

        protected void Awake()
        {
            canvasScaler = gameObject.GetOrAddComponent<CanvasScaler>();
        }

        protected void Start()
        {
            if (canvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
            {
                designWidth = canvasScaler.referenceResolution.x;
                designHeight = canvasScaler.referenceResolution.y;
                Adapt();
            }
        }

        private void Adapt()
        {
            //获取设备宽高    
            float deviceWidth = Screen.width;
            float deviceHeight = Screen.height;

            float designRatio = designWidth / designHeight;
            float deviceRatio = deviceWidth / deviceHeight;

            if (deviceRatio < designRatio)
            {
                // 匹配宽
                canvasScaler.matchWidthOrHeight = 0;
                float adjustRatio = designRatio / deviceRatio;
                transform.localScale = new Vector3(1, adjustRatio, 1);

                if (guiCamera != null)
                {
                    guiCamera.gameObject.transform.localScale = new Vector3(1, adjustRatio, 1);
                }
            }
            else
            {
                // 匹配高
                canvasScaler.matchWidthOrHeight = 1;
                float adjustRatio = deviceRatio / designRatio;
                transform.localScale = new Vector3(adjustRatio, 1, 1);

                if (guiCamera != null)
                {
                    guiCamera.gameObject.transform.localScale = new Vector3(adjustRatio, 1, 1);
                }
            }
        }
    }
}