//================================================================================
//
//  CharacerBehaviour
//
//  キャラクターの基底クラス
//
//================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehavior : RigidbodyBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    /// <summary>
    /// 体力の最大値
    /// </summary>
    [field: Header("Stats")]
    [field: SerializeField, RenameField("Maximum Health")]
    protected int maximumHealth{
        get;
        set;
    }

    /// <summary>
    /// 現在の体力
    /// </summary>
    [field: SerializeField, RenameField("Current Health")]
    private int currentHealth_field;
    protected int currentHealth{
        get{
            return currentHealth_field;
        }
        set{
            if(value <= maximumHealth){
                currentHealth_field = value;
            }
            else{
                currentHealth = maximumHealth;
            }
        }
    }

    /**************************************************
        User Defined Functions
    **************************************************/

    /// <summary>
    /// 初期化処理
    /// </summary>
    override protected void Initialize(){
        base.Initialize();

        currentHealth = maximumHealth;
    }

    /// <summary>
    /// 受けたダメージの計算
    /// </summary>
    virtual public void TakeDamage(int damage, Vector3 direction){
        currentHealth -= damage;

        if(currentHealth <= 0){
            currentHealth = 0;
            Down();
        }
    }
    /// <summary>
    /// 受けたダメージの計算
    /// </summary>
    virtual public void TakeDamage(int damage){
        TakeDamage(damage, Vector3.zero);
    }

    /// <summary>
    /// 撃破された際の処理
    /// </summary>
    virtual protected void Down(){
        Destroy(transform.gameObject);
    }

}
