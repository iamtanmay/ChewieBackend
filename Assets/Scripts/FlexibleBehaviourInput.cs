using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlexibleBehaviourInput : MonoBehaviour
{
    public CharacterManager mgr;
    public FlexiblePlayerInput behaviourPlayerInput;
    public string behaviourName;
    public abstract void UpdateInputs();
    public abstract void BehaviourUpdate();
    public abstract void Enable();
    public abstract void Disable();
    public abstract void AdditionalInit();

    public void Start()
    {
        mgr.AddBehaviour(this);
        AdditionalInit();
    }
}