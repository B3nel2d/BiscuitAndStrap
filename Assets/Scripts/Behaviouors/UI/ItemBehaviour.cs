//================================================================================
//
//  ItemBehaviour
//
//  インベントリアイテムの挙動
//
//================================================================================

using UnityEngine;

public class ItemBehaviour : MonoBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/
    
    /// <summary>
    /// アイテム
    /// </summary>
    public Item item{
        get;
        set;
    }

    /**************************************************
        User Defined Functions
    **************************************************/

    /// <summary>
    /// 選択
    /// </summary>
    public void Select(){
        InventoryManager.instance.selectedItem = item;
        InventoryManager.instance.ShowWeaponStats();
    }

}
