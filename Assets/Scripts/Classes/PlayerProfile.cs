//================================================================================
//
//  PlayerProfile
//
//  プレイヤー情報のクラス
//
//================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProfile{

    /**************************************************
        Fields / Properties
    **************************************************/

    /// <summary>
    /// プレイヤー名
    /// </summary>
    public string name;

    /// <summary>
    /// 所持金
    /// </summary>
    public int currecy;
    /// <summary>
    /// 最大所持金
    /// </summary>
    public const int maximumCurrencyLimit = 99999999;
    /// <summary>
    /// 初期所持金
    /// </summary>
    private const int initialCurrency = 3000;

    /// <summary>
    /// 装備中の武器
    /// </summary>
    public Weapon weapon;

    /// <summary>
    /// ハイスコア
    /// </summary>
    public Score highScore;

    /**************************************************
        User Defined Functions
    **************************************************/

    /// <summary>
    /// コンストラクター
    /// </summary>
    public PlayerProfile(){
        name = "Player";
        currecy = initialCurrency;
        weapon = null;
        highScore = new Score();
    }

}
