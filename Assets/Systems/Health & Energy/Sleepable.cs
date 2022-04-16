using UnityEngine;

[RequireComponent(typeof(InteractionEventRegisterer))]
public class Sleepable : MonoBehaviour, IInteractable {

    private GameManager gameManager;

    private void Start() {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void OnPlayerInteraction() {
        gameManager.Sleep();
    }
}
