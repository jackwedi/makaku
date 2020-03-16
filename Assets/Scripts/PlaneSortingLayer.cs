using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSortingLayer : MonoBehaviour
{
    [SerializeField] private int _sortingOrder = -1;
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<MeshRenderer>().sortingLayerName = "Terrain";
        transform.GetComponent<MeshRenderer>().sortingOrder = _sortingOrder;
    }

}
