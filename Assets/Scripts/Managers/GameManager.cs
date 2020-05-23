//================================================================================
//
//  GameManager
//
//  �Q�[��(GameScene)�̊Ǘ����s��
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
    /// �v���b�g�t�H�[���̎��
    /// </summary>
    public enum TargetPlatform{
        PC,
        Mobile
    }

    /// <summary>
    /// ���_�̉�]����(���g�p)
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
    /// �N���X�̃C���X�^���X
    /// </summary>
    public static GameManager instance{
        get;
        private set;
    }

    //  Game states
    //==============================

    /// <summary>
    /// �|�[�Y���ł��邩
    /// </summary>
    public bool isPaused{
        get;
        private set;
    }

    /// <summary>
    /// �Q�[���I�[�o�[��Ԃł��邩
    /// </summary>
    public bool isGameOver{
        get;
        private set;
    }

    //  Settings
    //==============================

    /// <summary>
    /// �Ώۃv���b�g�t�H�[��
    /// </summary>
    [field: Header("Settings")]
    [field: SerializeField, RenameField("Target Platform")]
    public TargetPlatform targetPlatform{
        get;
        private set;
    }
    
    /// <summary>
    /// ���_�̉�]����
    /// </summary>
    [field: SerializeField, RenameField("Rotation Style")]
    public RotationStyle rotationStyle{
        get;
        private set;
    }

    //  Camera
    //==============================

    /// <summary>
    /// �J����
    /// </summary>
    [field: Header("Camera")]
    [field: SerializeField, RenameField("Camera")]
    public Camera camera{
        get;
        private set;
    }
    
    /// <summary>
    /// �J�����̉�]��
    /// </summary>
    [field: SerializeField, RenameField("Camera Axis")]
    public GameObject cameraAxis{
        get;
        private set;
    }

    //  Game Speed
    //==============================

    /// <summary>
    /// ���ۂ̃Q�[���X�s�[�h(���鑬�x)
    /// </summary>
    public float currentGameSpeed{
        get{
            return baseGameSpeed + additionalGameSpeed;
        }
    }

    /// <summary>
    /// ��{�̃Q�[���X�s�[�h
    /// </summary>
    [field: Header("Game Speed")]
    [field: SerializeField, RenameField("Base Game Speed")]
    public float baseGameSpeed{
        get;
        private set;
    }

    /// <summary>
    /// �ő�̃Q�[���X�s�[�h
    /// </summary>
    [field: SerializeField, RenameField("Maximum Game Speed")]
    public float maximumGameSpeed{
        get;
        private set;
    }
    
    /// <summary>
    /// �ŏ��̃Q�[���X�s�[�h
    /// </summary>
    [field: SerializeField, RenameField("Minimum Game Speed")]
    public float minimumGameSpeed{
        get;
        private set;
    }
    
    /// <summary>
    /// �Q�[���X�s�[�h�̕ϓ��l
    /// </summary>
    [HideInInspector]
    public float additionalGameSpeed{
        get;
        set;
    }

    /// <summary>
    /// �ő�Q�[���X�s�[�h�֒B����̂ɕK�v�ȃR�C���̐�
    /// </summary>
    [field: SerializeField, RenameField("Required Coin For Maximum Speed")]
    private int requiredCoinForMaximumSpeed{
        get;
        set;
    }

    //  Platform Generation
    //==============================

    /// <summary>
    /// ����̃v���n�u
    /// </summary>
    [field: Header("Platform Generation")]
    [field: SerializeField, RenameField("Platform Prefab")]
    private GameObject platformPrefab{
        get;
        set;
    }

    /// <summary>
    /// ����̐����ʒu
    /// </summary>
    [field: SerializeField, RenameField("Platform Spawn Position")]
    private Vector2 platformSpawnPosition{
        get;
        set;
    }

    /// <summary>
    /// ����̍ő�T�C�Y
    /// </summary>
    [field: Space(20)]
    [field: SerializeField, RenameField("Maximum Platform Size")]
    private Vector2 maximumPlatformSize{
        get;
        set;
    }

    /// <summary>
    /// ����̍ŏ��T�C�Y
    /// </summary>
    [field: Space(20)]
    [field: SerializeField, RenameField("Minimum Platform Size")]
    private Vector2 minimumPlatformSize{
        get;
        set;
    }
    
    /// <summary>
    /// ����̍����̍ő�l
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
    /// ����̍����̍ŏ��l
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
    /// �Q�[���X�s�[�h�ɂ�鑫��̊Ԋu�̕ϓ��
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
    /// ����̕��̍ŏ��ϓ��l
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
    /// ����̕��̍ő�ϓ��l
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
    /// ����̕��̍ŏ��P��
    /// </summary>
    [field: Space(20)]
    [field: SerializeField, RenameField("Width Unit")]
    private float widthUnit{
        get;
        set;
    }

    /// <summary>
    /// ����̍����̍ŏ��P��
    /// </summary>
    [field: SerializeField, RenameField("height Unit")]
    private float heightUnit{
        get;
        set;
    }

    /// <summary>
    /// �Ō�ɐ������ꂽ����
    /// </summary>
    private GameObject lastPlatform{
        get;
        set;
    }

    /// <summary>
    /// �Ō�ɐ������ꂽ�w�i�̌���
    /// </summary>
    private GameObject lastBackGround{
        get;
        set;
    }

    //  Environment Generation
    //==============================

    /// <summary>
    /// �X���̃v���n�u
    /// </summary>
    [field: Header("Environment Generation")]
    [field: SerializeField, RenameField("Street Lamp Prefab")]
    private GameObject streetLampPrefab{
        get;
        set;
    }

    /// <summary>
    /// �X���̐����ʒu
    /// </summary>
    [field: SerializeField, RenameField("Street Lamp Generation Position")]
    private Vector2 streetLampGenerationPosition{
        get;
        set;
    }

    /// <summary>
    /// �X���̐����Ԋu
    /// </summary>
    [field: Space(20)]
    [field: SerializeField, RenameField("Street Lamp Generation Interval")]
    private float StreetLampGenerationInterval{
        get;
        set;
    }

    /// <summary>
    /// �w�i�̌����̃v���n�u
    /// </summary>
    [field: SerializeField, RenameField("Building Prefabs")]
    private List<GameObject> buildingPrefabs{
        get;
        set;
    }

    /// <summary>
    /// �w�i�̌����̐����ʒu
    /// </summary>
    [field: SerializeField, RenameField("Background Generation Position")]
    private Vector2 backgroundGenerationPosition{
        get;
        set;
    }

    /// <summary>
    /// �w�i�̌����̗���鑬�x�̔{��
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
    /// �G(��Q��)�̃v���n�u
    /// </summary>
    [field: Header("Enemy Spawn")]
    [field: SerializeField]
    private GameObject[] enemyPrefabs{
        get;
        set;
    }

    /// <summary>
    /// �G�̊�{�����ʒu
    /// </summary>
    [field: SerializeField, RenameField("Enemy Spawn Position")]
    private Vector2 enemySpawnPosition{
        get;
        set;
    }
    
    /// <summary>
    /// �G�����̍ŏ��Ԋu
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
    /// �G�����̍ő�Ԋu
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
    /// �R�C���̃v���n�u
    /// </summary>
    [field: Header("Coin")]
    [field: Space(20)]
    [field: SerializeField, RenameField("Coin Prefab")]
    public GameObject coinPrefab{
        get;
        private set;
    }

    /// <summary>
    /// �R�C���̎擾��
    /// </summary>
    [field: SerializeField, RenameField("Coin Count")]
    private int coinCount{
        get;
        set;
    }

    /// <summary>
    /// �Q�[���̐i�s�x���̃R�C���̉��l
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
    /// �R�C���̎U��΂�͈�
    /// </summary>
    [field: SerializeField, RenameField("Coin Spread Range")]
    public float coinSpreadRange{
        get;
        private set;
    }

    //  Drop Item
    //==============================

    /// <summary>
    /// �h���b�v�A�C�e���̃v���n�u
    /// </summary>
    [field: Header("Drop Item")]
    [field: SerializeField]
    public GameObject[] dropItemPrefabs{
        get;
        private set;
    }
    
    /// <summary>
    /// �A�C�e���̃h���b�v�m��
    /// </summary>
    [field: SerializeField, RenameField("Item Drop Chance")]
    public float itemDropChance{
        get;
        private set;
    }

    //  Experience Point
    //==============================

    /// <summary>
    /// �l�������o���l��
    /// </summary>
    public int earnedExperiencePoint{
        get;
        private set;
    }

    /// <summary>
    /// �R�C���擾�ɂ��o���l��
    /// </summary>
    [field: Header("Experience Point")]
    [field: SerializeField, RenameField("Experience Point Per Coin")]
    public int experiencePointPerCoin{
        get;
        private set;
    }

    /// <summary>
    /// �G���j�ɂ��o���l��
    /// </summary>
    [field: SerializeField, RenameField("Experience Point Per Enemy")]
    public int experiencePointPerEnemy{
        get;
        private set;
    }

    //  Score
    //==============================

    /// <summary>
    /// �ړ�����
    /// </summary>
    private float traveledDistance{
        get;
        set;
    }

    /// <summary>
    /// �ړ������̃J�E���g�ő�l
    /// </summary>
    private const float maximumTraveledDistanceLimit = 999999.99f;

    /// <summary>
    /// �l�����z
    /// </summary>
    private int earnedCurrency{
        get;
        set;
    }

    /// <summary>
    /// �l�����z�̃J�E���g�ő�l
    /// </summary>
    private const int maximumEarnedCurrencyLimit = 99999999;

    /// <summary>
    /// �^�����_���[�W
    /// </summary>
    private int dealtDamage{
        get;
        set;
    }

    /// <summary>
    /// �^�����_���[�W�̃J�E���g�ő�l
    /// </summary>
    private const int maximumDealtDamageLimit = 99999;

    /// <summary>
    /// �G���j��
    /// </summary>
    private int defeatedEnemyCount{
        get;
        set;
    }

    /// <summary>
    /// �G���j���̃J�E���g�ő�l
    /// </summary>
    private const int maximumDefeatedEnemyLimit = 999;

    //  Audio
    //==============================

    /// <summary>
    /// �I�[�f�B�I�\�[�X
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
    /// �I�[�o�[���C�̃A�j���[�^�[
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
    /// �v���C���[�̏��
    /// </summary>
    public static PlayerProfile playerProfile{
        get;
        set;
    }

    //  Prefabs
    //==============================

    /// <summary>
    /// �ˏo���̃v���n�u
    /// </summary>
    public Dictionary<Weapon.Projectile, GameObject> projectilePrefabs{
        get;
        private set;
    }

    //  UI
    //==============================

    /// <summary>
    /// �I�[�o�[���C
    /// </summary>
    [field: Header("UI")]
    [field: SerializeField, RenameField("Overlay")]
    private GameObject overlay{
        get;
        set;
    }

    /// <summary>
    /// �|�[�Y���j���[�E�B���h�E
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
    /// ����������
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
    /// �Q�[���̊J�n����
    /// </summary>
    private async void StartGame(){
        UIManager.instance.ToggleOverlay(true);
        overlayAnimator.SetTrigger("FadeOut");

        await Task.Delay(3000);

        UIManager.instance.ToggleOverlay(false);
    }

    /// <summary>
    /// �ˏo���̓ǂݍ���
    /// </summary>
    private void LoadProjectiles(){
        projectilePrefabs = new Dictionary<Weapon.Projectile, GameObject>();

        projectilePrefabs.Add(Weapon.Projectile.Bullet, Resources.Load<GameObject>("Prefabs/Actors/Bullet"));

        //projectilePrefabs.Add(Weapon.Projectile.Granade, Resources.Load<GameObject>("Prefabs/Actors/Bullet"));
        //projectilePrefabs.Add(Weapon.Projectile.Rocket, Resources.Load<GameObject>("Prefabs/Actors/Bullet"));
        //projectilePrefabs.Add(Weapon.Projectile.Laser, Resources.Load<GameObject>("Prefabs/Actors/Bullet"));
    }

    /// <summary>
    /// ����̐���
    /// </summary>
    /// <param name="width">����̕�</param>
    /// <param name="height">����̍���</param>
    private void CreatePlatform(float width, float height){
        GameObject platform = Instantiate(platformPrefab);

        platform.transform.position = new Vector2(platformSpawnPosition.x + width / 2, platformSpawnPosition.y + height / 2);
        //platform.transform.localScale = new Vector2(width, height);

        platform.GetComponent<PlatformBehaviour>().size = new Vector2(width, height);

        lastPlatform = platform;
    }
    /// <summary>
    /// ����̐���
    /// </summary>
    /// <param name="width">����̕�</param>
    /// <param name="height">����̍���</param>
    /// <param name="position">�����ʒu</param>
    private void CreatePlatform(float width, float height, Vector2 position){
        GameObject platform = Instantiate(platformPrefab, new Vector3(position.x + width / 2, position.y + height / 2, 0.0f), Quaternion.identity);

        //platform.transform.localScale = new Vector2(width, height);

        platform.GetComponent<PlatformBehaviour>().size = new Vector2(width, height);

        lastPlatform = platform;
    }
    /// <summary>
    /// ����̐���
    /// </summary>
    /// <param name="width">����̕�</param>
    /// <param name="height">����̍���</param>
    /// <param name="distanceFromLastPlatform">�Ō�ɐ����������ꂩ��̋���</param>
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
    /// �p���I�ȑ���̐���
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
    /// �w�i�̌����̐���
    /// </summary>
    /// <param name="position">�����ʒu</param>
    private void CreateBuilding(Vector2 position){
        GameObject prefab = buildingPrefabs[0];
        if(UnityEngine.Random.value <= 0.2f){
            prefab = buildingPrefabs[UnityEngine.Random.Range(1, buildingPrefabs.Count)];
        }

        GameObject building = Instantiate(prefab, position, Quaternion.identity);
        lastBackGround = building;
    }
    /// <summary>
    /// �w�i�̌����̐���
    /// </summary>
    /// <param name="distanceFromLastBackground">�Ō�ɐ������������Ƃ̋���</param>
    private void CreateBuilding(float distanceFromLastBackground){
        Vector2 position = new Vector2(lastBackGround.transform.position.x + (buildingPrefabs[0].GetComponent<SpriteRenderer>().size.x * buildingPrefabs[0].transform.lossyScale.x), backgroundGenerationPosition.y);
        CreateBuilding(position);
    }
    /// <summary>
    /// �w�i�̌����̐���
    /// </summary>
    /// <returns>�������K�v��</returns>
    private bool CreateBuilding(){
        if(lastBackGround.transform.position.x + (lastBackGround.GetComponent<SpriteRenderer>().size.x * lastBackGround.transform.lossyScale.x) / 2.0f <= platformSpawnPosition.x){
            CreateBuilding(0);

            return true;
        }

        return false;
    }

    /// <summary>
    /// �p���I�Ȍ����̐���
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
    /// �G�̐���
    /// </summary>
    /// <param name="enemyPrefab">�G�̃v���n�u</param>
    private void SpawnEnemy(GameObject enemyPrefab){
        GameObject enemy = Instantiate(enemyPrefab, enemySpawnPosition, Quaternion.identity);
    }
    /// <summary>
    /// �G�̐���
    /// </summary>
    /// <param name="enemyPrefab">�G�̃v���n�u</param>
    /// <param name="spawnHeight">�������鍂��</param>
    private void SpawnEnemy(GameObject enemyPrefab, float spawnHeight){
        GameObject enemy = Instantiate(enemyPrefab, new Vector2(enemySpawnPosition.x, spawnHeight), Quaternion.identity);
    }
    /// <summary>
    /// �G�̐���
    /// </summary>
    /// <param name="enemyPrefab">�G�̃v���n�u</param>
    /// <param name="position">�����ʒu</param>
    private void SpawnEnemy(GameObject enemyPrefab, Vector2 position){
        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
    }

    /// <summary>
    /// �p���I�ȓG�̐���
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
    /// �X���̐���
    /// </summary>
    /// <param name="spawnHeight">�������鍂��</param>
    private void SpawnStreetLamp(float spawnHeight){
        GameObject enemy = Instantiate(streetLampPrefab, new Vector2(streetLampGenerationPosition.x, spawnHeight), Quaternion.identity);
    }
    /// <summary>
    /// �X���̐���
    /// </summary>
    /// <param name="spawnPosition">�����ʒu</param>
    private void SpawnStreetLamp(Vector2 spawnPosition){
        GameObject enemy = Instantiate(streetLampPrefab, spawnPosition, Quaternion.identity);
    }

    /// <summary>
    /// �p���I�ȊX���̐���
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
    /// �R�C���̎擾
    /// </summary>
    /// <param name="type">�R�C���̎��</param>
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
    /// �_���[�W�̕t�^
    /// </summary>
    /// <param name="value">�_���[�W��</param>
    public void DealDamage(int value){
        dealtDamage += value;
        if(maximumDealtDamageLimit < dealtDamage){
            dealtDamage = maximumDealtDamageLimit;
        }
    }

    /// <summary>
    /// �G�̌��j
    /// </summary>
    public void DefeatEnemy(){
        defeatedEnemyCount++;
        if(maximumDefeatedEnemyLimit < defeatedEnemyCount){
            defeatedEnemyCount = maximumDefeatedEnemyLimit;
        }

        earnedExperiencePoint += experiencePointPerEnemy;
    }

    /// <summary>
    /// �R�C���l�����ɉ������Q�[���X�s�[�h�̕ω�
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
    /// �i�񂾋����̃J�E���g
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
    /// �|�[�Y��Ԃ̐؂�ւ�
    /// </summary>
    /// <param name="value">�|�[�Y��Ԃɂ��邩</param>
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
    /// �Q�[���I���ƃV�[���؂�ւ�
    /// </summary>
    public void QuitGame(){
        SwitchPause(true);
        isGameOver = true;

        SaveDataManager.Save();

        GoToMenu();
    }

    /// <summary>
    /// �Q�[���̏I��
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
    /// �Q�[���̃��g���C
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
    /// ���j���[�ւ̐؂�ւ�
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
    /// �l�����z�̕ۑ�
    /// </summary>
    private void SaveEarnedCurrency(){
        playerProfile.currecy += earnedCurrency;
        if(PlayerProfile.maximumCurrencyLimit < playerProfile.currecy){
            playerProfile.currecy = PlayerProfile.maximumCurrencyLimit;
        }

        SaveDataManager.SetClass("Player Profile", playerProfile);
    }

    /// <summary>
    /// �X�R�A�ۑ�
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
    /// �����̍Đ�
    /// </summary>
    /// <param name="audioClip">�Đ�����N���b�v</param>
    public void PlayAudio(AudioClip audioClip){
        if(audioClip != null){
            audioSources[1].PlayOneShot(audioClip);
        }
    }

}
