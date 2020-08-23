using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlexiblePlayerInput : MonoBehaviour
{
    //Axes are two dials on bottom left and right
    public string axisL_1, axisL_2, axisR_1, axisR_2, axisScroll;

    //Character is Playermenu - Items, Magic, Skills, Journal - Small button on left bottom
    //Favorites is Map, Favorites, Statistics - Small button on Right bottom
    //Menu is a small button top center for MainMenuww
    //A, B are buttons on bottom Left, C, D are buttons on bottom Right
    public KeyCode fire1, fire2, character, favorites, menu, p1_A, p1_B, p1_C, p1_D;

    public Dictionary<string, InputUnit> inputs { get; private set; }

    public const string Input_Axis11 = "Axis11";
    public const string Input_Axis12 = "Axis12";
    public const string Input_Axis21 = "Axis21";
    public const string Input_Axis22 = "Axis22";
    public const string Input_AxisScroll = "AxisScroll";
    public const string Input_Fire1 = "Fire1";
    public const string Input_Fire2 = "Fire2";
    public const string Input_Fire3 = "Fire3";
    public const string Input_Character = "Character";
    public const string Input_Favorites = "Favorites";
    public const string Input_Menu = "Menu";
    public const string Input_A = "A";
    public const string Input_B = "B";
    public const string Input_C = "C";
    public const string Input_D = "D";

    public const string Inventory = "Inventory";
    public const string Escape = "Escape";
    public const string Interaction = "Interaction";
    public const string InteractionHit = "Hit";
    public const string SelectToolWithScroll = "Select tool by scroll";
    public const string DropItem = "Drop item";
    public const string PlaceObject = "Place";
    public const string RotateObject = "Rotate";

    public void Awake()
    {
        inputs = new Dictionary<string, InputUnit>();

        InputUnit unit = new InputUnit(axisL_1);
        inputs.Add(Input_Axis11, unit);

        unit = new InputUnit(axisL_2);
        inputs.Add(Input_Axis12, unit);

        unit = new InputUnit(axisR_1);
        inputs.Add(Input_Axis21, unit);

        unit = new InputUnit(axisR_2);
        inputs.Add(Input_Axis22, unit);

        unit = new InputUnit(axisScroll);
        inputs.Add(Input_AxisScroll, unit);        

        unit = new InputUnit(fire1);
        inputs.Add(Input_Fire1, unit);
        unit = new InputUnit(fire2);
        inputs.Add(Input_Fire2, unit);
        unit = new InputUnit(character);
        inputs.Add(Input_Character, unit);
        unit = new InputUnit(favorites);
        inputs.Add(Input_Favorites, unit);
        unit = new InputUnit(menu);
        inputs.Add(Input_Menu, unit);
        unit = new InputUnit(p1_A);
        inputs.Add(Input_A, unit);
        unit = new InputUnit(p1_B);
        inputs.Add(Input_B, unit);
        unit = new InputUnit(p1_C);
        inputs.Add(Input_C, unit);
        unit = new InputUnit(p1_D);
        inputs.Add(Input_D, unit);
    }

    //For on Input Events
    public void CheckInputEvents()
    {
        foreach (InputUnit unit in inputs.Values)
            unit.Update();
    }
}