using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public ItemSelect item;
    public Node node;

    public Sprite sprite;
    public string buildingName;
    public int cost;
    public int sell;

    public int damageDone = 0;

    public ParticleSystem deathEffect;
}
