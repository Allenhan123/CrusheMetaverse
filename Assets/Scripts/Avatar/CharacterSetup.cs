using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFrame;
using System;
using Utils;

namespace NUWA.Character
{
    public class CharacterSetup : MonoBehaviour
    {
		public Character _selfCharacter;
		public Transform _characterRootRf;
		// Start is called before the first frame update
		void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

		//获取avatar Json信息
		public void InitCharacterJson()
        {
			string jsonPath = string.Empty;
			jsonPath = NUWAUtils.GetSaveFilePath() + "/Character.json";
			Debug.LogFormat("CharacterSetup::Start {0}", jsonPath);
			InitCharacter(jsonPath, HandleAssembleFinish);
		}

		//用json开始组装avatar
		private void InitCharacter(string jsonPath, System.Action<Character> handleAssembleFinish = null)
		{
			if (string.IsNullOrEmpty(jsonPath))
			{
				Debug.LogError("CharacterSetup::InitCharacter param jsonPath is null");
				return;
			}
			CharacterManager.Instance.AssembleCharacter(jsonPath, HandleAssembleFinish);
		}

		//TODO组装完成开始渲染NFT
		private void HandleAssembleFinish(Character character)
		{
			Debug.Log("CharacterSetup::HandleAssembleFinish finish");
			_selfCharacter = character;
			_selfCharacter.transform.localScale = Vector3.one;
			//_selfCharacter.SetDynamicBoneEnable(true);
			_selfCharacter.transform.SetParent(_characterRootRf);
			_selfCharacter.transform.localPosition = Vector3.zero;
			_selfCharacter.transform.localEulerAngles = Vector3.zero;

			//StartCoroutine(RenderNFT());
        }

		//TODO 临时渲染方案
		int recordWidth = 2048;
		int recordHeight = 2048;
		RenderTexture renderTexture;
		Texture2D virtualPhoto;
		IEnumerator RenderNFT()
        {
			yield return new WaitForSeconds(0.1f);
			renderTexture = new RenderTexture(recordWidth, recordHeight, 24);
			virtualPhoto = new Texture2D(recordWidth, recordHeight, TextureFormat.RGBA32, true);

			Camera recordCamera = Camera.main;
			recordCamera.targetTexture = renderTexture;
			recordCamera.backgroundColor = new Color(0.23f, 0.25f, 0.29f, 0);
			recordCamera.Render();
			byte[] bytes = GetEncodedPNG();
			string filePathName = NUWAUtils.GetSaveFilePath() + "/" + "nft" + ".png";
			FileUtility.WriteBytes(filePathName, bytes);
			Debug.LogWarning("render nft finish");
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
			virtualPhoto.Apply();
			RenderTexture.active = null; //can help avoid errors 
			return virtualPhoto.EncodeToPNG();
		}


		/// 更改feature特征
		/// <param name="featureJson"></param>
		public void ChangeFeatureWithJson(string featureJson)
		{
			Debug.LogFormat("AvatarSetup::ChangeFeatureWithJson param featureJson {0}", featureJson);
			if (string.IsNullOrEmpty(featureJson))
			{
				Debug.LogError("AvatarSetup::ChangeFeatureWithJson param featureJson is null");
				return;
			}
			FeatureData featureData = Json.ToObject<FeatureData>(featureJson);
			CharacterManager.Instance.UpdateCharacterSkin(_selfCharacter, featureData);
		}
	}
}

