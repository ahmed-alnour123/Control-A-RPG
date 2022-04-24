using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(InteractionEventRegisterer))]
public class PlantGrow : MonoBehaviour, IInteractable {

    public float growPhase1Time;
    public float growPhase2Time;


    [SerializeField]
    Plant plant;
    [SerializeField]
    InventoryItem vegetable;

    private Inventory playerInventory;
    private bool isGrown;

    void Start() {
        StartCoroutine(StartTimeout());
        playerInventory = FindObjectOfType<PlayerXP>().GetComponent<Inventory>();
    }


    IEnumerator StartTimeout() {
        yield return new WaitForSeconds(growPhase1Time);
        transform.Find("Model/Grow1").gameObject.SetActive(true);
        yield return new WaitForSeconds(growPhase1Time);

        transform.Find("Model/Grow1").gameObject.SetActive(false);
        transform.Find("Model/Grow2").gameObject.SetActive(true);
        isGrown = true;
    }

    public void OnPlayerInteraction() {
        if (!isGrown) return;
        playerInventory.AddItem(vegetable, vegetable.count);
        transform.Find("Model/Grow1").gameObject.SetActive(false);
        transform.Find("Model/Grow2").gameObject.SetActive(false);
        isGrown = false;
        StartCoroutine(StartTimeout());
    }
}

enum Plant {
    VegeRow,
    Carrot,
    EggPlant,
    Pepper,
    Watermelon
}
