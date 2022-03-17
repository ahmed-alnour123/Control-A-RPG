using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(InteractionEventRegisterer))]
public class LootableObject : MonoBehaviour, IInteractable {

    public LootableObjectSO data;
    public List<Collectable> collectables;

    private new string name;
    private int animationId;
    private float timeToLoot;
    private float lastTime;
    private bool isCalled;
    private bool isLooted;
    private GameObject model;
    private InteractionEventRegisterer eventRegisterer;

    void Start() {
        name = data.name;
        animationId = data.animationId;
        timeToLoot = data.timeToLoot;
        model = data.model;

        eventRegisterer = GetComponent<InteractionEventRegisterer>();

        Instantiate(model, transform);
    }

    public void OnPlayerInteraction() {
        if (Time.time - eventRegisterer.startTime >= timeToLoot && !isLooted) {
            isLooted = true;
            Destroy(gameObject);
            collectables.ForEach(c => InventoryManager.instance.AddToInventory(c));
        }
    }
}
