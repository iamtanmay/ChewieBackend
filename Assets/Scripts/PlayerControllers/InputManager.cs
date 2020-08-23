using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
/**    Dictionary<string, InputGroup> m_inputGroups;
    bool m_isInMenu = false;

    //Controllers
    public IMoving m_movingController;
    public AccessViaRayCast m_interactor;
    public Rotator m_rotationWithMouseController;
    public CraftCharacterController m_controller;
    public ToolActionController m_toolActionController;

    public float p1_Axis1 = 0f, p1_Axis2 = 0f, p1_Axis3 = 0f, p1_Axis4 = 0f;
    public float p2_Axis1 = 0f, p2_Axis2 = 0f, p2_Axis3 = 0f, p2_Axis4 = 0f;
    public float p3_Axis1 = 0f, p3_Axis2 = 0f, p3_Axis3 = 0f, p3_Axis4 = 0f;
    public float p4_Axis1 = 0f, p4_Axis2 = 0f, p4_Axis3 = 0f, p4_Axis4 = 0f;
    public float p1_sensitivity = 1f, p2_sensitivity = 1f, p3_sensitivity = 1f, p4_sensitivity = 1f;

    public GameObject m_inventoryMenu;

    public void FixedUpdate()
    {
        p1_Axis1 = Input.GetAxisRaw("Mouse X") * p1_sensitivity;
        p1_Axis2 = Input.GetAxisRaw("Mouse Y") * p1_sensitivity;

        m_movingController.ClearValue();

        foreach (InputGroup inputUnit in m_inputGroups.Values)
            inputUnit.Execute();

        m_movingController.Walk();
    }

    void Awake()
    {
        m_inputGroups = new Dictionary<string, InputGroup>();

        m_movingController = GetComponent<IMoving>();//bind moving controller

        if (m_movingController == null)
            Debug.LogError("Person needs IMoving Component");
        else
            InitializeMovingInputs();

        m_interactor = GetComponent<AccessViaRayCast>();

        InitializeMenuInputs();
        InitializeInteractInputs();
        InitializeToolsActionsInputs();
        InitializePlacementInputs();

        GoToGame();
    }

    void InitializeMovingInputs()
    {
        InputGroup group = new InputGroup();

        InputUnit unit = new InputUnit { InputCheckerByKey = Input.GetKey, Key = KeyCode.W };
        unit.inputEvent += m_movingController.MoveForward;
        group.inputs.Add(InputManager.MoveForward, unit);

        unit = new InputUnit { InputCheckerByKey = Input.GetKey, Key = KeyCode.S };
        unit.inputEvent += m_movingController.MoveBack;
        group.inputs.Add(InputManager.MoveBack, unit);

        unit = new InputUnit { InputCheckerByKey = Input.GetKey, Key = KeyCode.A };
        unit.inputEvent += m_movingController.MoveLeft;
        group.inputs.Add(InputManager.MoveLeft, unit);

        unit = new InputUnit { InputCheckerByKey = Input.GetKey, Key = KeyCode.D };
        unit.inputEvent += m_movingController.MoveRight;
        group.inputs.Add(InputManager.MoveRight, unit);

        unit = new InputUnit { InputCheckerByKey = Input.GetKeyDown, Key = KeyCode.Space };
        unit.inputEvent += m_movingController.Jump;
        group.inputs.Add(InputManager.Jump, unit);

        m_inputGroups.Add(InputManager.MovingGroup, group);
    }

    void InitializeMenuInputs()
    {
        InputGroup group = new InputGroup();

        //push escape -> call EscapeHandler
        InputUnit unit = new InputUnit { InputCheckerByKey = Input.GetKeyDown, Key = KeyCode.Escape };
        unit.inputEvent += EscapeHandler;
        group.inputs.Add(InputManager.Escape, unit);

        //push Tab -> call InventoryHandler
        unit = new InputUnit { InputCheckerByKey = Input.GetKeyDown, Key = KeyCode.Tab };
        unit.inputEvent += InventoryHandler;
        group.inputs.Add(InputManager.Inventory, unit);

        m_inputGroups.Add(InputManager.MenuGroup, group);
    }

    void InitializeToolsActionsInputs()
    {
        InputGroup group = new InputGroup();
        KeyCode keyCode = KeyCode.Alpha1;
        InputUnit unit;

        //push '1' -> select first item on tool panel
        //push '6' -> select 6th item
        for(int i = 1; i <= 8; ++i, ++keyCode)
        {
            unit = new InputUnit { InputCheckerByKey = Input.GetKeyDown, Key = keyCode };
            int j = i;
            unit.inputEvent += () => m_controller.SetTool(j);
            group.inputs.Add(i.ToString(), unit);
        }

        //scroll down -> select next item on tool panel
        //scroll up -> select previous item on tool panel
        unit = new InputUnit();
        unit.inputEvent += () =>
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll > 0)
                m_controller.SelectPreviousTool();
            else if (scroll < 0)
                m_controller.SelectNextTool();
        };
        group.inputs.Add(InputManager.SelectToolWithScroll, unit);

        //push 'Q' -> drop item in hands
        unit = new InputUnit { InputCheckerByKey = Input.GetKeyDown, Key = KeyCode.Q };
        unit.inputEvent += m_controller.DropSelectedItem;
        group.inputs.Add(InputManager.DropItem, unit);

        m_inputGroups.Add(InputManager.ToolsActionsGroup, group);
    }

    void InitializeInteractInputs()
    {
        InputGroup group = new InputGroup();

        //push 'E' -> item pick up executes
        InputUnit unit = new InputUnit { InputCheckerByKey = Input.GetKeyDown, Key = KeyCode.E };
        unit.inputEvent += m_interactor.Interact;
        group.inputs.Add(InputManager.Interaction, unit);

        //click -> call item interaction
        unit = new InputUnit { InputCheckerByKey = (k) => !m_isInMenu && Input.GetKey(k), Key = KeyCode.Mouse0 };
        unit.inputEvent += new Action(() => { if (m_toolActionController) m_toolActionController.Hit(); });
        group.inputs.Add(InputManager.InteractionHit, unit);

        m_inputGroups.Add(InputManager.InteractGroup, group);
    }

    void InitializePlacementInputs()
    {
        InputGroup group = new InputGroup();

        //push 'F' -> call placement method
        InputUnit unit = new InputUnit { InputCheckerByKey = Input.GetKeyDown, Key = KeyCode.F };
        unit.inputEvent += m_controller.ObjectPlacement;
        group.inputs.Add(InputManager.PlaceObject, unit);

        //push 'R' -> call rotating method in placement mode
        unit = new InputUnit { InputCheckerByKey = Input.GetKey, Key = KeyCode.R };
        unit.inputEvent += m_controller.ObjectRotating;
        group.inputs.Add(InputManager.RotateObject, unit);

        m_inputGroups.Add(InputManager.PlacementGroup, group);
    }

    void EscapeHandler()
    {
        if (m_inventoryMenu.active)
        {
            if (!m_controller.m_inventory.m_isItPossibleToGoToGame)
                return;
            m_inventoryMenu.SetActive(false);
            GoToGame();
        }
        else if (m_isInMenu) // go to game
        {
            if (!m_controller.m_inventory.m_isItPossibleToGoToGame)
                return;
            GoToGame();
            m_inputGroups[InputManager.MenuGroup].inputs[InputManager.Inventory].inputEvent += InventoryHandler;
            //code to continue game
        }
        else // go to menu
        {
            GoToMenu();
            m_inputGroups[InputManager.MenuGroup].inputs[InputManager.Inventory].inputEvent -= InventoryHandler;
            //code to pause game
        }
    }

    void InventoryHandler()
    {
        if (m_isInMenu) // switch off inventory
        {
            if (!m_controller.m_inventory.m_isItPossibleToGoToGame)
                return;
            m_inventoryMenu.SetActive(false);
            GoToGame();
        }
        else
        {
            m_inventoryMenu.SetActive(true); //switch on inventory
            GoToMenu();
        }
    }

    public InputUnit GetInputUnit(string group, string key)
    {
        return m_inputGroups[group].inputs[key];
    }

    void GoToMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        m_inputGroups[InputManager.MovingGroup].Enabled = false;
        m_inputGroups[InputManager.InteractGroup].Enabled = false;
        m_inputGroups[InputManager.PlacementGroup].Enabled = false;
        m_inputGroups[InputManager.ToolsActionsGroup].Enabled = false;
        m_rotationWithMouseController.Enabled = false;
        m_isInMenu = true;
    }

    void GoToGame()
    {
        m_controller.DisablePlacementMode();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        m_inputGroups[InputManager.MovingGroup].Enabled = true;
        m_inputGroups[InputManager.InteractGroup].Enabled = true;
        m_inputGroups[InputManager.PlacementGroup].Enabled = true;
        m_inputGroups[InputManager.ToolsActionsGroup].Enabled = true;
        m_rotationWithMouseController.Enabled = true;
        m_isInMenu = false;
    }
    **/
}