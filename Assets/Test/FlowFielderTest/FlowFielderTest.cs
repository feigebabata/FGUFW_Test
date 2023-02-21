using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW;
using FGUFW.FlowFieldPathfinding;

public class FlowFielderTest : MonoBehaviour
{
    public Vector3 SpaceMin,SpaceMax;
    public Vector3Int GridCount;

    public Transform EndPoint,NodeRoot,MoveNodeRoot;


    private FGUFW.FlowFieldPathfinding.Space _space;


    // Start is called before the first frame update
    void Start()
    {
        _space = new FGUFW.FlowFieldPathfinding.Space(GridCount,SpaceMin,SpaceMax);
        NodeRoot.For<Grid>(NodeRoot.childCount,(i,comp)=>
        {
            _space[comp.transform.position] = new FGUFW.FlowFieldPathfinding.Grid((int)comp.Grid_Type,1);
        });

        if(!_space.Flow(EndPoint.position,1))return;

        foreach (Transform item in MoveNodeRoot)
        {
            
        }
    }

    IEnumerator pathMove(Transform node)
    {
        yield break;
    }

}
