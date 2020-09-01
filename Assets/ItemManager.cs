using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Comprehensive Item management
public class ItemManager : MonoBehaviour
{
    //Backend
    Backend backend;

    //Item pools
    public List<Item> templates;
    public List<uint> poolSizes;
    Dictionary<uint, List<Item>> pools;
    Dictionary<uint, Item> templateDictionary;

    // Start is called before the first frame update
    void Start()
    {
        templateDictionary = new Dictionary<uint, Item>();

        for (int i = 0; i < templates.Count; i++)
            templateDictionary.Add(templates[i].ID_Template, templates[i]);
    }

    /// <summary>
    /// TODO: Replace with object pool
    /// </summary>
    /// <param name="templateID"></param>
    /// <returns></returns>
    public Item CreateItem(uint templateID)
    {
        Item newItem = GameObject.Instantiate<Item>(templateDictionary[templateID]);
        return newItem;
    }

    //public Item GetItem(uint instanceID)
    //{
    //    Item newItem = GameObject.Instantiate<Item>(templateDictionary[templateID]);
    //    return newItem;
    //}
}
