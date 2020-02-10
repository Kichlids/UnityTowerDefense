using UnityEngine;

public enum ItemSelect { clear, barrier, tower1, sell };

public class Store : MonoBehaviour
{
    public ItemSelect item;

    private BuildManager buildManager;
    private Player player;

    public GameObject barrierPrefab;
    public GameObject tower1Prefab;

    private const float RAYDIST = 1000f;

    private void Start()
    {
        buildManager = GetComponent<BuildManager>();
        player = GetComponent<Player>();

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

                if (!node.IsOccupied())
                {
                    GameObject toBuild = SelectBuilding(item);

                    if (toBuild != null)
                    {
                        if (player.CanPurchase(toBuild.GetComponent<Building>().cost))
                        {
                            buildManager.Build(node, toBuild);

                            Debug.Log("Purchased " + toBuild.name);
                        }
                        else
                            Debug.Log("Not enough gold");
                    }
                }
                else
                    Debug.Log("Node is occupied");
            }
            else
                Debug.Log("Cannot place barrier on " + hit.collider.name);
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
                    GameObject toSell = node.GetBuilding();

                    if (toSell != null)
                    {
                        
                        player.SellBuilding(toSell);
                        buildManager.Sell(node);

                        Debug.Log("Sold " + toSell.name);
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
