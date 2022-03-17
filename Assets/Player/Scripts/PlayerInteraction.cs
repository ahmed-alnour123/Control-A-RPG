using UnityEngine;
using UnityEngine.Events;

public class PlayerInteraction : MonoBehaviour {

    void Start() {

    }


    void Update() {
        if (Input.GetKey(KeyCode.I)) {
            InteractionEventManager.instance.InvokeEvent(InteractableType.Touch);
        }
    }

    private void OnTriggerStay(Collider other) {
        InteractionEventManager.instance.InvokeEvent(InteractableType.Field);
    }
}
