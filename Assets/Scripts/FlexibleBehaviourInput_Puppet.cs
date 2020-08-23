using System.Collections;
using UnityEngine;
using RootMotion.Demos;

/// <summary>
/// ComplexCharacterController based on Puppetmaster
/// </summary>
public class FlexibleBehaviourInput_Puppet : FlexibleBehaviourInput
{
    public Vector3 _direction;
    public float rotationSpeed = 2f, speed = 2f, walkMultiplier = 0.5f;
    public float targetRotationOffset = 0f;
    public float headTurnAmountBeforeBodyTurn = 0.5f;

    // Input state
    public struct State
    {
        public Vector3 move;
        public Vector3 lookPos;
        public bool crouch;
        public bool jump;
        public int actionIndex;
    }

    public bool walkByDefault;        // toggle for walking state
    public bool canCrouch = true;
    public bool canJump = true;

    public State state = new State();           // The current state of the user input

    public Transform headBone;
    public Animator animator;
    public bool headBoneInitialised = false;
    public float headXReturnSpeed, headYReturnSpeed;
    public float headXSpeed = 0f, headYSpeed = 0f, headXLimit = 0f, headYLimit = 0f;
    public float headXMovement = 0f, headYMovement = 0f;

    public Transform target;                    // A reference to the main camera in the scenes transform
    public bool orbitalMode = false;

    //public KeyCode hitKey;
    public float h, v, x, y;
    public bool jump, crouch, interact, run;

    public override void AdditionalInit()
    {
        mgr.getOrbitalCamBool = orbitalModeBool;
    }

    public bool orbitalModeBool()
    {
        return orbitalMode;
    }

    public override void UpdateInputs()
    {
        h = behaviourPlayerInput.inputs[FlexiblePlayerInput.Input_Axis11].axisValue;
        v = behaviourPlayerInput.inputs[FlexiblePlayerInput.Input_Axis12].axisValue;
        x = behaviourPlayerInput.inputs[FlexiblePlayerInput.Input_Axis21].axisValue;
        y = behaviourPlayerInput.inputs[FlexiblePlayerInput.Input_Axis22].axisValue;
        run = behaviourPlayerInput.inputs[FlexiblePlayerInput.Input_B].keyPressed;
        crouch = behaviourPlayerInput.inputs[FlexiblePlayerInput.Input_A].keyPressed;
        jump = behaviourPlayerInput.inputs[FlexiblePlayerInput.Input_C].keyPressed;
        interact = behaviourPlayerInput.inputs[FlexiblePlayerInput.Input_D].keyPressed;
    }

    public override void Enable()
    {
        this.enabled = true;
    }

    public override void Disable()
    {
        this.enabled = false;
    }

    public void EnableOrbitalMode()
    {
        orbitalMode = true;
    }

    public void DisableOrbitalMode()
    {
        orbitalMode = false;
    }

    public override void BehaviourUpdate ()
    {
		state.actionIndex = interact ? 1: 0;
        state.crouch = canCrouch && crouch;
        state.jump = canJump && jump;

        if (!orbitalMode)
            transform.Rotate(new Vector3(0f, x, 0f) * Time.deltaTime * rotationSpeed);

        // calculate move direction
        Vector3 move = transform.forward * v * -1f;//orientation.rotation * new Vector3(0f, 0f, -1 * v).normalized;

        // Flatten move vector to the character.up plane
        if (move != Vector3.zero)
        {
            Vector3 normal = transform.up;
            Vector3.OrthoNormalize(ref normal, ref move);
            state.move = move;
        }
        else state.move = Vector3.zero;

        bool walkToggle = run;

        // We select appropriate speed based on whether we're walking by default, and whether the walk/run toggle button is pressed:
        float multiplier = (walkByDefault ? walkToggle ? 1 : walkMultiplier : walkToggle ? walkMultiplier : 1);

        state.move *= multiplier * speed;

        // calculate the head look target position
        state.lookPos = transform.position + target.forward * 100f;
        RotateHead();
    }

    public void RotateHead()
    {
        if (headBoneInitialised)
        {
            headXMovement = headXMovement + x * headXSpeed * Time.deltaTime;
            headXMovement = Mathf.Lerp(headXMovement, 0f, headXReturnSpeed * Time.deltaTime);

            headYMovement = headYMovement + y * headYSpeed * Time.deltaTime;
            headYMovement = Mathf.Lerp(headYMovement, 0f, headYReturnSpeed * Time.deltaTime);

            //Head movement limits
            headXMovement = Mathf.Clamp(headXMovement, -1f * headXLimit, headXLimit);
            headYMovement = Mathf.Clamp(headYMovement, -1f * headYLimit, headYLimit);

            //Apply the rotation
            animator.SetFloat("HeadVertical", headYMovement);
            animator.SetFloat("HeadHorizontal", headXMovement);
            target.localRotation = Quaternion.Euler(targetRotationOffset + headYMovement * -90f, 0f, 0f);
        }
    }

    public void InitialiseHeadBone(Transform headObject)
    {
        headBoneInitialised = true;
        headBone = headObject.parent;
    }
}