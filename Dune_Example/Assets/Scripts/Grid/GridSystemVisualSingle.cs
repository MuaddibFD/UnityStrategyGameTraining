using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer meshRenderer;

    internal void Show(Material material)
    {
        meshRenderer.enabled = true;

        meshRenderer.material = material;
    }

    internal void Hide()
    {
        meshRenderer.enabled = false;
    }
}
