//================================================================================
//
//  GameManager
//
//  ゲーム(GameScene)の管理を行う
//
//================================================================================

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{

    /**************************************************
        Enumerations
    **************************************************/

    /// <summary>
    /// プラットフォームの種類
    /// </summary>
    public enum TargetPlatform{
        PC,
        Mobile
    }

    /// <summary>
    /// 視点の回転方式(未使用)
    /// </summary>
    public enum RotationStyle{
        RotatePlayer,
        RotateWorld
    }

    /**************************************************
        Fields / Properties
    **************************************************/

    //  Singleton Instance
    //==============================

    /// <summary>
    /// クラスのインスタンス
    /// </summary>
    public static GameManager instance{
        get;
        private set;
    }

    //  Game states
    //==============================

    /// <summary>
    /// ポーズ中であるか
    /// </summary>
    public bool isPaused{
        get;
        private set;
    }

    /// <summary>
    /// ゲームオーバー状態であるか
    /// </summary>
    public bool isGameOver{
        get;
        private set;
    }

    //  Settings
    //==============================

    /// <summary>
    /// 対象プラットフォーム
    /// </summary>
    [field: Header("Settings")]
    [field: SerializeField, RenameField("Target Platform")]
    public TargetPlatform targetPlatform{
        get;
        private set;
    }
    
    /// <summary>
    /// 視点の回転方式
    /// </summary>
    [field: SerializeField, RenameField("Rotation Style")]
    public RotationStyle rotationStyle{
        get;
        private set;
    }

    //  Camera
    //==============================

    /// <summary>
    /// カメラ
    /// </summary>
    [field: Header("Camera")]
    [field: SerializeField, RenameField("Camera")]
    public Camera camera{
        get;
        private set;
    }
    
    /// <summary>
    /// カメラの回転軸
    /// </summary>
    [field: SerializeField, RenameField("Camera Axis")]
    public GameObject cameraAxis{
        get;
        private set;
    }

    //  Game Speed
    //==============================

    /// <summary>
    /// 実際のゲームスピード(走る速度)
    /// </summary>
    public float currentGameSpeed{
        get{
            return baseGameSpeed + additionalGameSpeed;
        }
    }

    /// <summary>
    /// 基本のゲームスピード
    /// </summary>
    [field: Header("Game Speed")]
    [field: SerializeField, RenameField("Base Game Speed")]
    public float baseGameSpeed{
        get;
        private set;
    }

    /// <summary>
    /// 最大のゲームスピード
    /// </summary>
    [field: SerializeField, RenameField("Maximum Game Speed")]
    public float maximumGameSpeed{
        get;
        private set;
    }
    
    /// <summary>
    /// 最小のゲームスピード
    /// </summary>
    [field: SerializeField, RenameField("Minimum Game Speed")]
    public float minimumGameSpeed{
        get;
        private set;
    }
    
    /// <summary>
    /// ゲームスピードの変動値
    /// </summary>
    [HideInInspector]
    public float additionalGameSpeed{
        get;
        set;
    }

    /// <summary>
    /// 最大ゲームスピードへ達するのに必要なコインの数
    /// </summary>
    [field: SerializeField, RenameField("Required Coin For Maximum Speed")]
    private int requiredCoinForMaximumSpeed{
        get;
        set;
    }

    //  Platform Generation
    //==============================

    /// <summary>
    /// 足場のプレハブ
    /// </summary>
    [field: Header("Platform Generation")]
    [field: SerializeField, RenameField("Platform Prefab")]
    private GameObject platformPrefab{
        get;
        set;
    }

    /// <summary>
    /// 足場の生成位置
    /// </summary>
    [field: SerializeField, RenameField("Platform Spawn Position")]
    private Vector2 platformSpawnPosition{
        get;
        set;
    }

    /// <summary>
    /// 足場の最大サイズ
    /// </summary>
    [field: Space(20)]
    [field: SerializeField, RenameField("Maximum Platform Size")]
    private Vector2 maximumPlatformSize{
        get;
        set;
    }

    /// <summary>
    /// 足場の最小サイズ
    /// </summary>
    [field: Space(20)]
    [field: SerializeField, RenameField("Minimum Platform Size")]
    private Vector2 minimumPlatformSize{
        get;
        set;
    }
    
    /// <summary>
    /// 足場の高さの最大値
    /// </summary>
    private float maximumPlatformHeightChange{
        get{
            float gameSpeedRate = (baseGameSpeed - minimumGameSpeed) / (maximumGameSpeed - minimumGameSpeed);

            if(gameSpeedRate < 0.2f){
                return 0;
            }
            else if(gameSpeedRate < 0.4f){
                return 1;
            }
            else if(gameSpeedRate < 0.6f){
                return 2;
            }
            else if(gameSpeedRate < 0.8f){
                return 3;
            }
            else{
                return 4;
            }
        }
    }
    
    /// <summary>
    /// 足場の高さの最小値
    /// </summary>
    private float minimumPlatformHeightChange{
        get{
            float gameSpeedRate = (baseGameSpeed - minimumGameSpeed) / (maximumGameSpeed - minimumGameSpeed);

            if(gameSpeedRate < 0.2f){
                return 0;
            }
            else if(gameSpeedRate < 0.4f){
                return 0;
            }
            else if(gameSpeedRate < 0.6f){
                return 0;
            }
            else if(gameSpeedRate < 0.8f){
                return 0;
            }
            else{
                return 1.5f;
            }
        }
    }
    
    /// <summary>
    /// ゲームスピードによる足場の間隔の変動具合
    /// </summary>
    private float platformSpacingRate{
        get{
            float gameSpeedRate = (baseGameSpeed - minimumGameSpeed) / (maximumGameSpeed - minimumGameSpeed);

            if(gameSpeedRate < 0.2f){
                return 0;
            }
            else if(gameSpeedRate < 0.4f){
                return 0;
            }
            else if(gameSpeedRate < 0.6f){
                return 0.3f;
            }
            else if(gameSpeedRate < 0.8f){
                return 0.3f;
            }
            else{
                return 0.5f;
            }
        }
    }

    /// <summary>
    /// 足場の幅の最小変動値
    /// </summary>
    private float minimumPlatformRange{
        get{
            float gameSpeedRate = (baseGameSpeed - minimumGameSpeed) / (maximumGameSpeed - minimumGameSpeed);

            if(gameSpeedRate < 0.2f){
                return 0f;
            }
            else if(gameSpeedRate < 0.4f){
                return 0f;
            }
            else if(gameSpeedRate < 0.6f){
                return 1.5f;
            }
            else if(gameSpeedRate < 0.8f){
                return 2f;
            }
            else{
                return 2.5f;
            }
        }
    }

    /// <summary>
    /// 足場の幅の最大変動値
    /// </summary>
    private float maximumPlatformRange{
        get{
            float gameSpeedRate = (baseGameSpeed - minimumGameSpeed) / (maximumGameSpeed - minimumGameSpeed);

            if(gameSpeedRate < 0.2f){
                return 0;
            }
            else if(gameSpeedRate < 0.4f){
                return 0;
            }
            else if(gameSpeedRate < 0.6f){
                return 2.0f;
            }
            else if(gameSpeedRate < 0.8f){
                return 3.0f;
            }
            else{
                return 4.0f;
            }
        }
    }

    /// <summary>
    /// 足場の幅の最小単位
    /// </summary>
    [field: Space(20)]
    [field: SerializeField, RenameField("Width Unit")]
    private float widthUnit{
        get;
        set;
    }

    /// <summary>
    /// 足場の高さの最小単位
    /// </summary>
    [field: SerializeField, RenameField("height Unit")]
    private float heightUnit{
        get;
        set;
    }

    /// <summary>
    /// 最後に生成された足場
    /// </summary>
    private GameObject lastPlatform{
        get;
        set;
    }

    /// <summary>
    /// 最後に生成された背景の建物
    /// </summary>
    private GameObject lastBackGround{
        get;
        set;
    }

    //  Environment Generation
    //==============================

    /// <summary>
    /// 街灯のプレハブ
    /// </summary>
    [field: Header("Environment Generation")]
    [field: SerializeField, RenameField("Street Lamp Prefab")]
    private GameObject streetLampPrefab{
        get;
        set;
    }

    /// <summary>
    /// 街灯の生成位置
    /// </summary>
    [field: SerializeField, RenameField("Street Lamp Generation Position")]
    private Vector2 streetLampGenerationPosition{
        get;
        set;
    }

    /// <summary>
    /// 街灯の生成間隔
    /// </summary>
    [field: Space(20)]
    [field: SerializeField, RenameField("Street Lamp Generation Interval")]
    private float StreetLampGenerationInterval{
        get;
        set;
    }

    /// <summary>
    /// 背景の建物のプレハブ
    /// </summary>
    [field: SerializeField, RenameField("Building Prefabs")]
    private List<GameObject> buildingPrefabs{
        get;
        set;
    }

    /// <summary>
    /// 背景の建物の生成位置
    /// </summary>
    [field: SerializeField, RenameField("Background Generation Position")]
    private Vector2 backgroundGenerationPosition{
        get;
        set;
    }

    /// <summary>
    /// 背景の建物の流れる速度の倍率
    /// </summary>
    [field: Space(20)]
    [field: SerializeField, RenameField("Background Speed Multiplier")]
    public float backgroundSpeedMultiplier{
        get;
        set;
    }

    //  Enemy Spawn
    //==============================

    /// <summary>
    /// 敵(障害物)のプレハブ
    /// </summary>
    [field: Header("Enemy Spawn")]
    [field: SerializeField]
    private GameObject[] enemyPrefabs{
        get;
        set;
    }

    /// <summary>
    /// 敵の基本生成位置
    /// </summary>
    [field: SerializeField, RenameField("Enemy Spawn Position")]
    private Vector2 enemySpawnPosition{
        get;
        set;
    }
    
    /// <summary>
    /// 敵生成の最小間隔
    /// </summary>
    private float minimumEnemySpawnInterval{
        get{
            float coinCountRate = (float)coinCount / (float)requiredCoinForMaximumSpeed;
            if(coinCountRate < 1.0f){
                return 3.0f;
            }else if(coinCountRate < 1.2f){
                return 2.75f;
            }else if(coinCountRate < 1.4f){
                return 2.5f;
            }else if(coinCountRate < 1.8f){
                return 2.25f;
            }else if(coinCountRate < .0f){
                return 2.0f;
            }else{
                return 1.0f;
            }
        }
    }

    /// <summary>
    /// 敵生成の最大間隔
    /// </summary>
    private float maximumEnemySpawnInterval{
        get{
            float coinCountRate = (float)coinCount / (float)requiredCoinForMaximumSpeed;
            if(coinCountRate < 1.0f){
                return 8.0f;
            }else if(coinCountRate < 1.2f){
                return 7.0f;
            }else if(coinCountRate < 1.4f){
                return 6.0f;
            }else if(coinCountRate < 1.6f){
                return 5.0f;
            }else if(coinCountRate < 1.8f){
                return 4.0f;
            }else if(coinCountRate < 10.0f){
                return 3.0f;
            }else{
                return 1.0f;
            }
        }
    }

    //  Coin
    //==============================

    /// <summary>
    /// コインのプレハブ
    /// </summary>
    [field: Header("Coin")]
    [field: Space(20)]
    [field: SerializeField, RenameField("Coin Prefab")]
    public GameObject coinPrefab{
        get;
        private set;
    }

    /// <summary>
    /// コインの取得数
    /// </summary>
    [field: SerializeField, RenameField("Coin Count")]
    private int coinCount{
        get;
        set;
    }

    /// <summary>
    /// ゲームの進行度毎のコインの価値
    /// </summary>
    private int coinValue_field;
    private int coinValue{
        get{
            float gameSpeedRate = (baseGameSpeed - minimumGameSpeed) / (maximumGameSpeed - minimumGameSpeed);

            if(gameSpeedRate < 0.2f){
                return 5;
            }
            else if(gameSpeedRate < 0.4f){
                return 10;
            }
            else if(gameSpeedRate < 0.6f){
                return 15;
            }
            else if(gameSpeedRate < 0.8f){
                return 30;
            }
            else{
                return 50;
            }
        }
    }
    
    /// <summary>
    /// コインの散らばる範囲
    /// </summary>
    [field: SerializeField, RenameField("Coin Spread Range")]
    public float coinSpreadRange{
        get;
        private set;
    }

    //  Drop Item
    //==============================

    /// <summary>
    /// ドロップアイテムのプレハブ
    /// </summary>
    [field: Header("Drop Item")]
    [field: SerializeField]
    public GameObject[] dropItemPrefabs{
        get;
        private set;
    }
    
    /// <summary>
    /// アイテムのドロップ確率
    /// </summary>
    [field: SerializeField, RenameField("Item Drop Chance")]
    public float itemDropChance{
        get;
        private set;
    }

    //  Experience Point
    //==============================

    /// <summary>
    /// 獲得した経験値量
    /// </summary>
    public int earnedExperiencePoint{
        get;
        private set;
    }

    /// <summary>
    /// コイン取得による経験値量
    /// </summary>
    [field: Header("Experience Point")]
    [field: SerializeField, RenameField("Experience Point Per Coin")]
    public int experiencePointPerCoin{
        get;
        private set;
    }

    /// <summary>
    /// 敵撃破による経験値量
    /// </summary>
    [field: SerializeField, RenameField("Experience Point Per Enemy")]
    public int experiencePointPerEnemy{
        get;
        private set;
    }

    //  Score
    //==============================

    /// <summary>
    /// 移動距離
    /// </summary>
    private float traveledDistance{
        get;
        set;
    }

    /// <summary>
    /// 移動距離のカウント最大値
    /// </summary>
    private const float maximumTraveledDistanceLimit = 999999.99f;

    /// <summary>
    /// 獲得金額
    /// </summary>
    private int earnedCurrency{
        get;
        set;
    }

    /// <summary>
    /// 獲得金額のカウント最大値
    /// </summary>
    private const int maximumEarnedCurrencyLimit = 99999999;

    /// <summary>
    /// 与えたダメージ
    /// </summary>
    private int dealtDamage{
        get;
        set;
    }

    /// <summary>
    /// 与えたダメージのカウント最大値
    /// </summary>
    private const int maximumDealtDamageLimit = 99999;

    /// <summary>
    /// 敵撃破数
    /// </summary>
    private int defeatedEnemyCount{
        get;
        set;
    }

    /// <summary>
    /// 敵撃破数のカウント最大値
    /// </summary>
    private const int maximumDefeatedEnemyLimit = 999;

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
    /// BGM
    /// </summary>
    [field:SerializeField,RenameField("BGM")]
    private List<AudioClip> bgm{
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

    //  Player profile
    //==============================

    /// <summary>
    /// プレイヤーの情報
    /// </summary>
    public static PlayerProfile playerProfile{
        get;
        set;
    }

    //  Prefabs
    //==============================

    /// <summary>
    /// 射出物のプレハブ
    /// </summary>
    public Dictionary<Weapon.Projectile, GameObject> projectilePrefabs{
        get;
        private set;
    }

    //  UI
    //==============================

    /// <summary>
    /// オーバーレイ
    /// </summary>
    [field: Header("UI")]
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

    /**************************************************
        Unity Event Functions
    **************************************************/

    private void Start(){
        Initialize();
    }

    private void FixedUpdate(){
        CountTraveledDistance();
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
    private void Initialize(){
        if(instance == null){
            instance = this;
        }
        else if(instance != this){
            Destroy(gameObject);
        }

        isGameOver = false;
        isPaused = false;
        SwitchPause(false);

        Input.gyro.enabled = true;

        baseGameSpeed = minimumGameSpeed;
        additionalGameSpeed = 0.0f;

        lastPlatform = null;

        coinCount = 0;

        traveledDistance = 0.0f;
        earnedCurrency = 0;
        dealtDamage = 0;
        defeatedEnemyCount = 0;

        LoadProjectiles();

        pauseMenuWindow.GetComponent<OptionWindowBehaviour>().Initialize();

        CreatePlatform(60, 2, new Vector3(-15, platformSpawnPosition.y, 0));
        CreateBuilding(new Vector2(-10, backgroundGenerationPosition.y));
        while(CreateBuilding());
        
        StartCoroutine(CreatePlatformContinuously());
        StartCoroutine(SpawnEnemyContinuously());
        StartCoroutine(SpawnStreetLampContinuously());
        StartCoroutine(CreateBuildingContinuously());

        if(0.333f < Random.value){
            GetComponents<AudioSource>()[0].clip = bgm[0];
        }
        else{
            GetComponents<AudioSource>()[0].clip = bgm[1];
        }
        GetComponents<AudioSource>()[0].Play();

        StartGame();
    }

    /// <summary>
    /// ゲームの開始処理
    /// </summary>
    private async void StartGame(){
        UIManager.instance.ToggleOverlay(true);
        overlayAnimator.SetTrigger("FadeOut");

        await Task.Delay(3000);

        UIManager.instance.ToggleOverlay(false);
    }

    /// <summary>
    /// 射出物の読み込み
    /// </summary>
    private void LoadProjectiles(){
        projectilePrefabs = new Dictionary<Weapon.Projectile, GameObject>();

        projectilePrefabs.Add(Weapon.Projectile.Bullet, Resources.Load<GameObject>("Prefabs/Actors/Bullet"));

        //projectilePrefabs.Add(Weapon.Projectile.Granade, Resources.Load<GameObject>("Prefabs/Actors/Bullet"));
        //projectilePrefabs.Add(Weapon.Projectile.Rocket, Resources.Load<GameObject>("Prefabs/Actors/Bullet"));
        //projectilePrefabs.Add(Weapon.Projectile.Laser, Resources.Load<GameObject>("Prefabs/Actors/Bullet"));
    }

    /// <summary>
    /// 足場の生成
    /// </summary>
    /// <param name="width">足場の幅</param>
    /// <param name="height">足場の高さ</param>
    private void CreatePlatform(float width, float height){
        GameObject platform = Instantiate(platformPrefab);

        platform.transform.position = new Vector2(platformSpawnPosition.x + width / 2, platformSpawnPosition.y + height / 2);
        //platform.transform.localScale = new Vector2(width, height);

        platform.GetComponent<PlatformBehaviour>().size = new Vector2(width, height);

        lastPlatform = platform;
    }
    /// <summary>
    /// 足場の生成
    /// </summary>
    /// <param name="width">足場の幅</param>
    /// <param name="height">足場の高さ</param>
    /// <param name="position">生成位置</param>
    private void CreatePlatform(float width, float height, Vector2 position){
        GameObject platform = Instantiate(platformPrefab, new Vector3(position.x + width / 2, position.y + height / 2, 0.0f), Quaternion.identity);

        //platform.transform.localScale = new Vector2(width, height);

        platform.GetComponent<PlatformBehaviour>().size = new Vector2(width, height);

        lastPlatform = platform;
    }
    /// <summary>
    /// 足場の生成
    /// </summary>
    /// <param name="width">足場の幅</param>
    /// <param name="height">足場の高さ</param>
    /// <param name="distanceFromLastPlatform">最後に生成した足場からの距離</param>
    private void CreatePlatform(float width, float height, float distanceFromLastPlatform){
        if(lastPlatform == null){
            CreatePlatform(width, height);
            return;
        }

        GameObject platform = Instantiate(platformPrefab);

        //platform.transform.position = new Vector2(platformSpawnPosition.x + distanceFromLastPlatform + width / 2, platformSpawnPosition.y + height / 2);
        platform.transform.position = new Vector2(lastPlatform.transform.position.x + lastPlatform.GetComponent<BoxCollider2D>().bounds.size.x / 2 + distanceFromLastPlatform + width / 2, platformSpawnPosition.y + height / 2);
        //platform.transform.localScale = new Vector2(width, height);
        platform.GetComponent<PlatformBehaviour>().size = new Vector2(width, height);

        lastPlatform = platform;
    }

    /// <summary>
    /// 継続的な足場の生成
    /// </summary>
    /// <returns></returns>
    private IEnumerator CreatePlatformContinuously(){
        while(!isGameOver){
            if(lastPlatform.transform.position.x + lastPlatform.transform.GetComponent<BoxCollider2D>().bounds.size.x / 2 <= platformSpawnPosition.x){
                Vector2 platformSize = new Vector2();
                //widthUnitf * (int)UnityEngine.Random.Range((minimumPlatformSize.x/widthUnit), (maximumPlatformSize.x / widthUnit) + 1)

                platformSize.x = widthUnit * (int)UnityEngine.Random.Range((minimumPlatformSize.x / widthUnit), (maximumPlatformSize.x / widthUnit) + 1);
                float localMinimumPlatfomHeight = Mathf.Max(lastPlatform.transform.GetComponent<BoxCollider2D>().bounds.size.y - maximumPlatformHeightChange, minimumPlatformSize.y);
                float localMaximumPlatfomHeight = Mathf.Min(lastPlatform.transform.GetComponent<BoxCollider2D>().bounds.size.y + maximumPlatformHeightChange, maximumPlatformSize.y);
                platformSize.y = heightUnit * (int)UnityEngine.Random.Range((localMinimumPlatfomHeight / widthUnit), (localMaximumPlatfomHeight / widthUnit) + 1);

                //float platformDistance = platformSpacingRate * baseGameSpeed * UnityEngine.Random.value;
                float platformDistance = widthUnit * (int)UnityEngine.Random.Range((minimumPlatformRange / widthUnit), (maximumPlatformRange / widthUnit) + 1);
                
                CreatePlatform(platformSize.x, platformSize.y, platformDistance);
            }

            yield return null;
        }
    }

    /// <summary>
    /// 背景の建物の生成
    /// </summary>
    /// <param name="position">生成位置</param>
    private void CreateBuilding(Vector2 position){
        GameObject prefab = buildingPrefabs[0];
        if(UnityEngine.Random.value <= 0.2f){
            prefab = buildingPrefabs[UnityEngine.Random.Range(1, buildingPrefabs.Count)];
        }

        GameObject building = Instantiate(prefab, position, Quaternion.identity);
        lastBackGround = building;
    }
    /// <summary>
    /// 背景の建物の生成
    /// </summary>
    /// <param name="distanceFromLastBackground">最後に生成した建物との距離</param>
    private void CreateBuilding(float distanceFromLastBackground){
        Vector2 position = new Vector2(lastBackGround.transform.position.x + (buildingPrefabs[0].GetComponent<SpriteRenderer>().size.x * buildingPrefabs[0].transform.lossyScale.x), backgroundGenerationPosition.y);
        CreateBuilding(position);
    }
    /// <summary>
    /// 背景の建物の生成
    /// </summary>
    /// <returns>生成が必要か</returns>
    private bool CreateBuilding(){
        if(lastBackGround.transform.position.x + (lastBackGround.GetComponent<SpriteRenderer>().size.x * lastBackGround.transform.lossyScale.x) / 2.0f <= platformSpawnPosition.x){
            CreateBuilding(0);

            return true;
        }

        return false;
    }

    /// <summary>
    /// 継続的な建物の生成
    /// </summary>
    private IEnumerator CreateBuildingContinuously(){
        while(!isGameOver){
            if(lastBackGround.transform.position.x + (lastBackGround.GetComponent<SpriteRenderer>().size.x * lastBackGround.transform.lossyScale.x) / 2.0f <= platformSpawnPosition.x){
                CreateBuilding(0);
            }

            yield return null;
        }
    }

    /// <summary>
    /// 敵の生成
    /// </summary>
    /// <param name="enemyPrefab">敵のプレハブ</param>
    private void SpawnEnemy(GameObject enemyPrefab){
        GameObject enemy = Instantiate(enemyPrefab, enemySpawnPosition, Quaternion.identity);
    }
    /// <summary>
    /// 敵の生成
    /// </summary>
    /// <param name="enemyPrefab">敵のプレハブ</param>
    /// <param name="spawnHeight">生成する高さ</param>
    private void SpawnEnemy(GameObject enemyPrefab, float spawnHeight){
        GameObject enemy = Instantiate(enemyPrefab, new Vector2(enemySpawnPosition.x, spawnHeight), Quaternion.identity);
    }
    /// <summary>
    /// 敵の生成
    /// </summary>
    /// <param name="enemyPrefab">敵のプレハブ</param>
    /// <param name="position">生成位置</param>
    private void SpawnEnemy(GameObject enemyPrefab, Vector2 position){
        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
    }

    /// <summary>
    /// 継続的な敵の生成
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnEnemyContinuously(){
        while(!isGameOver){
            float waitTime = UnityEngine.Random.Range(minimumEnemySpawnInterval, maximumEnemySpawnInterval);
            yield return new WaitForSeconds(waitTime);

            RaycastHit2D hit;
            bool isDetectedHall = false;
            while(!(hit = Physics2D.Raycast(new Vector2(enemySpawnPosition.x, 6), Vector2.down, 12, 1 << 8))){
                isDetectedHall = true;
                yield return null;
            }

            int enemyId = UnityEngine.Random.Range(0, enemyPrefabs.Length);
            if(!isDetectedHall){
                SpawnEnemy(enemyPrefabs[enemyId], hit.transform.position.y + hit.transform.GetComponent<BoxCollider2D>().bounds.size.y / 2.0f + (enemyPrefabs[enemyId].transform.GetComponent<BoxCollider2D>().size.y * enemyPrefabs[enemyId].transform.lossyScale.y) / 2.0f + UnityEngine.Random.Range(0.5f, 1.5f));
            }
            else{
                SpawnEnemy(enemyPrefabs[enemyId], new Vector2(hit.transform.position.x + hit.transform.GetComponent<BoxCollider2D>().bounds.size.x / UnityEngine.Random.Range(3.8f, 4.5f), hit.transform.position.y + hit.transform.GetComponent<BoxCollider2D>().bounds.size.y / 2.0f + (enemyPrefabs[enemyId].transform.GetComponent<BoxCollider2D>().size.y * enemyPrefabs[enemyId].transform.lossyScale.y) / 2.0f + UnityEngine.Random.Range(0.5f, 1.5f)));
            }
        }
    }

    /// <summary>
    /// 街灯の生成
    /// </summary>
    /// <param name="spawnHeight">生成する高さ</param>
    private void SpawnStreetLamp(float spawnHeight){
        GameObject enemy = Instantiate(streetLampPrefab, new Vector2(streetLampGenerationPosition.x, spawnHeight), Quaternion.identity);
    }
    /// <summary>
    /// 街灯の生成
    /// </summary>
    /// <param name="spawnPosition">生成位置</param>
    private void SpawnStreetLamp(Vector2 spawnPosition){
        GameObject enemy = Instantiate(streetLampPrefab, spawnPosition, Quaternion.identity);
    }

    /// <summary>
    /// 継続的な街灯の生成
    /// </summary>
    private IEnumerator SpawnStreetLampContinuously(){
        while(!isGameOver){
            float waitTime = StreetLampGenerationInterval / baseGameSpeed;
            yield return new WaitForSeconds(waitTime);

            RaycastHit2D hit;
            bool isDetectedHall = false;
            while(!(hit = Physics2D.Raycast(new Vector2(streetLampGenerationPosition.x, 6), Vector2.down, 12, 1 << 8))){
                isDetectedHall = true;
                yield return null;
            }

            if(!isDetectedHall){
                SpawnStreetLamp(hit.transform.position.y + hit.transform.GetComponent<BoxCollider2D>().bounds.size.y / 2 + (streetLampPrefab.transform.GetComponent<BoxCollider2D>().size.y * streetLampPrefab.transform.lossyScale.y) / 2);
            }
            else{
                SpawnStreetLamp(new Vector2(hit.transform.position.x - hit.transform.GetComponent<BoxCollider2D>().bounds.size.x / 2 * 0.80f, hit.transform.position.y + hit.transform.GetComponent<BoxCollider2D>().bounds.size.y / 2 + (streetLampPrefab.transform.GetComponent<BoxCollider2D>().size.y * streetLampPrefab.transform.lossyScale.y) / 2));
            }
        }
    }

    /// <summary>
    /// コインの取得
    /// </summary>
    /// <param name="type">コインの種類</param>
    public void GetCoin(CoinBehaviour.Type type){
        int valueBonusScale = 0;
        switch(type){
            case CoinBehaviour.Type.Bronze:
                valueBonusScale = 1;
                break;
            case CoinBehaviour.Type.Silver:
                valueBonusScale = 2;
                break;
            case CoinBehaviour.Type.Gold:
                valueBonusScale = 3;
                break;
        }

        earnedCurrency += coinValue * valueBonusScale;
        if(maximumEarnedCurrencyLimit < earnedCurrency){
            earnedCurrency = maximumEarnedCurrencyLimit;
        }

        coinCount++;
        earnedExperiencePoint += experiencePointPerCoin;

        ChangeGameSpeed();

        UIManager.instance.UpdateCurrecyText(earnedCurrency);
    }

    /// <summary>
    /// ダメージの付与
    /// </summary>
    /// <param name="value">ダメージ量</param>
    public void DealDamage(int value){
        dealtDamage += value;
        if(maximumDealtDamageLimit < dealtDamage){
            dealtDamage = maximumDealtDamageLimit;
        }
    }

    /// <summary>
    /// 敵の撃破
    /// </summary>
    public void DefeatEnemy(){
        defeatedEnemyCount++;
        if(maximumDefeatedEnemyLimit < defeatedEnemyCount){
            defeatedEnemyCount = maximumDefeatedEnemyLimit;
        }

        earnedExperiencePoint += experiencePointPerEnemy;
    }

    /// <summary>
    /// コイン獲得数に応じたゲームスピードの変化
    /// </summary>
    private void ChangeGameSpeed(){
        if(coinCount < requiredCoinForMaximumSpeed){
            baseGameSpeed = minimumGameSpeed + (maximumGameSpeed - minimumGameSpeed) * (float)coinCount / (float)requiredCoinForMaximumSpeed;
        }
        else{
            baseGameSpeed = maximumGameSpeed;
        }
    }

    /// <summary>
    /// 進んだ距離のカウント
    /// </summary>
    private void CountTraveledDistance(){
        if(isPaused || isGameOver){
            return;
        }

        traveledDistance += currentGameSpeed * Time.fixedDeltaTime;
        if(maximumTraveledDistanceLimit < traveledDistance){
            traveledDistance = maximumTraveledDistanceLimit;
        }
    }

    /// <summary>
    /// ポーズ状態の切り替え
    /// </summary>
    /// <param name="value">ポーズ状態にするか</param>
    public void SwitchPause(bool value){
        isPaused = value;

        if(value == true){
            Time.timeScale = 0.0f;
        }
        else{
            Time.timeScale = 1.0f;
        }
    }

    /// <summary>
    /// ゲーム終了とシーン切り替え
    /// </summary>
    public void QuitGame(){
        SwitchPause(true);
        isGameOver = true;

        SaveDataManager.Save();

        GoToMenu();
    }

    /// <summary>
    /// ゲームの終了
    /// </summary>
    public void FinishGame(){
        if(isGameOver){
            return;
        }

        SwitchPause(true);
        isGameOver = true;

        UIManager.instance.ToggleOverlay(true);
        UIManager.instance.ShowGameOverScreen();
        UIManager.instance.UpdateScoreTexts(new Score(traveledDistance, earnedCurrency, dealtDamage, defeatedEnemyCount));

        SaveDataManager.Save();
    }

    /// <summary>
    /// ゲームのリトライ
    /// </summary>
    public async void Retry(){
        if(UIManager.instance.isFadeIn){
            return;
        }

        UIManager.instance.isFadeIn = true;
        UIManager.instance.TogglePauseMenuWindow(false);
        UIManager.instance.ToggleOverlay(true);
        overlayAnimator.SetTrigger("FadeIn");

        SaveEarnedCurrency();
        SaveScore();

        await Task.Delay(3000);

        UIManager.instance.isFadeIn = false;

        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// メニューへの切り替え
    /// </summary>
    public async void GoToMenu(){
        if (UIManager.instance.isFadeIn) {
            return;
        }
        UIManager.instance.isFadeIn = true;

        UIManager.instance.TogglePauseMenuWindow(false);


        UIManager.instance.ToggleOverlay(true);
        overlayAnimator.SetTrigger("FadeIn");

        SaveEarnedCurrency();
        SaveScore();

        await Task.Delay(3000);

        UIManager.instance.isFadeIn = false;

        SceneManager.LoadScene("MenuScene");
    }

    /// <summary>
    /// 獲得金額の保存
    /// </summary>
    private void SaveEarnedCurrency(){
        playerProfile.currecy += earnedCurrency;
        if(PlayerProfile.maximumCurrencyLimit < playerProfile.currecy){
            playerProfile.currecy = PlayerProfile.maximumCurrencyLimit;
        }

        SaveDataManager.SetClass("Player Profile", playerProfile);
    }

    /// <summary>
    /// スコア保存
    /// </summary>
    private void SaveScore(){
        traveledDistance = Mathf.Max(traveledDistance, playerProfile.highScore.traveledDistance);
        earnedCurrency = Mathf.Max(earnedCurrency, playerProfile.highScore.earnedCurrency);
        dealtDamage = Mathf.Max(dealtDamage, playerProfile.highScore.dealtDamage);
        defeatedEnemyCount = Mathf.Max(defeatedEnemyCount, playerProfile.highScore.defeatedEnemyCount);

        playerProfile.highScore = new Score(traveledDistance, earnedCurrency, dealtDamage, defeatedEnemyCount);
        SaveDataManager.SetClass("Player Profile", playerProfile);
    }

    /// <summary>
    /// 音声の再生
    /// </summary>
    /// <param name="audioClip">再生するクリップ</param>
    public void PlayAudio(AudioClip audioClip){
        if(audioClip != null){
            audioSources[1].PlayOneShot(audioClip);
        }
    }

}
