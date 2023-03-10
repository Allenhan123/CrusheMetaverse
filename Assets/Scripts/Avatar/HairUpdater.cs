using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NUWA.Character
{
    /// <summary>
    /// 头发更新
    /// </summary>
    public class HairUpdater : MeshUpdater
    {
        public HairColorUpdater hairColorUpdater;
        public override bool UpdateColor(Color color)
        {
            if (hairColorUpdater != null)
            {
                hairColorUpdater.UpdateColor(color);
                hairColorUpdater.UpdateEndColor(color);
            }
            return true;
        }
    }
}
