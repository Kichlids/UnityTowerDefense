using UnityEngine;

public class Node : MonoBehaviour
{
    private Vector3 buildPosition;
    private bool isOccupied;
    public GameObject buildingObject;

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

    public void SetBuildingObject(GameObject toSet)
    {
        buildingObject = toSet;
    }

    public GameObject GetBuildingObject()
    {
        return buildingObject;
    }
}
