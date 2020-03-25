using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Collections;

// Class handles the beginning and the ending of game

public class GameManager : MonoBehaviour
{
    public NavMeshAgent dummy;

    // True when player has no move lives left and game is over
    public static bool isGameOver;


    public static GameManager _instance;

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
        isGameOver = false;

        dummy.SetDestination(GameObject.FindGameObjectWithTag("Destination").transform.position);
        dummy.speed = 0;

        StartCoroutine(OnPartialPath());
    }

    private void Update()
    {
        if (isGameOver)
        {
            Debug.Log("Game is over");
            enabled = false;
        }

        if (dummy.pathStatus == NavMeshPathStatus.PathPartial)
        {
            StartCoroutine(OnPartialPath());
        }
    }

    private void OnGameOver()
    {
        Debug.Log("Game is over");
        enabled = false;

        // TODO: Implement end screen
    }

    // Checks if there is a valid path from start to destination and reacts accordingly
    private IEnumerator OnPartialPath()
    {
        while (dummy.pathStatus == NavMeshPathStatus.PathPartial)
        {
            Debug.Log(dummy.pathStatus);

            List<Node> activeNodes = GetActiveNodes();

            if (activeNodes.Count > 0)
            {
                int random = Random.Range(0, activeNodes.Count);
                Node randomNode = activeNodes[random];

                BuildManager._instance.Destroy(randomNode);
                dummy.SetDestination(GameObject.FindGameObjectWithTag("Destination").transform.position);
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    // Finds all node objects that are active
    public List<Node> GetActiveNodes()
    {
        List<Node> activeNodes = new List<Node>();

        GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");

        foreach (GameObject node in nodes)
        {
            if (node.GetComponent<Node>().IsOccupied() && node.GetComponent<Node>().GetBuildingObject() != null)
            {
                activeNodes.Add(node.GetComponent<Node>());
            }
        }

        return activeNodes;
    }

    public void GameOver()
    {
        isGameOver = true;
    }
}
