using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildManager : MonoBehaviour
{
    public NavMeshSurface surface;

    // A very small amount of time to wait to avoid building before gameobjects inst/destroyed
    private const float waitSecondsBeforeBuildNavmeshSurface = 0.00001f;

    private void Start()
    {
        surface.BuildNavMesh();
    }

    public void Build(Node node, GameObject building)
    {
        Vector3 position = node.GetBuildPosition();

        GameObject toInst = Instantiate(building, position, Quaternion.identity);
        string name = toInst.GetComponent<Building>().buildingName;

        node.SetOccupied(true);
        node.SetBuilding(toInst);

        toInst.GetComponent<Building>().node = node;

        surface.BuildNavMesh();

        //Debug.Log("Built " + name);
    }

    public void Destroy(Node node)
    {
        GameObject toSell = node.GetBuilding();
        string name = toSell.GetComponent<Building>().buildingName;

        
        Destroy(Instantiate(toSell.GetComponent<Building>().deathEffect.gameObject, toSell.transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject, 2);
        Destroy(toSell);
        node.SetOccupied(false);
        node.SetBuilding(null);

        StartCoroutine(RebuildNavmesh());

        //Debug.Log("Destroyed " + name);
    }

    private IEnumerator RebuildNavmesh()
    {
        yield return new WaitForSeconds(waitSecondsBeforeBuildNavmeshSurface);

        surface.BuildNavMesh();
    }
}
