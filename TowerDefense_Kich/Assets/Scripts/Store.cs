using UnityEngine;

public enum ItemSelect { clear, barrier, tower1, sell };

public class Store : MonoBehaviour
{
    public ItemSelect item;

    public GameObject barrierPrefab;
    public GameObject tower1Prefab;

    private const float RAYDIST = 1000f;


    public static Store _instance;

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
        item = ItemSelect.clear;
    }

    public void PurchaseBuilding(ItemSelect item)
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, RAYDIST))
        {
            if (hit.collider.gameObject.tag == "Node")
            {
                Node node = hit.collider.gameObject.GetComponent<Node>();

                // Sell existing item
                if (node.IsOccupied())
                {
                    Player._instance.SellBuilding(node.buildingObject);
                    BuildManager._instance.Destroy(node);
                }

                GameObject toBuild = SelectBuilding(item);

                if (toBuild != null)
                {
                    int cost = toBuild.GetComponent<Building>().cost;

                    if (Player._instance.CanPurchase(cost))
                    {
                        Player._instance.Purchase(cost);
                        BuildManager._instance.Build(node, toBuild);
                    }
                    else
                        Debug.Log("Not enough gold");
                }
            }
        }
    }

    public void SellBuilding()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, RAYDIST))
        {
            if (hit.collider.gameObject.tag == "Node")
            {
                Node node = hit.collider.gameObject.GetComponent<Node>();

                if (node.IsOccupied())
                {
                    GameObject toSell = node.GetBuildingObject();

                    if (toSell != null)
                    {
                        Player._instance.SellBuilding(toSell);
                        BuildManager._instance.Destroy(node);
                    }
                }
                else
                    Debug.Log("No building to sell");
            }
        }
    }

    private GameObject SelectBuilding(ItemSelect item)
    {
        if (item == ItemSelect.barrier)
        {
            return barrierPrefab;
        }
        else if (item == ItemSelect.tower1)
        {
            return tower1Prefab;
        }

        return null;
    }
}
