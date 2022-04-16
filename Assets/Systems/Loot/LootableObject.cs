using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(InteractionEventRegisterer))]
public class LootableObject : MonoBehaviour, IInteractable {

    public LootableObjectSO data;
    public List<InventoryItem> collectables;
    public int requiredLevel;
    public int xp;
    public int requiredEnergy;

    private new string name;
    private int animationId;
    private float timeToLoot;
    private float lastTime;
    private bool isCalled;
    private bool isLooted;
    private GameObject model;
    private InteractionEventRegisterer eventRegisterer;
    private Inventory playerInventory;
    private PlayerXP playerXP;

    void Start() {
        name = data.name;
        animationId = data.animationId;
        timeToLoot = data.timeToLoot;
        model = data.model;

        eventRegisterer = GetComponent<InteractionEventRegisterer>();
        playerInventory = FindObjectOfType<PlayerInteraction>().GetComponent<Inventory>();
        playerXP = FindObjectOfType<PlayerXP>();

        if (transform.childCount == 0) {
            Instantiate(model, transform);
        }
    }

    public void OnPlayerInteraction() {
        if (playerXP.level < requiredLevel || playerXP.currentEnergy < requiredEnergy) return; // TODO: show message to player
        if (Time.time - eventRegisterer.startTime >= timeToLoot && !isLooted) {
            isLooted = true;
            Destroy(gameObject);
            playerXP.AddXP(xp);
            playerXP.ChangeEnergy(-requiredEnergy);
            collectables.ForEach(c => playerInventory.AddItem(c, c.count));
        }
    }
}
