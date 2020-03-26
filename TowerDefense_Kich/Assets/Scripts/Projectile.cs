using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject target;
    private Building source;
    private float speed = 100f;
    private int damage = 1;

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.transform.position - transform.position;
        float dist = speed * Time.deltaTime;

        if (direction.magnitude <= dist)
        {
            CollideTarget();
            return;
        }

        transform.Translate(direction.normalized * dist, Space.World);
        transform.LookAt(target.transform);
    }

    public void Initialize(Building _source, int _damage, GameObject _target)
    {
        source = _source;
        damage = _damage;
        target = _target;
    }

    private void CollideTarget()
    {
        target.GetComponent<Enemy>().TakeDamage(damage);
        source.damageDone += damage;
        Destroy(gameObject);
    }

    public void SetTarget(GameObject _target)
    {
        target = _target;
    }

    public void SetDamage(int _damage)
    {
        damage = _damage;
    }
}
