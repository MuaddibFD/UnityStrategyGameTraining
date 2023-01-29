using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro textMeshPro;

    private GridObject gridObject;

    private void Start()
    {
        Destroy(textMeshPro);
    }

    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }
    private void Update()
    {
        //if (gridObject != null)
        //    textMeshPro.text = gridObject.ToString();
    }
}
