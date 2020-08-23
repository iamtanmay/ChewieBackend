using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleBehaviourInput_Interact : FlexibleBehaviourInput
{
    public string[] actions = {"Use", "Take", "Steal", "Talk"};
    public Transform camTransform;
    public float interactRange = 1.0f;
    public Text interactText;
    public LayerMask interactLayer;
    bool interactKey;

    Inventory inventoryFrontend;

    public override void AdditionalInit()
    {
    }

    public override void UpdateInputs()
    {
        interactKey = behaviourPlayerInput.inputs[FlexiblePlayerInput.Input_D].keyPressed;
    }

    public override void Enable()
    {
        this.enabled = true;
    }

    public override void Disable()
    {
        this.enabled = false;
    }

    public override void BehaviourUpdate()
    {
        Ray ray = new Ray(transform.position, camTransform.transform.position - transform.position);
        Debug.DrawRay(transform.position, camTransform.transform.position - transform.position);

        RaycastHit hit;
        interactText.text = "";

        if (Physics.Raycast(ray, out hit, interactRange, interactLayer.value)) // raycast forward from camera (only objects with "Visible" layer set)
        {
            Item pickUp = hit.collider.gameObject.GetComponent<Item>();

            if (pickUp != null)
            {
                //print interaction key and hint to interact
                interactText.text = actions[0] + " (" + FlexiblePlayerInput.Input_D + ") " + pickUp.data.name;

                if (interactKey)
                    inventoryFrontend.Put(pickUp);
            }
        }
    }

    void Awake()
    {
        inventoryFrontend = GetComponent<Inventory>();
    }

    void Drop(Item item)
    {
        item.transform.position = transform.position + transform.forward * 2 + transform.up;
        item.transform.rotation = Quaternion.Euler(Random.insideUnitSphere * 100);
    }
}