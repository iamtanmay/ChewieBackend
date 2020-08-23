using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;
using UnityEngine.UI;

public class Inventory : MonoBehaviour 
{
    public uint ID;

    public Transform ManageHub;
    public ItemManager itemManager;
    public BackendInventory backend;
    public MountingSystem mounts;
    public int availableslots = 0;

    Delegate_Backend_CreateItem BackendItemCreate;
    Delegate_Backend_DestroyItem BackendItemDelete;
    Delegate_Backend_GetItem BackendGetItem;
    Delegate_Backend_MoveItem BackendMoveItem;

    Delegate_Backend_GetInventory BackendGetInventory;
    Delegate_Backend_PutInventory BackendPutInventory;
    Delegate_Backend_CreateInventory BackendCreateInventory;
    Delegate_Backend_DeleteInventory BackendDeleteInventory;

    /// <summary>
    /// Add Item via frontend
    /// Moves existing Item to this container
    /// </summary>
    /// <param name="iItem"></param>
    /// <returns></returns>
    public bool AddItem(Item iItem)
    {
        MountSlot freeMount = mounts.FindEmptySlot();

        if (freeMount != null)
        {
            //Move the object to backend Inventory
            BackendMoveItem(iItem.ID_Instance, ID);

            //Put it in the first free slot
            freeMount.Mount(iItem.transform);
            iItem.ID_Inventory = ID;
            availableslots--;
            return true;
        }

        //If there is no slot
        return false;
    }

    /// <summary>
    /// Add object via template ID
    /// Create a new Item and add it
    /// </summary>
    /// <param name="iID"></param>
    /// <returns></returns>
    public bool AddItem(uint iID)
    {
        MountSlot freeMount = mounts.FindEmptySlot();

        if (freeMount != null)
        {
            //Create a new item
            uint instanceID = BackendItemCreate(iID, ID);

            //Get the object template ID
            ItemBackend temp = BackendGetItem(instanceID);

            //Create new Frontend Item from the ItemManager
            Item tItem = itemManager.CreateItem(temp.ID_Template);
            AddItem(tItem);

            return true;
        }

        //If there is no slot
        return false;
    }

    public void Start()
    {
        //Do all the injections
        //TODO: Move this section to the ItemManager
        Backend_GameItems gameItems = ManageHub.GetComponent<Backend_GameItems>();
        Backend_Inventories gameInventories = ManageHub.GetComponent<Backend_Inventories>();
        mounts = transform.GetComponent<MountingSystem>();

        BackendItemCreate = gameItems.CreateItem;
        BackendItemDelete = gameItems.DeleteItem;
        BackendGetItem = gameItems.GetItem;
        BackendMoveItem = gameItems.MoveItem;

        BackendGetInventory = gameInventories.GetContainer;
        BackendPutInventory = gameInventories.PutContainer;
        BackendCreateInventory = gameInventories.CreateContainer;
        BackendDeleteInventory = gameInventories.DeleteContainer;

        ID = BackendCreateInventory();
        backend = BackendGetInventory(ID);
        mounts.InitialiseMountPoints();

        availableslots = mounts.mountSlots.Count;
    }

    public void SyncBackend()
    {
        BackendPutInventory(ID, backend);
    }

    public void SyncFrontend()
    {
        backend = BackendGetInventory(ID);
    }

    /// <summary>
    /// Move ItemBackend to new container
    /// Fix Item as child in container
    /// </summary>
    /// <param name="gameItem"></param>
    /// <returns></returns>
    public void Put(Item gameItem)
    {
        AddItem(gameItem);
    }


    //public CraftCharacterView m_view;
    //public CraftCharacterController m_controller;

    //List<Cell> m_cells;//backpack
    //List<Cell> m_equipmentCells;
    //public List<Cell> m_craftCells { get; private set; }//cell in crafter
    //public List<string> m_prefabsWithReceipt { get; set; }//all craftable items

