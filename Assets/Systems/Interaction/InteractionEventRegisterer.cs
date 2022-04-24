using UnityEngine;
using UnityEngine.Events;

public class InteractionEventRegisterer : MonoBehaviour {

    [SerializeField]
    private UnityEvent Event;

    public InteractableType interactableType;

    [HideInInspector]
    public float startTime;

    private InteractionEventManager eventManager;

    private void InvokeEvent() {
        Event?.Invoke();
    }

    private void Start() {
        // check for errors
        eventManager = InteractionEventManager.instance;

        Event.AddListener(GetComponent<IInteractable>().OnPlayerInteraction);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            startTime = Time.time;
            eventManager.AddEvent(InvokeEvent, interactableType);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            eventManager.RemoveEvent(InvokeEvent, interactableType);
        }
    }
}
