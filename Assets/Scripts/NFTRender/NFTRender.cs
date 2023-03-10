using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System.IO;
using Utils;

namespace NUWA.Character
{
    public class NFTRender : MonoSingleton<NFTRender>
    {

        // Record settings
        const int FRAME_RATE = 15;
        const int TEXTURE_WIDTH = 2048;
        const int TEXTURE_HEIGHT = 2048;
        string mNFTRootPath;
        string mCurrentGifPath;

        int mFrameCount;
        bool mIsRecording;
        private bool mGifIsGeneratoring;

        GifScenario mRecordingScenario;
        private Character _character;

        private string currGifName;
        private string currSaveGifFolder;
        private GameObject mGo;


        void Awake()
        {
            mNFTRootPath =
#if UNITY_EDITOR
            Application.dataPath + "/../Backup/NFT/";
#else
            Application.persistentDataPath + "/NFT/";
#endif
        }


        // Start is called before the first frame update
        void Start()
        {
            mGifIsGeneratoring = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                currGifName = "nft_pose";
                MargeGifFromNative("nft_pose");
            }
        }


        public void MargeGifFromNative(string path)
        {
#if !UNITY_EDITOR
            //string gifBundlePath = HomeNative._getCurrentGifBundlePath();
            //HomeNative._margeGifFromNativeReceived(gifBundlePath);
#endif
            if (mGifIsGeneratoring)
            {

#if !UNITY_EDITOR
                //HomeNative._margeGifFromNativeError("MargeGifFromNative Gif Rending now ..");
#endif
                return;
            }
            StopRecording();
            mGifIsGeneratoring = true;

            Debug.LogWarning("开始合成单人gif");
            StartCoroutine(myRecordGIF(path));
        }

        IEnumerator myRecordGIF(string GifName)
        {
            if (_character == null)
            {
                string jsonPath = NUWAUtils.GetSaveFilePath() + "/Character.json";
                Debug.Log("NFTRender start creat nft avatar");
                CharacterManager.Instance.AssembleCharacter(jsonPath, (_char) =>
                {
                    _character = _char;
                    StartCoroutine(DelayToHanderAssembleSelfAction());
                });
            }
            else
            {
                yield return null;
                StartCoroutine(DelayToHanderAssembleSelfAction());
            }

            while (mIsRecording)
            {
                yield return null;
            }

        }


        IEnumerator DelayToHanderAssembleSelfAction()
        {

            yield return new WaitForSeconds(0.1f);
            HanderAssembleSelfAction();
        }

        private void HanderAssembleSelfAction()
        {

#if UNITY_EDITOR
            string gifBundlePath = NUWAUtils.GetSaveFilePath() + currGifName;
            currSaveGifFolder = NUWAUtils.GetSaveFilePath() + "nft/" + currGifName;
            int frame_rate = FRAME_RATE;
            int texture_width = TEXTURE_WIDTH;
            int texture_height = TEXTURE_HEIGHT;

#else

#endif
            if (!Directory.Exists(currSaveGifFolder))
            {
                Directory.CreateDirectory(currSaveGifFolder);
            }
            GameObject obj = QFrame.ResourceManager.Instance.LoadAsset<GameObject>(gifBundlePath, currGifName);
            if (obj == null)
            {
                Debug.LogError("NFTRender error  avatar object is null");
                return;
            }
            if (mGo != null)
            {
                Destroy(mGo);
                mGo = null;
            }
            GameObject go = Instantiate(obj) as GameObject;
            mGo = go;
            mRecordingScenario = go.GetComponent<GifScenario>();
            if (mRecordingScenario == null)
            {
                Debug.LogError("NFTRender error  mRecordingScenario is null");
                mIsRecording = false;
                return;
            }

            if (mRecordingScenario.recordCamera == null)
            {
                Debug.LogError("NFTRender error  recordCamera is null");
                mIsRecording = false;
                return;
            }
            var cameraRecorder = mRecordingScenario.recordCamera.GetComponent<CameraRecorder>();
            if (cameraRecorder == null)
            {
                Debug.LogError("NFTRender error  CameraRecorder is null");
                mIsRecording = false;
                return;
            }

            mRecordingScenario.recordCamera.nearClipPlane = 0.55f;
            mRecordingScenario.recordCamera.farClipPlane = 1000;
            if (mRecordingScenario.recordCamera.usePhysicalProperties)
            {
                mRecordingScenario.recordCamera.usePhysicalProperties = false;
            }

            mFrameCount = 0;
            mRecordingScenario.transform.position = new Vector3(0, 0, 0);
            cameraRecorder.Init(texture_width, texture_height);

            _character.gameObject.SetActive(true);
            _character.transform.SetParent(mRecordingScenario.gameObject.transform, false);
            //mRecordingScenario.animations[0].parent.position = new Vector3(10000, -10000, 10000);
            _character.transform.localPosition = Vector3.zero;
            _character.animationPlayer.PlayFrameByFrame(mRecordingScenario, MySaveCurrentFrame, frame_rate);
        }

        void MySaveCurrentFrame(bool hasNextFrame)
        {
            // Save to file
            //#if !UNITY_EDITOR
            var cameraRecorder = mRecordingScenario.recordCamera.GetComponent<CameraRecorder>();
            byte[] bytes = cameraRecorder.GetEncodedPNG();
            string filePathName = currSaveGifFolder + "/" + mFrameCount.ToString() + ".png";
            FileUtility.WriteBytes(filePathName, bytes);
            //#endif
            mFrameCount++;
            if (!hasNextFrame)
            {
                _character.transform.SetParent(null, false);
                _character.gameObject.SetActive(false);
                StopRecording();
                mGifIsGeneratoring = false;

            }
        }



        public void StopRecording()
        {
            Debug.LogWarning("Stop recording...");
            mGifIsGeneratoring = false;
            if (_character != null)
            {
                _character.animationPlayer.StopPlayingFrameByFrame();
            }

            if (mGo)
            {
                GameObject.Destroy(mGo);
                mGo = null;
            }
            StopAllCoroutines();
            UnityEngine.Resources.UnloadUnusedAssets();
        }
    }
}