    ////containers
    //public GameObject m_backpack;
    //public GameObject m_tools;
    //public GameObject m_equipment;
    //public GameObject m_crafter;

    ////cellOnCursor
    //public CellInfo m_cellOnCursor;

    //Cell m_selectedTool;

    //public bool Put(string it) // put item to backpack
    //{
    //    int index;
    //    if ((index = SearchSlot(it)) != -1)
    //    {
    //        m_cells[index].Add(it);
    //        if (index < m_toolCount)//if this cell is from tools panel -> call event handler
    //            m_controller.SelectedToolChangedHandler();
    //        return true;
    //    }
    //    else
    //        return false;
    //}

    //public int m_toolCount { get; private set; }

    //public bool m_isItPossibleToGoToGame// to go to game possible in case when cursor is not holding items and in crafter are not items
    //{
    //    get
    //    {
    //        if (m_cellOnCursor != null)
    //            return false;
    //        foreach (Cell c in m_craftCells)
    //            if (c.m_item != null)
    //                return false;
    //        return true;
    //    }
    //}

    //public Cell m_selectedToolCell //cell selected on the tools panel at the bottom of the screen
    //{
    //    get
    //    {
    //        return m_selectedTool;
    //    }
    //    set
    //    {
    //        m_selectedTool = value;
    //        m_view.SelectedToolChangedHandler(value);
    //    }
    //}

    //public Cell GetCellOnIndex(int index)//get concrete cell via index
    //{
    //    return m_cells[index];
    //}

    //string m_cellOnCursorItem
    //{
    //    get { return m_cellOnCursor.m_item; }
    //    set { m_cellOnCursor.m_item = value; if (value == null) m_cellOnCursor = null; }
    //}
    //int m_cellOnCursorCount
    //{
    //    get { return m_cellOnCursor == null ? 0 : m_cellOnCursor.m_count; }
    //    set { m_cellOnCursor.m_count = value; if (value == 0) m_cellOnCursorItem = null; }
    //}


    //#region Cell-Cursor Actions
    //public void RecountToCell(Cell cell)
    //{
    //    RecountToCell(cell, m_cellOnCursorCount);
    //}
    //public void RecountToCell(Cell cell, int count)//get <count> items from cursor and bind that to cell
    //{
    //    if (cell.m_item == null)
    //        cell.m_item = m_cellOnCursorItem;
    //    cell.m_count += count;
    //    m_cellOnCursorCount -= count;
    //    m_view.CursorStateChangedHandler(m_cellOnCursor);
    //    m_view.CellCountChangedHandler(cell);
    //    if (cell == m_selectedToolCell)
    //        m_controller.SelectedToolChangedHandler();
    //}
    //public void MoveItemsFromCellToCursor(Cell cell)
    //{
    //    MoveItemsFromCellToCursor(cell, cell.m_count);
    //}
    //public void MoveItemsFromCellToCursor(Cell cell, int count)
    //{
    //    m_cellOnCursor = new CellInfo { m_count = count, m_item = cell.m_item };
    //    cell.m_count -= count;

    //    //refresh view
    //    m_view.CursorStateChangedHandler(m_cellOnCursor);
    //    m_view.CellCountChangedHandler(cell);

    //    if (cell == m_selectedToolCell)
    //        m_controller.SelectedToolChangedHandler();
    //}
    //public void SwapCursorAndCellItems(Cell cell)
    //{
    //    CellInfo temp = m_cellOnCursor;
    //    m_cellOnCursor = new CellInfo { m_item = cell.m_item };
    //    m_cellOnCursorCount = cell.m_count;
    //    cell.m_item = temp.m_item;
    //    cell.m_count = temp.m_count;

    //    m_view.CellCountChangedHandler(cell);
    //    m_view.CursorStateChangedHandler(m_cellOnCursor);

    //    if (cell == m_selectedToolCell)
    //        m_controller.SelectedToolChangedHandler();
    //}
    //public void MoveItemsFromCursorToCell(Cell cell)
    //{
    //    cell.Add(m_cellOnCursorItem);
    //    cell.m_count = m_cellOnCursorCount;
    //    m_cellOnCursor = null;

