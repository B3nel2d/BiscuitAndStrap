//================================================================================
//
//  UIManager
//
//  ゲーム内UIの管理を行う
//
//================================================================================

using System;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    //  Singleton Instance
    //==============================

    /// <summary>
    /// クラスのインスタンス
    /// </summary>
    public static UIManager instance{
        get;
        private set;
    }

    //  References
    //==============================

    /// <summary>
    /// キャンバス
    /// </summary>
    [field: Header("References")]
    [field: SerializeField, RenameField("Canvas")]
    private GameObject canvas{
        get;
        set;
    }

    /// <summary>
    /// 体力のテキスト
    /// </summary>
    [field: SerializeField, RenameField("Health Text")]
    private TextMeshProUGUI healthText{
        get;
        set;
    }

    /// <summary>
    /// 獲得金額のテキスト
    /// </summary>
    [field: SerializeField, RenameField("Currency Text")]
    private TextMeshProUGUI currencyText{
        get;
        set;
    }

    /// <summary>
    /// 左ボタン
    /// </summary>
    [field: SerializeField, RenameField("Left Action Button")]
    public GameObject leftActionButton{
        get;
        set;
    }

    /// <summary>
    /// 右ボタン
    /// </summary>
    [field: SerializeField, RenameField("Right Action Button")]
    public GameObject rightActionButton{
        get;
        set;
    }

    /// <summary>
    /// オーバーレイ
    /// </summary>
    [field: SerializeField, RenameField("Overlay")]
    private GameObject overlay{
        get;
        set;
    }

    /// <summary>
    /// ポーズメニューウィンドウ
    /// </summary>
    [field: SerializeField, RenameField("Pause Menu Window")]
    private GameObject pauseMenuWindow{
        get;
        set;
    }

    /// <summary>
    /// ゲームオーバー画面
    /// </summary>
    [field: SerializeField, RenameField("Game Over Screen")]
    private GameObject gameOverScreen{
        get;
        set;
    }

    /// <summary>
    /// 総移動距離のテキスト
    /// </summary>
    [field: SerializeField, RenameField("Traveled Distance Text")]
    private TextMeshProUGUI traveledDistanceText{
        get;
        set;
    }

    /// <summary>
    /// 総獲得金額のテキスト
    /// </summary>
    [field: SerializeField, RenameField("Earned Currency Text")]
    private TextMeshProUGUI earnedCurrencyText{
        get;
        set;
    }

    /// <summary>
    /// 総与ダメージのテキスト
    /// </summary>
    [field: SerializeField, RenameField("Dealt Damage Text")]
    private TextMeshProUGUI dealtDamageText{
        get;
        set;
    }

    /// <summary>
    /// 敵の総撃破数のテキスト
    /// </summary>
    [field: SerializeField, RenameField("Defeated Enemy Text")]
    private TextMeshProUGUI defeatedEnemyText{
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

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Initialize(){
        if(instance == null){
            instance = this;
        }
        else if(instance != this){
            Destroy(gameObject);
        }

        UpdateHealthText(3);
        UpdateCurrecyText(0);
    }

    /// <summary>
    /// 体力テキストの更新
    /// </summary>
    public void UpdateHealthText(int amount){
        healthText.text = amount.ToString();
    }

    /// <summary>
    /// 獲得金額テキストの更新
    /// </summary>
    public void UpdateCurrecyText(int amount){
        currencyText.text = String.Format("${0:#,0}", amount);
    }

    /// <summary>
    /// 各スコアテキストの更新
    /// </summary>
    public void UpdateScoreTexts(Score score){
        traveledDistanceText.text = String.Format("{0:f2}m", score.traveledDistance);
        if(GameManager.playerProfile.highScore.traveledDistance < score.traveledDistance){
            traveledDistanceText.color = Color.yellow;
            traveledDistanceText.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        else{
            traveledDistanceText.color = Color.white;
            traveledDistanceText.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }

        earnedCurrencyText.text = "$" + score.earnedCurrency;
        if(GameManager.playerProfile.highScore.earnedCurrency < score.earnedCurrency){
            earnedCurrencyText.color = Color.yellow;
            earnedCurrencyText.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        else{
            earnedCurrencyText.color = Color.white;
            earnedCurrencyText.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }

        dealtDamageText.text = score.dealtDamage.ToString();
        if(GameManager.playerProfile.highScore.dealtDamage < score.dealtDamage){
            dealtDamageText.color = Color.yellow;
            dealtDamageText.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        else{
            dealtDamageText.color = Color.white;
            dealtDamageText.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }

        defeatedEnemyText.text = score.defeatedEnemyCount.ToString();
        if(GameManager.playerProfile.highScore.defeatedEnemyCount < score.defeatedEnemyCount){
            defeatedEnemyText.color = Color.yellow;
            defeatedEnemyText.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        else{
            defeatedEnemyText.color = Color.white;
            defeatedEnemyText.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ゲームオーバー画面の表示
    /// </summary>
    public void ShowGameOverScreen(){
        gameOverScreen.SetActive(true);
    }

    /// <summary>
    /// オーバーレイの表示切替
    /// </summary>
    public void ToggleOverlay(bool active){
        overlay.SetActive(active);
    }

    /// <summary>
    /// ポーズメニューウィンドウの表示切替
    /// </summary>
    public void TogglePauseMenuWindow(bool active){
        GameManager.instance.SwitchPause(active);

        overlay.SetActive(active);
        pauseMenuWindow.SetActive(active);
    }

}
