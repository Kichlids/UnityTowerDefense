using UnityEngine;

public class Node : MonoBehaviour
{
    private Vector3 buildPosition;
    public bool isOccupied;
    public GameObject building;

    private void Start()
    {
        buildPosition = transform.position + new Vector3(0, 1f, 0);
        isOccupied = false;
    }

    public Vector3 GetBuildPosition()
    {
        return buildPosition;
    }

    public bool IsOccupied()
    {
        return isOccupied;
    }

    public void SetOccupied(bool set)
    {
        isOccupied = set;
    }

    public void SetBuilding(GameObject toSet)
    {
        building = toSet;
    }

    public GameObject GetBuilding()
    {
        return building;
    }
}
