using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFrame
{

	public static class ColorUtils
	{
		public static float[] RGB2HSB(int rgbR, int rgbG, int rgbB)
		{
			if (!(0 <= rgbR && rgbR <= 255) && (0 <= rgbG && rgbG <= 255) && (0 <= rgbB && rgbB <= 255))
			{
				return null;
			}

			int[] rgb = new int[]
			{
				rgbR,
				rgbG,
				rgbB
			};

			Array.Sort(rgb);

			int max = rgb[2];
			int min = rgb[0];

			float hsbB = max / 255.0f;
			float hsbS = max == 0 ? 0 : (max - min) / (float)max;

			float hsbH = 0;
			if (max == rgbR && rgbG >= rgbB)
			{
				hsbH = (rgbG - rgbB) * 60f / (max - min) + 0;
			}
			else if (max == rgbR && rgbG < rgbB)
			{
				hsbH = (rgbG - rgbB) * 60f / (max - min) + 360;
			}
			else if (max == rgbG)
			{
				hsbH = (rgbB - rgbR) * 60f / (max - min) + 120;
			}
			else if (max == rgbB)
			{
				hsbH = (rgbR - rgbG) * 60f / (max - min) + 240;
			}

			return new float[]
			{
				hsbH,
				hsbS,
				hsbB
			};
		}

		public static float[] HSB2RGB(float h, float s, float v)
		{
			if (!((h.CompareTo(0.0f) >= 0 && h.CompareTo(360.0f) <= 0) && (s.CompareTo(0.0f) >= 0 && s.CompareTo(1.0f) <= 0) && (v.CompareTo(0.0f) >= 0 && v.CompareTo(1.0f) <= 0)))
			{

				return null;
			}

			float r = 0, g = 0, b = 0;
			int i = (int)((h / 60) % 6);
			float f = (h / 60) - i;
			float p = v * (1 - s);
			float q = v * (1 - f * s);
			float t = v * (1 - (1 - f) * s);
			switch (i)
			{
				case 0:
					r = v;
					g = t;
					b = p;
					break;
				case 1:
					r = q;
					g = v;
					b = p;
					break;
				case 2:
					r = p;
					g = v;
					b = t;
					break;
				case 3:
					r = p;
					g = q;
					b = v;
					break;
				case 4:
					r = t;
					g = p;
					b = v;
					break;
				case 5:
					r = v;
					g = p;
					b = q;
					break;
				default:
					break;
			}
			return new float[]
			{
				r, g, b
			};
		}

	}
}