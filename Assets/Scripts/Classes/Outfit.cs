using UnityEngine;

[System.Serializable]
public class Outfit : Item{

    /**************************************************
        Enumerations
    **************************************************/

    public enum Rarity{
        Common,
        Rare,
        Epic
    }

    /**************************************************
        Fields / Properties
    **************************************************/

    public Rarity rarity{
        get;
        set;
    }

}
