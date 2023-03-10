using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace NUWA.Character
{
    public class CharacterUIManager : MonoBehaviour
    {
        // Use this for initialization
        public CharacterSetup characterSetup;

        //需要更换的特征有头发 发色 脸部特征Blendshape(脸型 眼型 嘴形 鼻型) 眉毛 眉色 眼球 胡子 唇色 衣服
        //头发 皮肤 眉毛支持随着mesh一起更换颜色 也支持单独换色
        private string getColor()
        {
            string[] colors = new string[5] { "HSB(106.000, 44.000, 22.000, 1.000)", "HSB(46.000, 0.300, 0.400, 1.000)", "HSB(156.000, 0.600, 0.700, 1.000)", "HSB(46.000, 0.200, 0.600, 1.000)", "HSB(26.000, 0.500, 0.200, 1.000)" };
            int colorIndex = Random.Range(0, 4);
            string color = colors[colorIndex];
            return color;
        }

        /// 换发色
        public void ChangeHairColor()
        {
            Hashtable myHash = new Hashtable();//创建一个Hashtable实例
            myHash["type"] = 114;
            myHash["name"] = "";
            myHash["path"] = "";
            myHash["color"] = "HSB(106.000, 44.000, 22.000, 1.000),HSB(46.000, 0.300, 0.400, 1.000)";
            string jsonString = JsonConvert.SerializeObject(myHash);
            characterSetup.ChangeFeatureWithJson(jsonString);
        }


        /// 换肤色
        public void ChangeSkinColor()
        {
            Hashtable myHash = new Hashtable();//创建一个Hashtable实例
            myHash["type"] = 101;
            myHash["name"] = "";
            myHash["path"] = "";
            myHash["color"] = getColor();
            string jsonString = JsonConvert.SerializeObject(myHash);
            characterSetup.ChangeFeatureWithJson(jsonString);
        }

        /// 换眉色
        public void ChangeEyebrowColor()
        {
            Hashtable myHash = new Hashtable();//创建一个Hashtable实例
            myHash["type"] = 105;
            myHash["name"] = "";
            myHash["path"] = "";
            myHash["color"] = getColor();
            string jsonString = JsonConvert.SerializeObject(myHash);
            characterSetup.ChangeFeatureWithJson(jsonString);
        }



        public void ChangeSkinTexture()
        {
            string jsonData = @"{'type':100, 'name':'b_body_a002', path:'/Users/handongqiang/boo/NXGen/BooUniverse/CrusheMetaverse/Assets/backup/assets/male/body/', 'color':'HSB(0, 0, 100)'}";
            characterSetup.ChangeFeatureWithJson(jsonData);
        }


        /// 换眼球
        public void ChangeEyeball()
        {
            string jsonData = @"{'type':107, 'name':'b_eyes_a002', path:'/Users/handongqiang/boo/NXGen/BooUniverse/CrusheMetaverse/Assets/backup/assets/male/eyeball/', 'color':'HSB(0, 0, 0)'}";
            characterSetup.ChangeFeatureWithJson(jsonData);
        }


        /// 换头发
        public void ChangeHair()
        {
            string jsonData = @"{'type':102, 'name':'b_hair_a002', path:'/Users/handongqiang/boo/NXGen/BooUniverse/CrusheMetaverse/Assets/backup/assets/male/haircut/', 'color':'HSB(136.000, 0.100, 0.230, 1.000),HSB(46.000, 0.300, 0.400, 1.000)'}";
            characterSetup.ChangeFeatureWithJson(jsonData);
        }


        //随机生成blendshape脸型
        public void ChangeFace()
        {
            FaceBlendShapes faceBlendShapes = characterSetup.gameObject.GetComponentInChildren<FaceBlendShapes>();
            if(faceBlendShapes != null)
            {
                faceBlendShapes.GenerateRandomFace();
            }
        }


        /// 换眉毛
        public void ChangeEyeBrow()
        {
            string jsonData = @"{'type':104, 'name':'b_eyebrow_a003', path:'/Users/handongqiang/boo/NXGen/BooUniverse/CrusheMetaverse/Assets/backup/assets/male/eyebrow/', 'color':'HSB(56.000, 0.340, 0.700, 1.000))'}";
            characterSetup.ChangeFeatureWithJson(jsonData);
        }


        /// 换耳朵
        public void ChangeEar()
        {
            string jsonData = @"{'type':106, 'name':'b_ears_a002', path:'/Users/handongqiang/boo/NXGen/BooUniverse/CrusheMetaverse/Assets/backup/assets/male/ear/', 'color':'HSB(0, 0, 0, 1.000))'}";
            characterSetup.ChangeFeatureWithJson(jsonData);
        }

        /// 换嘴唇
        public void ChangeMouth()
        {
            string jsonData = @"{'type':103, 'name':'b_mouth_a001', path:'/Users/handongqiang/boo/NXGen/BooUniverse/CrusheMetaverse/Assets/backup/assets/male/mouth/', 'color':'HSB(0, 0, 0, 1.000))'}";
            characterSetup.ChangeFeatureWithJson(jsonData);
        }

        /// 换纹身
        public void ChangeBodyTattoo()
        {
            string jsonData = @"{'type':115, 'name':'b_bodytattoo_001', path:'/Users/handongqiang/boo/NXGen/BooUniverse/CrusheMetaverse/Assets/backup/assets/male/bodytattoo/', 'color':'HSB(0, 0, 0, 1.000))'}";
            characterSetup.ChangeFeatureWithJson(jsonData);
        }


        /// 换脸纹
        public void ChangeFaceTattoo()
        {
            string jsonData = @"{'type':113, 'name':'b_ear_a003', path:'/Users/handongqiang/boo/NXGen/BooUniverse/CrusheMetaverse/Assets/backup/assets/male/ear/', 'color':'HSB(0, 0, 0, 1.000))'}";
            characterSetup.ChangeFeatureWithJson(jsonData);
        }


        /// 生成角色json
        public void GenerateJson()
        {

        }

        /// 加载角色json
        public void InitCharacterWithJson()
        {

        }
    }
}

