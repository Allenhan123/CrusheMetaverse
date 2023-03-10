using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NUWA.Character
{
    public class SkinTextureUpdater : TextureUpdater
    {
        public SkinColorUpdater skinColorUpdater;
        public override bool UpdateColor(Color color)
        {
            if (skinColorUpdater != null)
            {
                skinColorUpdater.UpdateColor(color);
            }
            return true;
        }
    }
}
