//================================================================================
//
//  Score
//
//  スコアのクラス
//
//================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Score{

    /**************************************************
        Fields / Properties
    **************************************************/

    /// <summary>
    /// 総移動距離
    /// </summary>
    public float traveledDistance;
    /// <summary>
    /// 総獲得金額
    /// </summary>
    public int earnedCurrency;
    /// <summary>
    /// 総与ダメージ
    /// </summary>
    public int dealtDamage;
    /// <summary>
    /// 敵の総撃破数
    /// </summary>
    public int defeatedEnemyCount;

    /**************************************************
        User Defined Functions
    **************************************************/

    /// <summary>
    /// コンストラクター
    /// </summary>
    public Score(){
        traveledDistance = 0.0f;
        earnedCurrency = 0;
        dealtDamage = 0;
        defeatedEnemyCount = 0;
    }
    /// <summary>
    /// コンストラクター
    /// </summary>
    public Score(float traveledDistance, int earnedCurrency, int dealtDamage, int defeatedEnemyCount){
        this.traveledDistance = traveledDistance;
        this.earnedCurrency = earnedCurrency;
        this.dealtDamage = dealtDamage;
        this.defeatedEnemyCount = defeatedEnemyCount;
    }

}
