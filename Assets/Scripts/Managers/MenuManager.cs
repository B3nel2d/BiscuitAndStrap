//================================================================================
//
//  MenuManager
//
//  メニュー画面の管理を行う
//
//================================================================================

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    //  Singleton Instance
    //==============================
    
    /// <summary>
    /// クラスのインスタンス
    /// </summary>
    public static MenuManager instance{
        get;
        private set;
    }

    //  Screen
    //==============================

    /// <summary>
    /// 現在表示対象の画面
    /// </summary>
    private GameObject activeScreen{
        get;
        set;
    }

    /// <summary>
    /// 一つ前に表示対象だった画面
    /// </summary>
    private GameObject previousScreen{
        get;
        set;
    }

    /// <summary>
    /// 画面のリスト
    /// </summary>
    private Dictionary<string, GameObject> screens{
        get;
        set;
    }

    /// <summary>
    /// メニュー画面
    /// </summary>
    [field: Header("Screen")]
    [field: SerializeField, RenameField("Menu Screen")]
    private GameObject menuScreen{
        get;
        set;
    }

    /// <summary>
    /// ロードアウト画面
    /// </summary>
    [field: SerializeField, RenameField("Loadout Screen")]
    private GameObject loadoutScreen{
        get;
        set;
    }

    /// <summary>
    /// ストア画面
    /// </summary>
    [field: SerializeField, RenameField("Store Screen")]
    private GameObject storeScreen{
        get;
        set;
    }

    //  Store
    //==============================

    /// <summary>
    /// 武器箱一つ分の値段
    /// </summary>
    [field: Header("Store")]
    [field: SerializeField, RenameField("1 Crate Price")]
    private int oneCratePrice{
        get;
        set;
    }

    /// <summary>
    /// 武器箱五つ分の値段
    /// </summary>
    [field: SerializeField, RenameField("5 Crates Price")]
    private int fiveCratesPrice{
        get;
        set;
    }

    //  Animation
    //==============================

    /// <summary>
    /// オーバーレイのアニメーター
    /// </summary>
    [field: Header("Animation")]
    [field: SerializeField, RenameField("Overlay Animator")]
    private Animator overlayAnimator{
        get;
        set;
    }

    //  Audio
    //==============================

    /// <summary>
    /// オーディオソース
    /// </summary>
    [field: Header("Audio")]
    [field: SerializeField, RenameField("Audio Sources")]
    public List<AudioSource> audioSources{
        get;
        set;
    }

    /// <summary>
    /// ボタンクリック時の効果音
    /// </summary>
    [field: SerializeField, RenameField("Button Click Sound")]
    private AudioClip buttonClickSound{
        get;
        set;
    }

    /// <summary>
    /// アイテム購入時の効果音
    /// </summary>
    [field: SerializeField, RenameField("Purchase Sound")]
    private AudioClip purchaseSound{
        get;
        set;
    }

    //  References
    //==============================

    /// <summary>
    /// ゲームのバージョンのテキスト
    /// </summary>
    [field: Header("References")]
    [field: SerializeField, RenameField("Version Text")]
    private TextMeshProUGUI versionText{
        get;
        set;
    }

    /// <summary>
    /// プレイヤー情報
    /// </summary>
    [field: SerializeField, RenameField("Player Profile")]
    private GameObject playerProfile{
        get;
        set;
    }

    /// <summary>
    /// プレイヤー名のテキスト
    /// </summary>
    [field: SerializeField, RenameField("Player Name Text")]
    private TextMeshProUGUI playerNameText{
        get;
        set;
    }

    /// <summary>
    /// 所持金のテキスト
    /// </summary>
    [field: SerializeField, RenameField("Currency Text")]
    private TextMeshProUGUI currencyText{
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
    /// 獲得総額のテキスト
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

    /// <summary>
    /// オプションウィンドウ
    /// </summary>
    [field: SerializeField, RenameField("Option Window")]
    private GameObject optionWindow{
        get;
        set;
    }

    /// <summary>
    /// メッセージウィンドウ
    /// </summary>
    [field: SerializeField, RenameField("Message Window")]
    private GameObject messageWindow{
        get;
        set;
    }

    /// <summary>
    /// 箱から引いた武器のスタッツ
    /// </summary>
    [field: SerializeField, RenameField("Crate Weapon Stats")]
    private GameObject crateWeaponStats{
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

    /**************************************************
        Unity Event Functions
    **************************************************/

    private void Start(){
        Initialize();
    }

    private void OnApplicationQuit(){
        SaveDataManager.Save();
    }

    /**************************************************
        User Defined Functions
    **************************************************/

    /// <summary>
    /// 初期化処理
    /// </summary>
    private async void Initialize(){
        if(instance == null){
            instance = this;
        }
        else if(instance != this){
            Destroy(gameObject);
        }

        if(GameManager.playerProfile == null){
            GameManager.playerProfile = SaveDataManager.GetClass("Player Profile", new PlayerProfile());
        }
        if(GameManager.playerProfile.weapon == null && InventoryManager.weapons.Count == 0){
            GameManager.playerProfile.weapon = new Weapon();
            InventoryManager.instance.AddItem(GameManager.playerProfile.weapon);
        }

        InventoryManager.instance.SetEquippedWeapon();

        UpdateProfile();
        UpdateHighScore();

        ChangeScreen(menuScreen);

        overlay.SetActive(true);
        overlayAnimator.SetTrigger("FadeOut");

        await Task.Delay(3000);

        overlay.SetActive(false);

        optionWindow.GetComponent<OptionWindowBehaviour>().Initialize();
    }

    /// <summary>
    /// 画面の遷移
    /// </summary>
    public void ChangeScreen(GameObject screen){
        if(screen == null){
            return;
        }

        activeScreen?.SetActive(false);
        previousScreen = activeScreen;

        activeScreen = screen;
        activeScreen?.SetActive(true);

        UpdateScreen(screen);
    }

    /// <summary>
    /// 画面の更新
    /// </summary>
    private void UpdateScreen(GameObject screen){
        if(screen == loadoutScreen){
            InventoryManager.instance.UpdateLoadoutScreen();
        }
    }

    /// <summary>
    /// プレイヤー情報の更新
    /// </summary>
    private void UpdateProfile(){
        playerNameText.text = GameManager.playerProfile.name;
        currencyText.text = String.Format("$ {0:#,0}", GameManager.playerProfile.currecy);
    }

    /// <summary>
    /// ハイスコアの更新
    /// </summary>
    private void UpdateHighScore(){
        traveledDistanceText.text = String.Format("{0:f2}m", GameManager.playerProfile.highScore.traveledDistance);
        earnedCurrencyText.text = "$" + GameManager.playerProfile.highScore.earnedCurrency;
        dealtDamageText.text = GameManager.playerProfile.highScore.dealtDamage.ToString();
        defeatedEnemyText.text = GameManager.playerProfile.highScore.defeatedEnemyCount.ToString();
    }

    /// <summary>
    /// 一つ前の画面への遷移
    /// </summary>
    public void GoBack(){
        ChangeScreen(previousScreen);
    }

    /// <summary>
    /// ゲームの開始
    /// </summary>
    public async void Play(){
        overlay.SetActive(true);
        overlayAnimator.SetTrigger("FadeIn");

        await Task.Delay(3000);
        
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// 武器箱一つの購入
    /// </summary>
    public void BuyOneCrate(){
        if(GameManager.playerProfile.currecy < oneCratePrice){
            ToggleMessageWindow(true);

            messageWindow.GetComponent<MessageWindowBehaviour>().titleText.text = "WARNING";
            messageWindow.GetComponent<MessageWindowBehaviour>().messageText.text = "You don't have\nenough money.";

            PlayAudio(buttonClickSound);

            return;
        }
        if(InventoryManager.instance.weaponCountLimit <= InventoryManager.weapons.Count){
            ToggleMessageWindow(true);

            messageWindow.GetComponent<MessageWindowBehaviour>().titleText.text = "WARNING";
            messageWindow.GetComponent<MessageWindowBehaviour>().messageText.text = "Your inventory is full.";

            PlayAudio(buttonClickSound);

            return;
        }

        GameManager.playerProfile.currecy -= oneCratePrice;
        UpdateProfile();

        List<Weapon> weapons = new List<Weapon>();
        weapons.Add(new Crate(Crate.Rank.Normal, Weapon.Grade.I).Open());
        InventoryManager.instance.AddItem(weapons[0]);

        ToggleCrateWeaponStats(true);
        crateWeaponStats.GetComponent<CrateWeaponStatsBehaviour>().SetWeapons(weapons);
        crateWeaponStats.GetComponent<CrateWeaponStatsBehaviour>().ShowWeaponStats();

        PlayAudio(purchaseSound);
    }

    /// <summary>
    /// 武器箱五つの購入
    /// </summary>
    public void BuyFiveCrates(){
        if(GameManager.playerProfile.currecy < fiveCratesPrice){
            ToggleMessageWindow(true);

            messageWindow.GetComponent<MessageWindowBehaviour>().titleText.text = "WARNING";
            messageWindow.GetComponent<MessageWindowBehaviour>().messageText.text = "You don't have\nenough money.";

            PlayAudio(buttonClickSound);

            return;
        }
        if(InventoryManager.instance.weaponCountLimit <= InventoryManager.weapons.Count){
            ToggleMessageWindow(true);

            messageWindow.GetComponent<MessageWindowBehaviour>().titleText.text = "WARNING";
            messageWindow.GetComponent<MessageWindowBehaviour>().messageText.text = "Your inventory is full.";

            PlayAudio(buttonClickSound);

            return;
        }

        GameManager.playerProfile.currecy -= fiveCratesPrice;
        UpdateProfile();

        List<Weapon> weapons = new List<Weapon>();
        for(int count = 0; count < 5; count++){
            weapons.Add(new Crate(Crate.Rank.Normal, Weapon.Grade.I).Open());
            InventoryManager.instance.AddItem(weapons[count]);
        }

        ToggleCrateWeaponStats(true);
        crateWeaponStats.GetComponent<CrateWeaponStatsBehaviour>().SetWeapons(weapons);
        crateWeaponStats.GetComponent<CrateWeaponStatsBehaviour>().ShowWeaponStats();

        PlayAudio(purchaseSound);
    }

    /// <summary>
    /// オプションウィンドウの表示切替
    /// </summary>
    public void ToggleOptionWindow(bool active){
        overlay.SetActive(active);
        optionWindow.SetActive(active);
    }

    /// <summary>
    /// メッセージウィンドウの表示切替
    /// </summary>
    public void ToggleMessageWindow(bool active){
        overlay.SetActive(active);
        messageWindow.SetActive(active);
    }

    /// <summary>
    /// 武器スタッツの表示切替
    /// </summary>
    public void ToggleCrateWeaponStats(bool active){
        overlay.SetActive(active);
        crateWeaponStats.SetActive(active);
    }

    /// <summary>
    /// 音声の再生
    /// </summary>
    public void PlayAudio(AudioClip audioClip){
        audioSources[1].PlayOneShot(audioClip);
    }

}
