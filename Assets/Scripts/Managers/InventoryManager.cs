//================================================================================
//
//  InventoryManager
//
//  アイテムやインベントリの管理を行う
//
//================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    //  Singleton Instance
    //==============================
    
    /// <summary>
    /// クラスのインスタンス
    /// </summary>
    public static InventoryManager instance{
        get;
        private set;
    }

    //  Inventories
    //==============================

    /// <summary>
    /// 武器のリスト
    /// </summary>
    public static List<Weapon> weapons{
        get;
        private set;
    }

    /// <summary>
    /// 武器数の上限
    /// </summary>
    [field: Header("Inventories")]
    [field: SerializeField, RenameField("Weapon Count Limit")]
    public int weaponCountLimit{
        get;
        private set;
    }

    /// <summary>
    /// スキンのリスト
    /// </summary>
    public static List<Outfit> outfits{
        get;
        private set;
    }

    /// <summary>
    /// スキン数の上限
    /// </summary>
    [field: SerializeField, RenameField("Outfit Count Limit")]
    public int outfitCountLimit{
        get;
        private set;
    }

    /// <summary>
    /// 選択したアイテム
    /// </summary>
    public Item selectedItem{
        get;
        set;
    }

    //  Animation
    //==============================

    /// <summary>
    /// 武器のアニメーター
    /// </summary>
    [field: Header("Animation")]
    [field: SerializeField, RenameField("Weapon Image Animator")]
    private Animator weaponImageAnimator{
        get;
        set;
    }

    //  References
    //==============================

    /// <summary>
    /// 武器スタッツの背景
    /// </summary>
    [field: Header("References")]
    [field: SerializeField, RenameField("Stats Background Image")]
    private Image statsBackgroundImage{
        get;
        set;
    }

    /// <summary>
    /// 装備済みサイン
    /// </summary>
    [field: SerializeField, RenameField("Equipped Sign")]
    private GameObject equippedSign{
        get;
        set;
    }

    /// <summary>
    /// 武器ランクのテキスト
    /// </summary>
    [field: SerializeField, RenameField("Rank Text")]
    private TextMeshProUGUI rankText{
        get;
        set;
    }

    /// <summary>
    /// スコアのテキスト
    /// </summary>
    [field: SerializeField, RenameField("Score Text")]
    private TextMeshProUGUI scoreText{
        get;
        set;
    }

    /// <summary>
    /// ダメージのテキスト
    /// </summary>
    [field: SerializeField, RenameField("Damage Text")]
    private TextMeshProUGUI damageText{
        get;
        set;
    }

    /// <summary>
    /// ファイアレートのテキスト
    /// </summary>
    [field: SerializeField, RenameField("Fire Rate Text")]
    private TextMeshProUGUI fireRateText{
        get;
        set;
    }

    /// <summary>
    /// 弾速のテキスト
    /// </summary>
    [field: SerializeField, RenameField("Bullet Speed Text")]
    private TextMeshProUGUI bulletSpeedText{
        get;
        set;
    }

    /// <summary>
    /// クリティカル率のテキスト
    /// </summary>
    [field: SerializeField, RenameField("Critical Chance Text")]
    private TextMeshProUGUI criticalChanceText{
        get;
        set;
    }

    /// <summary>
    /// 武器スタッツ背景のスプライトリスト
    /// </summary>
    [field: SerializeField, RenameField("Stats Background Sprites")]
    private List<Sprite> statsBackgroundSprites{
        get;
        set;
    }

    /// <summary>
    /// インベントリのインデックスのテキスト
    /// </summary>
    [field: SerializeField, RenameField("Index Number Text")]
    private TextMeshProUGUI indexNumberText{
        get;
        set;
    }

    /// <summary>
    /// 装備ボタン
    /// </summary>
    [field: SerializeField, RenameField("Equip Button")]
    private Button equipButton{
        get;
        set;
    }

    /// <summary>
    /// 売却ボタン
    /// </summary>
    [field: SerializeField, RenameField("Sell Button")]
    private Button sellButton{
        get;
        set;
    }

    //  Prefabs
    //==============================

    /// <summary>
    /// アイテムのプレハブ
    /// </summary>
    [field: Header("Prefabs")]
    [field: SerializeField, RenameField("Item Prefab")]
    private GameObject itemPrefab{
        get;
        set;
    }

    /**************************************************
        Unity Event Functions
    **************************************************/

    private void Start(){
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

        if(weapons == null){
            weapons = SaveDataManager.GetList("Inventory Weapons", new List<Weapon>());
        }
        if(outfits == null){
            outfits = SaveDataManager.GetList("Inventory Outfits", new List<Outfit>());
        }
    }

    /// <summary>
    /// アイテムの追加
    /// </summary>
    public void AddItem(Weapon weapon){
        weapons.Add(weapon);
        SaveDataManager.SetList("Inventory Weapons", weapons);
    }
    /// <summary>
    /// アイテムの追加
    /// </summary>
    public void AddItem(Outfit outfit){
        outfits.Add(outfit);
        SaveDataManager.SetList("Inventory Outfits", outfits);
    }

    /// <summary>
    /// 武器スタッツの表示
    /// </summary>
    public void ShowWeaponStats(){
        statsBackgroundImage.sprite = statsBackgroundSprites[((int)((Weapon)selectedItem).rank)];

        rankText.text = ((Weapon)selectedItem).rank.ToString().ToUpper();
        scoreText.text = "Score: " + ((Weapon)selectedItem).score;

        damageText.text = ((Weapon)selectedItem).projectileDamage.ToString();
        fireRateText.text = String.Format("{0:#.##}/s", (1.0f / ((Weapon)selectedItem).shotDelay));
        bulletSpeedText.text = String.Format("{0:#.##}/s", (((Weapon)selectedItem).projectileSpeed * 30.0f));
        criticalChanceText.text = String.Format("{0:##.##}%", (((Weapon)selectedItem).criticalChance) * 100.0f);
        
        if(weaponCountLimit < weapons.Count){
            indexNumberText.text = (weapons.IndexOf((Weapon)selectedItem) + 1) + "/<color=red>" + weapons.Count + "</color>";
        }
        else{
            indexNumberText.text = (weapons.IndexOf((Weapon)selectedItem) + 1) + "/" + weapons.Count;
        }
    }

    /// <summary>
    /// リスト内の次のアイテムの表示
    /// </summary>
    public void ShowNextWeaponStats(){
        int weaponNumber = 0;
        if(weapons.Contains((Weapon)selectedItem)){
            weaponNumber = weapons.IndexOf((Weapon)selectedItem);
        }

        weaponNumber++;
        if(weapons.Count <= weaponNumber){
            weaponNumber = 0;
        }

        selectedItem = weapons[weaponNumber];
        ShowWeaponStats();

        SetButtonStates();

        weaponImageAnimator.SetTrigger("LeftSlide");
    }

    /// <summary>
    /// リスト内の前のアイテムの表示
    /// </summary>
    public void ShowPreviousWeaponStats(){
        int weaponNumber = 0;
        if(weapons.Contains((Weapon)selectedItem)){
            weaponNumber = weapons.IndexOf((Weapon)selectedItem);
        }

        weaponNumber--;
        if(weaponNumber < 0){
            weaponNumber = weapons.Count - 1;
        }

        selectedItem = weapons[weaponNumber];
        ShowWeaponStats();

        SetButtonStates();

        weaponImageAnimator.SetTrigger("RightSlide");
    }

    /// <summary>
    /// 各ボタンの状態の更新
    /// </summary>
    public void SetButtonStates(){
        if(selectedItem == GameManager.playerProfile.weapon){
            equipButton.interactable = false;
            sellButton.interactable = false;
            equippedSign.SetActive(true);
        }
        else{
            equipButton.interactable = true;
            equippedSign.SetActive(false);

            if(weapons.Count <= 1){
                sellButton.interactable = false;
            }
            else{
                sellButton.interactable = true;
            }
        }
    }

    /// <summary>
    /// アイテムの装備
    /// </summary>
    public void EquipItem(){
        if(selectedItem == null){
            return;
        }

        if(selectedItem.GetType() == typeof(Weapon)){
            GameManager.playerProfile.weapon = (Weapon)selectedItem;
        }
        else if(selectedItem.GetType() == typeof(Outfit)){
            //GameManager.playerProfile.outfit = (Outfit)selectedItem;
        }

        equippedSign.SetActive(true);

        SaveDataManager.SetClass("Player Profile", GameManager.playerProfile);

        SetButtonStates();
    }

    /// <summary>
    /// アイテムの売却
    /// </summary>
    public void SellItem(){
        if(selectedItem == null){
            return;
        }
        if(selectedItem == GameManager.playerProfile.weapon){
            return;
        }

        RemoveItem();

        int weaponNumber = 0;
        if(weapons.Contains((Weapon)selectedItem)){
            weaponNumber = weapons.IndexOf((Weapon)selectedItem);
        }

        weaponNumber--;
        if(weaponNumber < 0){
            weaponNumber = weapons.Count - 1;
        }

        selectedItem = weapons[weaponNumber];
        ShowWeaponStats();

        SetButtonStates();
    }

    /// <summary>
    /// アイテムの削除
    /// </summary>
    public void RemoveItem(){
        if(weapons.Contains((Weapon)selectedItem)){
            weapons.Remove((Weapon)selectedItem);
            SaveDataManager.SetList("Inventory Weapons", weapons);
        }
        else if(!outfits.Contains((Outfit)selectedItem)){
            outfits.Remove((Outfit)selectedItem);
            SaveDataManager.SetList("Inventory Outfits", outfits);
        }
    }

    /// <summary>
    /// 装備した武器をプロフィールに設定する
    /// </summary>
    public void SetEquippedWeapon(){
        if(weapons.Find(weapon => weapon == GameManager.playerProfile.weapon) == null){
            selectedItem = weapons.Find(target => target.id == GameManager.playerProfile.weapon.id);
            GameManager.playerProfile.weapon = (Weapon)selectedItem;
        }
    }

    /// <summary>
    /// ロードアウト画面の更新
    /// </summary>
    public void UpdateLoadoutScreen(){
        selectedItem = GameManager.playerProfile.weapon;

        ShowWeaponStats();
        SetButtonStates();
    }

}
