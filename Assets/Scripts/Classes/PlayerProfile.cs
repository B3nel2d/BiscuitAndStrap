//================================================================================
//
//  PlayerProfile
//
//  プレイヤー情報のクラス
//
//================================================================================

[System.Serializable]
public class PlayerProfile{

    /**************************************************
        Fields / Properties
    **************************************************/

    /// <summary>
    /// 名前
    /// </summary>
    public string name;

    /// <summary>
    /// 所持金
    /// </summary>
    public int currecy;

    /// <summary>
    /// 所持金のカウント最大値
    /// </summary>
    public const int maximumCurrencyLimit = 99999999;

    /// <summary>
    /// ゲーム開始時の所持金
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
    /// コンストラクタ
    /// </summary>
    public PlayerProfile(){
        name = "Player";
        currecy = initialCurrency;
        weapon = null;
        highScore = new Score();
    }

}
