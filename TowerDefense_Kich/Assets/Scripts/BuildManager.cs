using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildManager : UnityEngine.MonoBehaviour
{
    public NavMeshSurface surface;

    public void Build(Node node, GameObject building)
    {
        Vector3 position = node.GetBuildPosition();

        GameObject toInst = Instantiate(building, position, Quaternion.identity);
        node.SetOccupied(true);
        node.SetBuilding(toInst);
        surface.BuildNavMesh();
    }

    public void Sell(Node node)
    {
        GameObject toSell = node.GetBuilding();

        Destroy(toSell);
        node.SetOccupied(false);
        node.SetBuilding(null);
        surface.BuildNavMesh();
    }
}
