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
    /// 武器の最大所持数
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
    /// スキンの最大所持数
    /// </summary>
    [field: SerializeField, RenameField("Outfit Count Limit")]
    public int outfitCountLimit{
        get;
        private set;
    }

    /// <summary>
    /// 選択中のアイテム
    /// </summary>
    public Item selectedItem{
        get;
        set;
    }

    //  Animation
    //==============================

    /// <summary>
    /// 武器画像のアニメーター
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
    /// ステータス背景の画像
    /// </summary>
    [field: Header("References")]
    [field: SerializeField, RenameField("Stats Background Image")]
    private Image statsBackgroundImage{
        get;
        set;
    }

    /// <summary>
    /// 装備済みであるかの表示
    /// </summary>
    [field: SerializeField, RenameField("Equipped Sign")]
    private GameObject equippedSign{
        get;
        set;
    }

    /// <summary>
    /// 武器のランクのテキスト
    /// </summary>
    [field: SerializeField, RenameField("Rank Text")]
    private TextMeshProUGUI rankText{
        get;
        set;
    }

    /// <summary>
    /// 武器のスコアのテキスト
    /// </summary>
    [field: SerializeField, RenameField("Score Text")]
    private TextMeshProUGUI scoreText{
        get;
        set;
    }

    /// <summary>
    /// 武器の与ダメージのテキスト
    /// </summary>
    [field: SerializeField, RenameField("Damage Text")]
    private TextMeshProUGUI damageText{
        get;
        set;
    }

    /// <summary>
    /// 武器の発射レートのテキスト
    /// </summary>
    [field: SerializeField, RenameField("Fire Rate Text")]
    private TextMeshProUGUI fireRateText{
        get;
        set;
    }

    /// <summary>
    /// 武器の弾速のテキスト
    /// </summary>
    [field: SerializeField, RenameField("Bullet Speed Text")]
    private TextMeshProUGUI bulletSpeedText{
        get;
        set;
    }

    /// <summary>
    /// 武器のクリティカル発生確率のテキスト
    /// </summary>
    [field: SerializeField, RenameField("Critical Chance Text")]
    private TextMeshProUGUI criticalChanceText{
        get;
        set;
    }

    /// <summary>
    /// 武器のステータス背景の画像
    /// </summary>
    [field: SerializeField, RenameField("Stats Background Sprites")]
    private List<Sprite> statsBackgroundSprites{
        get;
        set;
    }

    /// <summary>
    /// インベントリの添え字のテキスト
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
    /// <param name="weapon">追加する武器</param>
    public void AddItem(Weapon weapon){
        weapons.Add(weapon);
        SaveDataManager.SetList("Inventory Weapons", weapons);
    }
    /// <summary>
    /// アイテムの追加
    /// </summary>
    /// <param name="outfit">追加するスキン</param>
    public void AddItem(Outfit outfit){
        outfits.Add(outfit);
        SaveDataManager.SetList("Inventory Outfits", outfits);
    }

    /// <summary>
    /// 武器ステータスの表示
    /// </summary>
    public void ShowWeaponStats(){
        statsBackgroundImage.sprite = statsBackgroundSprites[((int)((Weapon)selectedItem).rank)];

        rankText.text = ((Weapon)selectedItem).rank.ToString().ToUpper();
        scoreText.text = "Score: " + ((Weapon)selectedItem).score;

        damageText.text = ((Weapon)selectedItem).projectileDamage.ToString();
        fireRateText.text = String.Format("{0:f2}/s", (1.0f / ((Weapon)selectedItem).shotDelay));
        bulletSpeedText.text = String.Format("{0:f2}/s", (((Weapon)selectedItem).projectileSpeed * 30.0f));
        criticalChanceText.text = String.Format("{0:f2}%", (((Weapon)selectedItem).criticalChance) * 100.0f);
        
        if(weaponCountLimit < weapons.Count){
            indexNumberText.text = (weapons.IndexOf((Weapon)selectedItem) + 1) + "/<color=red>" + weapons.Count + "</color>";
        }
        else{
            indexNumberText.text = (weapons.IndexOf((Weapon)selectedItem) + 1) + "/" + weapons.Count;
        }
    }

    /// <summary>
    /// リスト内の次の武器ステータスの表示
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
    /// リスト内の前の武器ステータスの表示
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
    /// 武器のソート(スコア降順)
    /// </summary>
    public void SortWeaponsByScore() {
        weapons.Sort((a, b) => b.score - a.score);
        ShowWeaponStats();
    }

    /// <summary>
    /// 武器のソート(与ダメージ降順)
    /// </summary>
    public void SortWeaponsByDamage() {
        weapons.Sort((a, b) => b.projectileDamage - a.projectileDamage);
        ShowWeaponStats();
    }

    /// <summary>
    /// 武器のソート(発射レート降順)
    /// </summary>
    public void SortWeaponsByFireRate() {
        weapons.Sort((a, b) =>Math.Sign(a.shotDelay - b.shotDelay));
        ShowWeaponStats();
    }

    /// <summary>
    /// 武器のソート(弾速降順)
    /// </summary>
    public void SortWeaponsByBulletSpeed() {
        weapons.Sort((a, b) => Math.Sign(b.projectileSpeed - a.projectileSpeed));
        ShowWeaponStats();
    }

    /// <summary>
    /// 武器のソート(クリティカル発生確率降順)
    /// </summary>
    public void SortWeaponsByCriticalChance() {
        weapons.Sort((a, b) => Math.Sign(b.criticalChance - a.criticalChance));
        ShowWeaponStats();
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
    /// 選択中のアイテムの装備
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
        SaveDataManager.Save();

        SetButtonStates();
    }

    /// <summary>
    /// 選択中のアイテムの売却
    /// </summary>
    public void SellItem(){
        if(selectedItem == null){
            return;
        }
        if(selectedItem == GameManager.playerProfile.weapon){
            return;
        }

        GameManager.playerProfile.currecy += ((Weapon)selectedItem).score * 10;
        if(PlayerProfile.maximumCurrencyLimit < GameManager.playerProfile.currecy){
            GameManager.playerProfile.currecy = PlayerProfile.maximumCurrencyLimit;
        }

        MenuManager.instance.UpdateProfile();

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

        SaveDataManager.SetClass("Player Profile", GameManager.playerProfile);
        SaveDataManager.Save();

        SetButtonStates();
    }

    /// <summary>
    /// 選択中のアイテムの削除
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
    /// 装備した武器のプレイヤー情報への反映
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
