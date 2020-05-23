//================================================================================
//
//  Score
//
//  スコアのクラス
//
//================================================================================

[System.Serializable]
public class Score{

    /**************************************************
        Fields / Properties
    **************************************************/

    /// <summary>
    /// 移動距離
    /// </summary>
    public float traveledDistance;

    /// <summary>
    /// 獲得金額
    /// </summary>
    public int earnedCurrency;

    /// <summary>
    /// 与ダメージ
    /// </summary>
    public int dealtDamage;

    /// <summary>
    /// 敵撃破数
    /// </summary>
    public int defeatedEnemyCount;

    /**************************************************
        User Defined Functions
    **************************************************/

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public Score(){
        traveledDistance = 0.0f;
        earnedCurrency = 0;
        dealtDamage = 0;
        defeatedEnemyCount = 0;
    }
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="traveledDistance">移動距離</param>
    /// <param name="earnedCurrency">獲得金額</param>
    /// <param name="dealtDamage">与ダメージ</param>
    /// <param name="defeatedEnemyCount">敵撃破数</param>
    public Score(float traveledDistance, int earnedCurrency, int dealtDamage, int defeatedEnemyCount){
        this.traveledDistance = traveledDistance;
        this.earnedCurrency = earnedCurrency;
        this.dealtDamage = dealtDamage;
        this.defeatedEnemyCount = defeatedEnemyCount;
    }

}
