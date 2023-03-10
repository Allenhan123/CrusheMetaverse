using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NUWA.Character
{

	public enum Sex
	{
		female,
		male,
	}

	/// <summary>
	/// 角色数据
	/// </summary>
	public class CharacterData
	{

		public int gender; // 性别 1 男 2 女
		public int custom; // 自定义数据
		public List<FeatureData> feature; // feature 集合
		public List<FaceBlendShapeData> faceBlendShapeDatas; //脸部blendshape数据

		public CharacterData()
		{

		}

		public Dictionary<int, FeatureData> ToMap()
		{
			Dictionary<int, FeatureData> featureMap = new Dictionary<int, FeatureData>();

			for (int i = 0; i < feature.Count; i++)
			{
				if (feature[i] != null)
				{
					if (!featureMap.ContainsKey(feature[i].type))
					{
						featureMap.Add(feature[i].type, feature[i]);
					}
				}
			}
			return featureMap;
		}
	}

	/// <summary>
	/// feature 数据
	/// </summary>
	public class FeatureData
	{
		public int type;
		public string name = string.Empty;
		public string path = string.Empty;
		public string color = string.Empty;

		public FeatureData()
		{

		}
	}

    //脸部blendshapes数据 new
    public class FaceBlendShapeData
    {
        public int blendshapeIndex = 0;
        public string blendValue = "100f";
        public FaceBlendShapeData()
        {

        }
    }


    public enum FeatureType
	{
		skinTexture = 100, // 皮肤贴图
		skinColor = 101, // 皮肤颜色
		hair = 102, // 头发
		mouth = 103, // 嘴唇
		eyebrow = 104, // 眉毛
		eyebrowColor = 105, // 眉色
		ear = 106, // 耳朵
		eyeball = 107, // 眼球
		
		headTattoo = 113, // 脸部纹身
		hairColor = 114,  // 头发颜色
		bodyTattoo = 115, // 纹身

		//suit = 301, // 套装
		//top = 302, // 上衣
		//bottom = 303, // 裤子
		//socks = 304, // 袜子
		//shoe = 305, // 鞋子
	}
}