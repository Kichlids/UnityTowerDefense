using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildManager : MonoBehaviour
{
    public NavMeshSurface surface;

    // A very small amount of time to wait to avoid building before gameobjects inst/destroyed
    private const float waitSecondsBeforeBuildNavmeshSurface = 0.00001f;


    public static BuildManager _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        surface.BuildNavMesh();
    }

    public void Build(Node node, GameObject building)
    {
        Vector3 position = node.GetBuildPosition();

        GameObject buildingObject = Instantiate(building, position, Quaternion.identity);

        node.SetOccupied(true);
        node.SetBuildingObject(buildingObject);

        buildingObject.GetComponent<Building>().node = node;

        UIManager._instance.lastSelectedBuilding = building;

        surface.BuildNavMesh();
    }

    public void Destroy(Node node)
    {
        GameObject buildingToSell = node.GetBuildingObject();
        
        Destroy(Instantiate(buildingToSell.GetComponent<Building>().deathEffect.gameObject, buildingToSell.transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject, 2);
        Destroy(buildingToSell);

        node.SetOccupied(false);
        node.SetBuildingObject(null);

        StartCoroutine(RebuildNavmesh());
    }

    private IEnumerator RebuildNavmesh()
    {
        yield return new WaitForSeconds(waitSecondsBeforeBuildNavmeshSurface);

        surface.BuildNavMesh();
    }
}
