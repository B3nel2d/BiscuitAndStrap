//================================================================================
//
//  HealingItemBehaviour
//
//  回復アイテムの挙動
//
//================================================================================

using System;
using UnityEngine;

class HealingItemBehavior : DropItemBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    [field: SerializeField, RenameField("Health Popup Prefab")]
    private GameObject healthPopupPrefab{
        get;
        set;
    }

    /**************************************************
        User Defined Functions
    **************************************************/

    protected override void GiveEffect(PlayerBehaviour target){
        target.GetHealed(1);

        GameObject healthPopup = Instantiate(healthPopupPrefab, target.gameObject.transform.position, Quaternion.identity, target.gameObject.transform.parent);
        healthPopup.GetComponent<ImagePopupBehaviour>().Initialize();

        base.GiveEffect(target);
    }

}
