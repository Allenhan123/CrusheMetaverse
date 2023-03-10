using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NUWA.Character
{
    public static class FaceBlendShapesUtils
    {
        private static List<int> blendIndexList = null;
        //根据性别(1男2女)获取随机生成的脸型blendshape index
        public static List<int> getFaceBlendShape(int sex)
        {
            blendIndexList = new List<int>();
            blendIndexList.Clear();
            if(sex == 1)
            {
                blendIndexList.Add(Random.Range(60, 65));//嘴型
                blendIndexList.Add(Random.Range(65, 70));//鼻子
                blendIndexList.Add(Random.Range(70, 75));//眼睛
                blendIndexList.Add(Random.Range(75, 78));//脸型
                
                
            }
            else
            {
                blendIndexList.Add(Random.Range(0, 4));
                blendIndexList.Add(Random.Range(4, 7));
                blendIndexList.Add(Random.Range(7, 12));
            }
            return blendIndexList;
        }

        public static float getBlendValue()
        {
            int a = Random.Range(0, 3);
            float blendValue = a > 0 ? 100f : 0f;
            return blendValue;
            //return 100f;
        }
    }
}
