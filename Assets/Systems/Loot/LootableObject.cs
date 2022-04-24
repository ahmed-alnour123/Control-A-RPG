using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InteractionEventRegisterer))]
public class LootableObject : MonoBehaviour, IInteractable {

    // public LootableObjectSO data;
    public List<InventoryItem> collectables;
    public int requiredLevel;
    public int xp;
    public int requiredEnergy;
    public float timeToLoot;
    public GameObject lootingAnimation;


    // private int animationId;
    // private float lastTime;
    // private GameObject model;
    private bool isLooted;
    private float startTimeToLoot;

    private InteractionEventRegisterer eventRegisterer;
    private Inventory playerInventory;
    private PlayerXP playerXP;
    private RectTransform canvas;
    private Image lootingAnimationFill;
    private IEnumerator routine;

    void Start() {
        // animationId = data.animationId;
        // model = data.model;

        eventRegisterer = GetComponent<InteractionEventRegisterer>();
        playerInventory = FindObjectOfType<PlayerInteraction>().GetComponent<Inventory>();
        playerXP = FindObjectOfType<PlayerXP>();
        canvas = GameObject.FindWithTag("InGameCanvas").GetComponent<RectTransform>();
        startTimeToLoot = timeToLoot;

        transform.rotation = Quaternion.Euler(Vector3.up * Random.Range(0, 360));

        var models = transform.Find("Models");
        if (models != null && models.childCount > 0) {
            var luckyModelIndex = Random.Range(0, models.childCount);
            for (int i = 0; i < models.childCount; i++) {
                if (i == luckyModelIndex) continue;
                Destroy(models.GetChild(i).gameObject);
            }
        }
    }

    private void Collect() {
        isLooted = true;
        Destroy(gameObject);
        playerXP.AddXP(xp);
        playerXP.ChangeEnergy(-requiredEnergy);
        collectables.ForEach(c => playerInventory.AddItem(c, c.count));
    }

    public void OnPlayerInteraction() {
        if (playerXP.level < requiredLevel || playerXP.currentEnergy < requiredEnergy) return; // TODO: show message to player

        if (eventRegisterer.interactableType == InteractableType.Field) {
            if (Time.time - eventRegisterer.startTime >= timeToLoot && !isLooted)
                Collect();

            if (lootingAnimationFill != null)
                lootingAnimationFill.fillAmount = GetFillAmount();
        } else {
            if ((timeToLoot -= Time.deltaTime) <= 0)
                Collect();

            if (lootingAnimationFill != null)
                lootingAnimationFill.fillAmount = GetFillAmount();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (playerXP.level < requiredLevel || playerXP.currentEnergy < requiredEnergy) return; // TODO: show message to player

        if (other.CompareTag("Player")) {
            CreateLootingUI();

            if (routine != null)
                StopCoroutine(routine);
        }
    }

    private void CreateLootingUI() {
        if (lootingAnimationFill != null) return;
        var newLootAnim = Instantiate(lootingAnimation, canvas);
        newLootAnim.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2);
        lootingAnimationFill = newLootAnim.transform.Find("LootingForeground").GetComponent<Image>();
        lootingAnimationFill.fillAmount = GetFillAmount();
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            routine = LootingAnimTimeout();
            StartCoroutine(routine);
        }
    }

    private float GetFillAmount() {

        if (eventRegisterer.interactableType == InteractableType.Field) {
            return ((Time.time - eventRegisterer.startTime) / timeToLoot);
        } else {
            return 1 - (timeToLoot / startTimeToLoot);
        }
    }

    IEnumerator LootingAnimTimeout() {
        if (lootingAnimationFill == null) yield break;

        yield return new WaitForSeconds(2);
        if (lootingAnimationFill != null) {
            var parent = lootingAnimationFill.transform.parent;
            Destroy(parent.gameObject);
        }
    }

    private void OnDestroy() {
        if (lootingAnimationFill != null) {
            var parent = lootingAnimationFill.transform.parent;
            Destroy(parent.gameObject);
        }
    }

}
