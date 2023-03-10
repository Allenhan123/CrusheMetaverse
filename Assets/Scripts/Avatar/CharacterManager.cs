using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using QFramework;
using QFrame;
using Utils;

namespace NUWA.Character
{
    public class CharacterManager : MonoSingleton<CharacterManager>
    {

        public GameObject malePrefab;
        public GameObject femalePrefab;
        private List<Character> _characterLst = null;

        void Start()
        {

        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="character"></param>
        public void AddCharacter(Character character)
        {
            if (null == _characterLst)
            {
                _characterLst = new List<Character>();
            }
            _characterLst.Add(character);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="character"></param>
        public void RemoveCharacter(Character character)
        {
            _characterLst.Remove(character);
        }

        /// <summary>
        /// 清理角色
        /// </summary>
        public void ClearCharacters()
        {
            if (_characterLst == null || _characterLst.Count <= 0)
            {
                return;
            }
            for (int i = 0; i < _characterLst.Count; i++)
            {
                if (_characterLst[i] != null)
                {
                    _characterLst[i].gameObject.SetActive(false);
                    Destroy(_characterLst[i].gameObject);
                }
            }
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="characterjsonPath"></param>
        /// <param name="handAssembleCharacterFinish"></param>
        public void AssembleCharacter(string characterjsonPath, System.Action<Character> handAssembleCharacterFinish = null)
        {
            StartCoroutine(CreateCharacter(characterjsonPath, handAssembleCharacterFinish));
        }

        IEnumerator CreateCharacter(string characterjsonPath, System.Action<Character> handAssembleCharacterFinish = null)
        {
            if (string.IsNullOrEmpty(characterjsonPath))
            {
                Debug.LogError("CharacterManager::AssembleCharacter param characterjsonPath error");
                yield break;
            }
            string characterJsonStr = FileUtility.ReadText(characterjsonPath);
            if (string.IsNullOrEmpty(characterJsonStr))
            {
                Debug.LogError("CharacterManager::AssembleCharacter characterJsonStr is empty");
                yield break;
            }

            Debug.LogFormat("CharacterManager::AssembleCharacter jsonStr => {0}", characterJsonStr);
            CharacterData characterData = Json.ToObject<CharacterData>(characterJsonStr);
            if (characterData == null)
            {
                Debug.LogError("CharacterManager::AssembleCharacter characterData is null");
                yield break;
            }

            Sex sex = characterData.gender == 1 ? Sex.male : Sex.female;
            Character character = null;
            character = CreateCharacter(sex);
            if (character == null)
            {
                Debug.LogError("CharacterManager::AssembleCharacter Create Character is null");
                yield break;
            }
            //character.SetLocalPosition(Vector3.one * 10000);
            character.SetDynamicBoneEnable(false);
            AssembleCharacter(character, characterData, handAssembleCharacterFinish);
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="sex"></param>
        /// <returns></returns>
        public Character CreateCharacter(Sex sex)
        {
            GameObject characterGo = null;
            if (sex == Sex.male)
            {
                characterGo = Instantiate<GameObject>(malePrefab);
            }
            else
            {
                characterGo = Instantiate<GameObject>(femalePrefab);
            }

            if (characterGo == null)
            {
                Debug.LogError("CharacterManager::CreateCharacter characterGo is error");
                return null;
            }

            Character character = characterGo.GetComponent<Character>();
            character.CharacterSex = sex;
            return character;
        }

        /// 组装角色
        /// <param name="character"></param>
        /// <param name="characterData"></param>
        /// <param name="handAssembleCharacterFinish"></param>
        public void AssembleCharacter(Character character, CharacterData characterData, System.Action<Character> handAssembleCharacterFinish = null)
        {
            StartCoroutine(StartAssembleCharacter(character, characterData, handAssembleCharacterFinish));
        }

        public IEnumerator StartAssembleCharacter(Character character, CharacterData characterData, System.Action<Character> handleAssembleCharacter = null)
        {
            yield return null;
            if (character == null)
            {
                Debug.LogError("updateCharacterFeature param character is null");
                yield break;
            }
            if (characterData == null)
            {
                Debug.LogError("updateCharacterFeature param characterData is null");
                yield break;
            }

            List<FeatureData> featureDataLst = characterData.feature;
            if (featureDataLst == null)
            {
                Debug.LogError("updateCharacterFeature featureDataLst is null");
                yield break;
            }

            Dictionary<int, FeatureData> characterDic = characterData.ToMap();
            foreach (FeatureType featureType in System.Enum.GetValues(typeof(FeatureType)))
            {
                FeatureData featureData = characterDic.GetMapValue((int)featureType);
                if (featureData != null)
                {
                    UpdateCharacterSkin(character, featureData);
                }
            }

            //TODO 设置脸部blendshape值
            FaceBlendShapes faceBlendShapes = character.gameObject.GetComponent<FaceBlendShapes>();
            if (faceBlendShapes != null)
            {
                List<FaceBlendShapeData> blendShapeDatas = characterData.faceBlendShapeDatas;
                if (blendShapeDatas == null)
                {
                    Debug.LogError("updateCharacterFeature blendShapeDatas is null");
                    yield break;
                }
                List<int> blendIndex = new List<int>();
                blendIndex.Clear();
                for (int i = 0; i < blendShapeDatas.Count; i++)
                {
                    blendIndex.Add(blendShapeDatas[i].blendshapeIndex);
                }
                faceBlendShapes.SetFaceBlendshape(blendIndex, 1);
            }

            if (handleAssembleCharacter != null)
            {
                yield return null;
                handleAssembleCharacter(character);
            }
        }



        /// <summary>
        /// 更新Skin
        /// </summary>
        /// <param name="character"></param>
        /// <param name="featureData"></param>
        public void UpdateCharacterSkin(Character character, FeatureData featureData)
        {
            Debug.LogError("11111111111  start UpdateCharacterFeature" + featureData.type.ToString());
            UpdateCharacterFeature(character, featureData, (isSuc) =>
            {
                if (isSuc)
                {
                    Debug.LogError("2222222222222  start UpdateCharacterFeatureColor" + featureData.type.ToString());

                    UpdateCharacterFeatureColor(character, featureData);
                }
            });
            //StartCoroutine(UpdaterMeshRender(character));
        }

        private IEnumerator UpdaterMeshRender(Character character)
        {
            yield return null;
            character.gameObject.UpdateMeshRenders();
        }

        /// <summary>
        /// 更新角色feature
        /// </summary>
        /// <param name="character"></param>
        /// <param name="featureData"></param>
        public void UpdateCharacterFeature(Character character, FeatureData featureData, System.Action<bool> handleUpdateComplete = null)
        {
            if (null == character)
            {
                Debug.LogError("updateCharacterFeature param character is null");
                return;
            }

            if (null == featureData)
            {
                Debug.LogError("updateCharacterFeature param featuredata is null");
                return;
            }

            AssembleCharacter assembleCharacter = character.gameObject.GetOrAddComponent<AssembleCharacter>();
            string abName = string.Empty;
            string assetName = string.Empty;

            FeatureType featureType = (FeatureType)(System.Enum.Parse(typeof(FeatureType), featureData.type.ToString()));
            FeatureUpdater featureUpdater = assembleCharacter.GetFeatureUpdater(featureType);
            Debug.LogFormat("updateCharacterFeature FeatureType {0}", featureType);
            if (featureUpdater == null)
            {
                Debug.LogFormat("updateCharacterFeature featureUpdater is null");
                return;
            }
            if (featureType == FeatureType.skinColor || featureType == FeatureType.hairColor || featureType == FeatureType.eyebrowColor)
            {
                featureUpdater.UpdateFeature(null, handleUpdateComplete);
                return;
            }else
            {
                if (string.IsNullOrEmpty(featureData.name))
                {
                    Debug.LogWarningFormat("updateCharacterFeature assetName is null");
                    return;
                }

                assetName = featureData.name;
                abName = QFrame.Path.GetCombinePath(featureData.path, assetName);

                if (string.IsNullOrEmpty(abName))
                {
                    Debug.LogErrorFormat("updateCharacterFeature abName is null => {0}", featureType);
                    return;
                }

                Debug.LogFormat("updateCharacterFeature abName => {0}, assetName => {1}, featureType => {2}", abName, assetName, featureType);
                UnityEngine.Object skinObj = QFrame.ResourceManager.Instance.LoadAsset<UnityEngine.Object>(abName, assetName);
                if (skinObj == null)
                {
                    Debug.LogErrorFormat("updateCharacterFeature skinObj is null => {0}", featureType);
                    return;
                }
                assembleCharacter.UpdateFeature(featureUpdater, skinObj, handleUpdateComplete);
                if (featureType == FeatureType.skinTexture)//更改皮肤贴图时需要把耳朵颜色改成白色(身体颜色在json中写死)
                {
                    FeatureUpdater earFeatureUpdater = assembleCharacter.GetFeatureUpdater(FeatureType.ear);
                    assembleCharacter.UpdateFeatureColor(earFeatureUpdater, Color.white);
                }
            }
        }

        /// <summary>
        /// 更新角色feature 颜色
        /// </summary>
        /// <param name="character"></param>
        /// <param name="featureData"></param>
        public void UpdateCharacterFeatureColor(Character character, FeatureData featureData)
        {
            if (character == null)
            {
                Debug.LogError("UpdateCharacterFeatureColor param character is null");
                return;
            }
            if (featureData == null)
            {
                Debug.LogError("UpdateCharacterFeatureColor param featuredata is null");
                return;
            }

            AssembleCharacter assembleCharacter = character.gameObject.GetOrAddComponent<AssembleCharacter>();
            FeatureType featureType = (FeatureType)(System.Enum.Parse(typeof(FeatureType), featureData.type.ToString()));
            FeatureUpdater featureUpdater = assembleCharacter.GetFeatureUpdater(featureType);
            Debug.LogFormat("UpdateCharacterFeatureColor featureType => {0} ---colorData => {1}", featureType, featureData.color);
            if (featureUpdater == null)
            {
                Debug.LogFormat("UpdateCharacterFeatureColor featureUpdater is null");
                return;
            }

            if (featureType == FeatureType.skinColor)
            {
                if (string.IsNullOrEmpty(featureData.color))
                {
                    Debug.LogError("UpdateCharacterFeatureColor skinColor color is null");
                    return;
                }

                Color color = NUWAUtils.Instance.HSB2RGB(featureData.color);
                FeatureUpdater earFeatureUpdater = assembleCharacter.GetFeatureUpdater(FeatureType.ear);
                //FeatureUpdater headFeatureUpdater = assembleCharacter.GetFeatureUpdater(FeatureType.head);
                //FeatureUpdater eyeShapefeatureUpdater = assembleCharacter.GetFeatureUpdater(FeatureType.eyeshape);

                assembleCharacter.UpdateFeatureColor(featureUpdater, color);
                //assembleCharacter.UpdateFeatureColor(headFeatureUpdater, color);
                assembleCharacter.UpdateFeatureColor(earFeatureUpdater, color);
            }
            else if (featureType == FeatureType.ear) //如果是耳朵那么材质颜色应保持不变即肤色(防止json中的耳朵颜色和皮肤颜色不一致)
            {
            }
            //头发渐变色
            else if (featureType == FeatureType.hair || featureType == FeatureType.hairColor)
            {
                if (string.IsNullOrEmpty(featureData.color))
                {
                    Debug.LogError("UpdateCharacterFeatureColor hariColor color is null");
                    return;
                }

                string hsbColorStr = featureData.color;
                Color startColor = Color.white;
                Color endColor = Color.white;
                MatchCollection matchCollect = Regex.Matches(hsbColorStr, "HSB\\((.*?)\\)");
                if (matchCollect != null && matchCollect.Count > 0)
                {
                    if (matchCollect.Count == 1)
                    {
                        if (!string.IsNullOrEmpty(matchCollect[0].Value))
                        {
                            Debug.LogWarning("color:" + matchCollect[0].Value);
                            startColor = NUWAUtils.Instance.HSB2RGB(matchCollect[0].Value);
                            endColor = startColor;
                        }
                    }
                    else if (matchCollect.Count == 2)
                    {
                        if (!string.IsNullOrEmpty(matchCollect[0].Value))
                        {
                            startColor = NUWAUtils.Instance.HSB2RGB(matchCollect[0].Value);
                        }

                        if (!string.IsNullOrEmpty(matchCollect[1].Value))
                        {
                            endColor = NUWAUtils.Instance.HSB2RGB(matchCollect[1].Value);
                        }
                    }
                }

                Debug.LogFormat("haircolor => {0} {1}", startColor, endColor);
                HairColorUpdater hairColorUpdater = assembleCharacter.GetFeatureUpdater(FeatureType.hairColor) as HairColorUpdater;
                if (hairColorUpdater != null)
                {
                    hairColorUpdater.UpdateColor(startColor);
                    hairColorUpdater.UpdateEndColor(endColor);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(featureData.color))
                {
                    Debug.LogError("UpdateCharacterFeatureColor color is null");
                    return;
                }
                Color color = NUWAUtils.Instance.HSB2RGB(featureData.color);
                assembleCharacter.UpdateFeatureColor(featureUpdater, color);
            }
        }
    }
}

