using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NUWA.Character
{
    public delegate void OnRecordFinished();

    [RequireComponent(typeof(Camera))]
    public class CameraRecorder : MonoBehaviour
    {

        int recordWidth;
        int recordHeight;

        RenderTexture renderTexture;
        Texture2D virtualPhoto;

        // Use this for initialization
        public void Init(int width, int height)
        {
            recordWidth = width;
            recordHeight = height;
            renderTexture = new RenderTexture(recordWidth, recordHeight, 24);
            virtualPhoto = new Texture2D(recordWidth, recordHeight, TextureFormat.RGBA32, false);

            var recordCamera = GetComponent<Camera>();
            recordCamera.targetTexture = renderTexture;
            recordCamera.backgroundColor = new Color(1, 1, 1, 0);
        }

        public byte[] GetEncodedPNG()
        {
            if (renderTexture == null)
            {
                Debug.LogError("CameraRecorder has not been initialized!");
                return null;
            }

            RenderTexture.active = renderTexture;
            virtualPhoto.ReadPixels(new Rect(0, 0, recordWidth, recordHeight), 0, 0);
            RenderTexture.active = null; //can help avoid errors 

            return virtualPhoto.EncodeToPNG();
        }

        void OnDestroy()
        {
            if (virtualPhoto != null)
            {
                DestroyImmediate(virtualPhoto);
                virtualPhoto = null;
                UnityEngine.Resources.UnloadUnusedAssets();
                System.GC.Collect();
            }
        }
    }
}