    //    m_view.CursorStateChangedHandler(m_cellOnCursor);
    //    m_view.CellCountChangedHandler(cell);

    //    if (cell == m_selectedToolCell)
    //        m_controller.SelectedToolChangedHandler();
    //}
    //public bool AreCompatibleArmorAndCellOnCursor(ArmorType type)
    //{
    //    if (m_cellOnCursor == null)
    //        return true;
    //    Armor cursorArmor = Resources.Load<GameObject>("Prefabs/" + m_cellOnCursor.m_item.m_prefab).GetComponent<Armor>();
    //    return cursorArmor != null && cursorArmor.m_armorType == type;
    //}
    //#endregion

    //void InitializeCell(CellRenderer renderer, List<Cell> list)
    //{
    //    //just add cell renderer to concrete list
    //    Cell cell = new Cell
    //    {
    //        m_view = this.m_view,
    //        m_renderer = renderer,
    //        m_item = null            
    //    };
    //    list.Add(cell);
    //    renderer.m_cell = cell;
    //}

    //public void Update()
    //{        
    //}

    //void Awake()
    //{
    //    //initialize lists
    //    m_cells = new List<Cell>();
    //    m_equipmentCells = new List<Cell>();
    //    m_craftCells = new List<Cell>();
    //    m_prefabsWithReceipt = new List<string>();

    //    m_cellOnCursor = null;

    //    //fill lists with cells

    //    foreach (CellRenderer child in m_tools.GetComponentsInChildren<CellRenderer>())
    //    {
    //        InitializeCell(child, m_cells);
    //    }
    //    m_toolCount = m_cells.Count;
    //    foreach (CellRenderer child in m_backpack.GetComponentsInChildren<CellRenderer>())
    //    {
    //        InitializeCell(child, m_cells);
    //    }
    //    foreach(CellArmor child in m_equipment.GetComponentsInChildren<CellArmor>())
    //    {
    //        InitializeCell(child, m_equipmentCells);
    //    }
    //    foreach(CellCraft child in m_crafter.GetComponentsInChildren<CellCraft>())
    //    {
    //        InitializeCell(child, m_craftCells);
    //    }
    //    m_prefabsWithReceipt = new List<string>(Resources.LoadAll<string>("Items/")).FindAll(prefab => prefab.m_receiptItems.Length != 0);
    //}
    //int SearchSlot(string it)//search suitable cell to put item in
    //{
    //    for(int i = 0; i < m_cells.Count; ++i)
    //    {
    //        if (m_cells[i].m_item == null || m_cells[i].m_item.Equals(it) && m_cells[i].m_count < m_cells[i].m_item.m_maxCount)
    //        {
    //            return i;
    //        }
    //    }
    //    return -1;
    //}
    //public Cell SearchCellInCrafterByItemDescription(string element, List<Cell>selectedItems)
    //{
    //    foreach (Cell cell in m_craftCells)
    //    {
    //        if (!selectedItems.Contains(cell) && cell.m_item == element)
    //        {
    //            return cell;
    //        }
    //    }
    //    return null;
    //}    
}

//public class Cell
//{
//    string item;
//    public string m_item
//    {
//        get
//        {
//            return item;
//        }
//        set
//        {
//            item = value;
//            m_view.CellItemChangedHandler(this);
//        }
//    }
//    public CraftCharacterView m_view;
//    public CellRenderer m_renderer { get; set; }

//    int count;
//    public int m_count
//    {
//        get { return count; }
//        set
//        {
//            count = value;
//            if (count == 0)
//                m_item = null;
//        }
//    }
//    public void Add(string it)
//    {
//        if (m_item == null)
//        {
//            m_item = it;
//            m_count = 1;
//        }
//        else
//        {
//            ++m_count;
//        }
//        m_view.CellCountChangedHandler(this);
//    }
//}