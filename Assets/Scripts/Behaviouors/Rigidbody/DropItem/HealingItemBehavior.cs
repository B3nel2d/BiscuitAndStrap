//================================================================================
//
//  HealingItemBehaviour
//
//  回復アイテムの挙動
//
//================================================================================

using UnityEngine;

class HealingItemBehavior : DropItemBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    /// <summary>
    /// 回復ポップアップエフェクトのプレハブ
    /// </summary>
    [field: SerializeField, RenameField("Health Popup Prefab")]
    private GameObject healthPopupPrefab{
        get;
        set;
    }

    /**************************************************
        User Defined Functions
    **************************************************/

    /// <summary>
    /// 効果の付与
    /// </summary>
    /// <param name="target">付与対象</param>
    protected override void GiveEffect(PlayerBehaviour target){
        target.GetHealed(1);

        GameObject healthPopup = Instantiate(healthPopupPrefab, target.gameObject.transform.position, Quaternion.identity, target.gameObject.transform.parent);
        healthPopup.GetComponent<ImagePopupBehaviour>().Initialize();

        base.GiveEffect(target);
    }

}
