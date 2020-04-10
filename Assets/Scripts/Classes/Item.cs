//================================================================================
//
//  Item
//
//  インベントリアイテムの規定クラス
//
//================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item{

    /**************************************************
        Fields / Properties
    **************************************************/

    [field: SerializeField, RenameField("Name")]
    public string name;

}
