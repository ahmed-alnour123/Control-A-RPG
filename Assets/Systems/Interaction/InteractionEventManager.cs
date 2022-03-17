using UnityEngine;
using UnityEngine.Events;

public class InteractionEventManager : MonoBehaviour {

    public static InteractionEventManager instance;

    /// <summary> Event that automatically triggers when player enters an area</summary>
    [SerializeField]
    private UnityEvent fieldEvent;

    /// <summary> Event that triggers when player presses down a button</summary>
    [SerializeField]
    private UnityEvent touchEvent;

    public void InvokeEvent(InteractableType eventType) {
        switch (eventType) {
            case InteractableType.Field:
                fieldEvent?.Invoke();
                break;
            case InteractableType.Touch:
                touchEvent?.Invoke();
                break;
        }
    }

    private void Awake() {
        instance = this;
    }

    public void AddEvent(UnityAction action, InteractableType eventType) {
        switch (eventType) {
            case InteractableType.Field:
                fieldEvent.AddListener(action);
                break;
            case InteractableType.Touch:
                touchEvent.AddListener(action);
                break;
        }
    }

    public void RemoveEvent(UnityAction action, InteractableType eventType) {
        switch (eventType) {
            case InteractableType.Field:
                fieldEvent.RemoveListener(action);
                break;
            case InteractableType.Touch:
                touchEvent.RemoveListener(action);
                break;
        }
    }

    public void RemoveAllEvent(UnityAction action, InteractableType eventType) {
        switch (eventType) {
            case InteractableType.Field:
                fieldEvent.RemoveAllListeners();
                break;
            case InteractableType.Touch:
                touchEvent.RemoveAllListeners();
                break;
        }
    }
}

public enum InteractableType {
    /// <summary>interact by player entering area of effect</summary>
    Field,
    /// <summary>interact by player touching it</summary>
    Touch
}

