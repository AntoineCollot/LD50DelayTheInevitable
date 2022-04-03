using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generate a new instanced materials for a list of renderers so that we don't edit the material in the project.
/// </summary>
public class MaterialInstance : MonoBehaviour
{
    [System.Serializable]
    public struct RendererData
    {
        public Renderer renderer;
        public int materialId;
    }

    public List<RendererData> renderers = new List<RendererData>();

    bool hasBeenInstanced = false;
    Material instancedMat;
    public Material InstancedMat
    {
        get
        {
            if (!hasBeenInstanced)
            {
                GenerateMaterialInstance();
            }
            return instancedMat;
        }
    }

    void GenerateMaterialInstance()
    {
        //Use the first material as a base
        instancedMat = new Material(renderers[0].renderer.sharedMaterials[renderers[0].materialId]);
        instancedMat.name = instancedMat.name + "(Instanced)";

        foreach (RendererData data in renderers)
        {
            Material[] mats = data.renderer.sharedMaterials;
            mats[data.materialId] = instancedMat;
            data.renderer.sharedMaterials = mats;
        }
        hasBeenInstanced = true;
    }
}
