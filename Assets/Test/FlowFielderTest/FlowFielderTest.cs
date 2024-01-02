using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW;
using FGUFW.FlowFieldPathfinding;
using Unity.Collections;
using Unity.Mathematics;

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
            _space[comp.transform.position] = new FGUFW.FlowFieldPathfinding.Grid((uint)comp.Grid_Type,1);
        });

        if(!_space.Flow(EndPoint.position,3))return;

        NodeRoot.For<Grid>(NodeRoot.childCount,(i,comp)=>
        {
            comp.Num.text = _space[comp.transform.position].Distance.ToString();
        });

        foreach (Transform item in MoveNodeRoot)
        {
            StartCoroutine(pathMove(item));
        }

    }


    IEnumerator pathMove(Transform node)
    {
        List<Vector3> path = new List<Vector3>();
        _space.MoveTo(node.position,new Vector3(0.5f,0.5f,0.5f),(coord,grid,point)=>
        {
            path.Add(point);
        });
        foreach (var item in path)
        {
            node.transform.position = item;
            yield return new WaitForSeconds(1);
        }
        yield break;
    }

}
