using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputUnit
{
    public event Action inputEvent;
    public bool isAxis;

    public delegate float CheckerByAxis(string axisName);
    public delegate bool CheckerByKey(KeyCode key);
    public delegate bool SimpleChecker();

    public SimpleChecker InputSimpleChecker
    {
        get;
        set;
    }

    public CheckerByKey InputCheckerByKey //way to check input GetKeyDown | GetKey
    {
        get;
        set;
    }

    public CheckerByAxis InputCheckerByAxis //way to check input GetKeyDown | GetKey
    {
        get;
        set;
    }

    public KeyCode Key
    {
        get;
        set;
    }

    public string Axis
    {
        get;
        set;
    }

    public InputUnit(KeyCode iKey)
    {
        isAxis = false;
        Key = iKey;
        InputCheckerByKey = Input.GetKey;
    }

    public InputUnit(string iaxisName)
    {
        isAxis = true;
        Axis = iaxisName;
        InputCheckerByAxis = Input.GetAxis;
    }

    public float axisValue
    {
        get { return InputCheckerByAxis(Axis); }
    }

    public bool keyPressed
    {
        get
        {
            bool res = true;
            if (InputCheckerByKey != null)
                res = res && InputCheckerByKey(Key);
            if (InputSimpleChecker != null)
                res = res && InputSimpleChecker();
            return res;
        }
    }

    //Does not support raise event on Axis (Its unnecessary)
    public void Update()
    {
        bool res = true;

        if (isAxis)
        {
            InputCheckerByAxis(Axis);
        }
        else
        {
            if (InputCheckerByKey != null)
                res = res && InputCheckerByKey(Key);
            if (InputSimpleChecker != null)
                res = res && InputSimpleChecker();

            if (inputEvent != null)
                if (res)
                    inputEvent();
        }
    }
}


public delegate bool boolDelegate();

//Behaviour inputs are inputs for different character behaviours. They can be swapped or disabled/enabled
public class CharacterManager : MonoBehaviour
{
    public boolDelegate getOrbitalCamBool;
    public FlexiblePlayerInput playerInput;
    public string[] behaviourNames = new string[] { };
    public Dictionary<string, FlexibleBehaviourInput> behaviourInputs = new Dictionary<string, FlexibleBehaviourInput>();

    //Controllers
    public FlexibleBehaviourInput_Interact m_interactor;
    public Rotator m_rotationWithMouseController;
    public CraftCharacterController m_controller;
    public ToolActionController m_toolActionController;
    public GameObject m_inventoryMenu;

    public bool m_isInMenu = false;
    public bool getNames = false;

    // Start is called before the first frame update
    public void Awake()
    {

        //m_interactor = GetComponent<AccessViaRayCast>();

        //InitializeMenuInputs();
        //InitializeInteractInputs();
        //InitializeToolsActionsInputs();
        //InitializePlacementInputs();

        //GoToGame();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (getNames)
            GetBehaviourNames();

        if (playerInput.enabled)
            playerInput.CheckInputEvents();

        foreach (KeyValuePair<string, FlexibleBehaviourInput> behaviourInput in behaviourInputs)
            if (behaviourInput.Value.enabled)
            {
                behaviourInput.Value.UpdateInputs();
                behaviourInput.Value.BehaviourUpdate();
            }

        //p1_Axis1 = Input.GetAxisRaw("Mouse X") * p1_sensitivity;
        //p1_Axis2 = Input.GetAxisRaw("Mouse Y") * p1_sensitivity;

        //m_movingController.ClearValue();

        //foreach (InputGroup inputUnit in m_inputGroups.Values)
        //    inputUnit.Execute();

        //m_movingController.Walk();
    }

    public void GetBehaviourNames()
    {
        behaviourNames = new string[behaviourInputs.Keys.Count];
        behaviourInputs.Keys.CopyTo(behaviourNames, 0);
    }

    public void AddBehaviour(FlexibleBehaviourInput newBehaviour)
    {
        if (!behaviourInputs.ContainsKey(newBehaviour.behaviourName))
        {
            behaviourInputs.Add(newBehaviour.behaviourName, newBehaviour);
        }
    }

    public bool RemoveBehaviour(string oldBehaviour)
    {
        return behaviourInputs.Remove(oldBehaviour);
    }
}




//void InitializeMovingInputs()
//{
//}

//void InitializeMenuInputs()
//{
//    //push escape -> call EscapeHandler
//    InputUnit unit = new InputUnit { InputCheckerByKey = Input.GetKeyDown, Key = KeyCode.Escape };
//    unit.inputEvent += EscapeHandler;
//    group.inputs.Add(InputManager.Escape, unit);

//    //push Tab -> call InventoryHandler
//    unit = new InputUnit { InputCheckerByKey = Input.GetKeyDown, Key = KeyCode.Tab };
//    unit.inputEvent += InventoryHandler;
//    group.inputs.Add(InputManager.Inventory, unit);

//    m_inputGroups.Add(InputManager.MenuGroup, group);
//}

//void InitializeToolsActionsInputs()
//{
//    KeyCode keyCode = KeyCode.Alpha1;
//    InputUnit unit;

