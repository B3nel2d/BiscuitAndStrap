//================================================================================
//
//  ItemBehaviour
//
//  インベントリアイテムの挙動
//
//================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/
    
    public Item item{
        get;
        set;
    }

    /**************************************************
        Unity Event Functions
    **************************************************/

    private void Awake(){
        Initialize();
    }

    /**************************************************
        User Defined Functions
    **************************************************/

    private void Initialize(){

    }

    public void Select(){
        InventoryManager.instance.selectedItem = item;
        InventoryManager.instance.ShowWeaponStats();
    }

}
