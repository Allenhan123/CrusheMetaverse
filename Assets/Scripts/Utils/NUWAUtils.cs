using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using QFramework;

namespace NUWA.Character
{
    public class NUWAUtils : Singleton<NUWAUtils>
    {

        private NUWAUtils() { }
        public Color StringTOColol(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return Color.white;
            }

            Color mcolor = Color.white;
            string colorstr = "";

            colorstr = str.Replace("RGBA(", "");
            colorstr = colorstr.Replace(")", "");

            string[] colorstrS = colorstr.Split(',');

            float R = Convert.ToSingle(colorstrS[0]);
            float G = Convert.ToSingle(colorstrS[1]);
            float B = Convert.ToSingle(colorstrS[2]);
            float A = Convert.ToSingle(colorstrS[3]);

            mcolor = new Color(R, G, B, A);

            return mcolor;
        }

        public Color HSB2RGB(string hsbStr)
        {
            Debug.LogWarning("color:" + hsbStr);
            if (string.IsNullOrEmpty(hsbStr))
            {
                return Color.white;
            }

            Color mcolor = Color.white;
            string colorstr = "";

            hsbStr = hsbStr.Replace("HSB(", "");
            hsbStr = hsbStr.Replace(")", "");

            string[] colorstrS = hsbStr.Split(',');

            float h = Convert.ToSingle(colorstrS[0]);
            float s = Convert.ToSingle(colorstrS[1]);
            float b = Convert.ToSingle(colorstrS[2]);
            float a = 1;

            float[] rgbArr = QFrame.ColorUtils.HSB2RGB(h, s, b);

            if (rgbArr != null)
            {
                mcolor = new Color(rgbArr[0], rgbArr[1], rgbArr[2], a);
            }

            return mcolor;
        }

        public static string GetSaveFilePath()
        {
            string path = "";
#if UNITY_EDITOR
            path = Application.dataPath + "/backup/";
#else
            path = Application.dataPath;
#endif

            return path;
        }
    }
}