//    //push '1' -> select first item on tool panel
//    //push '6' -> select 6th item
//    for (int i = 1; i <= 8; ++i, ++keyCode)
//    {
//        unit = new InputUnit { InputCheckerByKey = Input.GetKeyDown, Key = keyCode };
//        int j = i;
//        unit.inputEvent += () => m_controller.SetTool(j);
//        group.inputs.Add(i.ToString(), unit);
//    }

//    //scroll down -> select next item on tool panel
//    //scroll up -> select previous item on tool panel
//    unit = new InputUnit();
//    unit.inputEvent += () =>
//    {
//        float scroll = Input.GetAxis("Mouse ScrollWheel");
//        if (scroll > 0)
//            m_controller.SelectPreviousTool();
//        else if (scroll < 0)
//            m_controller.SelectNextTool();
//    };
//    group.inputs.Add(InputManager.SelectToolWithScroll, unit);

//    //push 'Q' -> drop item in hands
//    unit = new InputUnit { InputCheckerByKey = Input.GetKeyDown, Key = KeyCode.Q };
//    unit.inputEvent += m_controller.DropSelectedItem;
//    group.inputs.Add(InputManager.DropItem, unit);

//    m_inputGroups.Add(InputManager.ToolsActionsGroup, group);
//}

//void InitializeInteractInputs()
//{
//    //push 'E' -> item pick up executes
//    InputUnit unit = new InputUnit { InputCheckerByKey = Input.GetKeyDown, Key = KeyCode.E };
//    unit.inputEvent += m_interactor.Interact;
//    group.inputs.Add(InputManager.Interaction, unit);

//    //click -> call item interaction
//    unit = new InputUnit { InputCheckerByKey = (k) => !m_isInMenu && Input.GetKey(k), Key = KeyCode.Mouse0 };
//    unit.inputEvent += new Action(() => { if (m_toolActionController) m_toolActionController.Hit(); });
//    group.inputs.Add(InputManager.InteractionHit, unit);

//    m_inputGroups.Add(InputManager.InteractGroup, group);
//}

//void InitializePlacementInputs()
//{
//    //push 'F' -> call placement method
//    InputUnit unit = new InputUnit { InputCheckerByKey = Input.GetKeyDown, Key = KeyCode.F };
//    unit.inputEvent += m_controller.ObjectPlacement;
//    group.inputs.Add(InputManager.PlaceObject, unit);

//    //push 'R' -> call rotating method in placement mode
//    unit = new InputUnit { InputCheckerByKey = Input.GetKey, Key = KeyCode.R };
//    unit.inputEvent += m_controller.ObjectRotating;
//    group.inputs.Add(InputManager.RotateObject, unit);

//    m_inputGroups.Add(InputManager.PlacementGroup, group);
//}

//void EscapeHandler()
//{
//    if (m_inventoryMenu.activeSelf)
//    {
//        if (!m_controller.m_inventory.m_isItPossibleToGoToGame)
//            return;
//        m_inventoryMenu.SetActive(false);
//        GoToGame();
//    }
//    else if (m_isInMenu) // go to game
//    {
//        if (!m_controller.m_inventory.m_isItPossibleToGoToGame)

//            return;
//        GoToGame();
//        //m_inputGroups[InputManager.MenuGroup].inputs[InputManager.Inventory].inputEvent += InventoryHandler;
//        //code to continue game
//    }
//    else // go to menu
//    {
//        GoToMenu();
//        //m_inputGroups[InputManager.MenuGroup].inputs[InputManager.Inventory].inputEvent -= InventoryHandler;
//        //code to pause game
//    }
//}

//void InventoryHandler()
//{
//    if (m_isInMenu) // switch off inventory
//    {
//        if (!m_controller.m_inventory.m_isItPossibleToGoToGame)
//            return;
//        m_inventoryMenu.SetActive(false);
//        GoToGame();
//    }
//    else
//    {
//        m_inventoryMenu.SetActive(true); //switch on inventory
//        GoToMenu();
//    }
//}

//public InputUnit GetInputUnit(string group, string key)
//{
//    return m_inputGroups[group].inputs[key];
//}

//void GoToMenu()
//{
//    Cursor.lockState = CursorLockMode.None;
//    Cursor.visible = true;
//    behaviourInputs["Movement"].Disable();
//    behaviourInputs["Camera"].Enable();
//    behaviourInputs["Interaction"].Disable();
//    //behaviourInputs["Placement"].Disable();
//    //behaviourInputs["Tools"].Disable();
//    m_rotationWithMouseController.Enabled = false;
//    m_isInMenu = true;
//}

//void GoToGame()
//{
//    m_controller.DisablePlacementMode();
//    Cursor.lockState = CursorLockMode.Locked;
//    Cursor.visible = false;
//    behaviourInputs["Movement"].Enable();
//    behaviourInputs["Camera"].Enable();
//    behaviourInputs["Interaction"].Enable();
//    //behaviourInputs["Placement"].Enable();
//    //behaviourInputs["Tools"].Enable();
//    m_rotationWithMouseController.Enabled = true;
//    m_isInMenu = false;
//}