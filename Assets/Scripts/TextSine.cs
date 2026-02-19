using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSine : MonoBehaviour
{
    public TMP_Text text;
    public float speed = 2f;
    public float squash = 0.01f;
    public float amount = 10f;
    void LateUpdate()
    {
        text.ForceMeshUpdate();
        var textInfo = text.textInfo;
        for(int i = 0; i < textInfo.characterCount; ++i)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;
            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            for(int j =0; j < 4; ++j)
            {
                var orig = verts[charInfo.vertexIndex + j];
                 verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.unscaledTime * speed + orig.x * squash) * amount, 0);
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; ++i)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            text.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
