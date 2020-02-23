using UnityEngine;
using UnityEngine.AI;

public class Building : MonoBehaviour
{
    public ItemSelect item;
    public Node node;

    public string buildingName;
    public int cost;
    public int sell;

    public ParticleSystem deathEffect;
}
