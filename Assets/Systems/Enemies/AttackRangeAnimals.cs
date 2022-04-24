using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeAnimals : MonoBehaviour
{
    public List<Collider> inRangeColliders = new List<Collider>();
    public bool attacking;
    public bool justAttcked;
    public float attackPoints;

    private void OnTriggerEnter(Collider other)
    {
        inRangeColliders.Add(other);

    }
    private void OnTriggerExit(Collider other)
    {
        inRangeColliders.Remove(other);

    }
}