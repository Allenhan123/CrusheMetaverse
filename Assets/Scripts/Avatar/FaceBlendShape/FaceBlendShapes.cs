using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NUWA.Character
{
    public class FaceBlendShapes : MonoBehaviour
    {
        public SkinnedMeshRenderer faceSkm;
        public SkinnedMeshRenderer eyelashesSkm;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void GenerateRandomFace()
        {
            List<int> faceBlend = FaceBlendShapesUtils.getFaceBlendShape(1);
            int blendCount = faceSkm.sharedMesh.blendShapeCount;
            int eyelashCount = eyelashesSkm.sharedMesh.blendShapeCount;
            Debug.LogWarning("BlendCount:" + blendCount);
            for (int i = 0; i < blendCount; i++)
            {
                faceSkm.SetBlendShapeWeight(i, 0);
            }
            for (int i = 0; i < eyelashCount; i++)
            {
                eyelashesSkm.SetBlendShapeWeight(i, 0);
            }

            float eyeBlendValue = 0f;
            for (int i = 0; i < faceBlend.Count; i++)
            {
                Debug.LogWarning("faceIndex:" + faceBlend[i]);
                float blendValue = FaceBlendShapesUtils.getBlendValue();
                if (i == 2)
                {
                    eyeBlendValue = blendValue;
                    Debug.LogWarning("eyeBlendValue:" + eyeBlendValue);
                }
                faceSkm.SetBlendShapeWeight(faceBlend[i], blendValue);
            }
            Debug.LogWarning("eyelashIndex:" + (faceBlend[2] - 10));
            eyelashesSkm.SetBlendShapeWeight(faceBlend[2] - 10, eyeBlendValue);
        }

        public void SetFaceBlendshape(List<int> faceBlend,int sex)
        {
            int earBlendIndex = 0;
            if (sex == 1)
            {
                earBlendIndex = faceBlend[2] - 10;
            }
            else
            {

            }
            
            int blendCount = faceSkm.sharedMesh.blendShapeCount;
            int eyelashCount = eyelashesSkm.sharedMesh.blendShapeCount;
            Debug.LogWarning("BlendCount:" + blendCount);
            for (int i = 0; i < blendCount; i++)
            {
                faceSkm.SetBlendShapeWeight(i, 0);
            }
            for (int i = 0; i < eyelashCount; i++)
            {
                eyelashesSkm.SetBlendShapeWeight(i, 0);
            }

            float eyeBlendValue = 0f;
            for (int i = 0; i < faceBlend.Count; i++)
            {
                Debug.LogWarning("faceIndex:" + faceBlend[i]);
                float blendValue = FaceBlendShapesUtils.getBlendValue();
                if (i == 2)
                {
                    eyeBlendValue = blendValue;
                }
                faceSkm.SetBlendShapeWeight(faceBlend[i], blendValue);
            }
            eyelashesSkm.SetBlendShapeWeight(earBlendIndex, eyeBlendValue);

            
        }
    }
}
