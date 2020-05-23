//================================================================================
//
//  RigidbodyBehaviour
//
//  剛体の基底クラス
//
//================================================================================

using UnityEngine;

public class RigidbodyBehaviour : MonoBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    /// <summary>
    /// 移動速度
    /// </summary>
    virtual protected Vector2 movementVelocity{
        get{
            return gameVelocity + fallVelocity;
        }
    }

    /// <summary>
    /// ゲームスピード
    /// </summary>
    protected Vector2 gameVelocity{
        get{
            return Vector2.left * GameManager.instance.currentGameSpeed;
        }
    }

    /// <summary>
    /// 落下速度
    /// </summary>
    virtual protected Vector2 fallVelocity{
        get{
            return Vector2.down * fallSpeed;
        }
    }

    /// <summary>
    /// 落下の速さ
    /// </summary>
    protected float fallSpeed{
        get;
        set;
    }

    /// <summary>
    /// 落下の加速度
    /// </summary>
    [field: Header("Movement")]
    [field: SerializeField, RenameField("Fall Acceleration")]
    protected float fallAcceleration{
        get;
        set;
    }

    /// <summary>
    /// 落下速度の上限
    /// </summary>
    [field: SerializeField, RenameField("Fall Speed Limit")]
    protected float fallSpeedLimit{
        get;
        set;
    }

    /// <summary>
    /// 乗っている足場の数のバッキングフィールド
    /// </summary>
    private int standingPlatformCount_value;
    /// <summary>
    /// 乗っている足場の数
    /// </summary>
    protected int standingPlatformCount{
        get{
            return standingPlatformCount_value;
        }
        set{
            standingPlatformCount_value = value;

            if(standingPlatformCount_value < 0){
                standingPlatformCount_value = 0;
            }
        }
    }

    /// <summary>
    /// 着地しているか
    /// </summary>
    protected bool isLanding{
        get{
            return 0 < standingPlatformCount_value;
        }
    }

    /// <summary>
    /// 足場への埋没許容量
    /// </summary>
    [field: SerializeField, RenameField("Embedding Tolerance")]
    protected float embeddingTolerance{
        get;
        set;
    }

    /// <summary>
    /// 足場に埋まっているか
    /// </summary>
    protected bool isEmbedding{
        get;
        set;
    }

    /// <summary>
    /// 壁に埋まっているか
    /// </summary>
    protected GameObject EmbeddingWall{
        get;
        set;
    }

    /// <summary>
    /// 衝突検知範囲
    /// </summary>
    [field: SerializeField, RenameField("Collision Detection Range")]
    private float collisionDetectionRange{
        get;
        set;
    }

    /**************************************************
        Unity Event Functions
    **************************************************/

    virtual protected void Awake(){
        Initialize();
    }

    virtual protected void Update(){

    }

    virtual protected void FixedUpdate(){
        Fall();
        Move();
    }

    virtual public void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Platform"){
            float footPosition = transform.position.y - transform.GetComponent<BoxCollider2D>().bounds.size.y / 2;
            float groundPositon = collision.transform.position.y + collision.transform.GetComponent<BoxCollider2D>().bounds.size.y / 2;
            isEmbedding = false;

            if(footPosition - groundPositon < 0 && embeddingTolerance < Mathf.Abs(footPosition - groundPositon)){
                isEmbedding = true;
                EmbeddingWall = collision.gameObject;
            }
            else{
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);
                transform.position = new Vector2(transform.position.x, collision.transform.position.y + transform.GetComponent<BoxCollider2D>().bounds.size.y / 2 + collision.transform.GetComponent<BoxCollider2D>().bounds.size.y / 2);

                standingPlatformCount++;
            }
        }
    }

    virtual public void OnTriggerExit2D(Collider2D collision){
        if(collision.gameObject.tag == "Platform"){
            if(collision.gameObject == EmbeddingWall){
                EmbeddingWall = null;
            }
            else{
                standingPlatformCount--;
            }
        }
    }

    /**************************************************
        User Defined Functions
    **************************************************/

    /// <summary>
    /// 初期化処理
    /// </summary>
    virtual protected void Initialize(){
        fallSpeed = 0.0f;
    }

    /// <summary>
    /// 移動
    /// </summary>
    virtual protected void Move(){
        GetComponent<Rigidbody2D>().velocity = movementVelocity;
    }

    /// <summary>
    /// 落下速度の計算
    /// </summary>
    virtual protected void Fall(){
        if(!isLanding){
            fallSpeed += fallAcceleration;
            if(fallSpeed > fallSpeedLimit){
                fallSpeed = fallSpeedLimit;
            }
        }
        else{
            fallSpeed = 0;
        }
    }

}
