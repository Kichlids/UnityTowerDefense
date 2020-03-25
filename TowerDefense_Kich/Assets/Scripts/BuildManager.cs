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

        GameObject toInst = Instantiate(building, position, Quaternion.identity);
        string name = toInst.GetComponent<Building>().buildingName;

        node.SetOccupied(true);
        node.SetBuildingObject(toInst);

        toInst.GetComponent<Building>().node = node;

        if (toInst.GetComponent<Tower>() != null)
        {
            UIManager._instance.lastSelectedTower = toInst.GetComponent<Tower>();
        }

        surface.BuildNavMesh();
    }

    public void Destroy(Node node)
    {
        GameObject toSell = node.GetBuildingObject();
        string name = toSell.GetComponent<Building>().buildingName;

        
        Destroy(Instantiate(toSell.GetComponent<Building>().deathEffect.gameObject, toSell.transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject, 2);
        Destroy(toSell);
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
