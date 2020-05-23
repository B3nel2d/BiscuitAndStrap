//================================================================================
//
//  DropItemBehaviour
//
//  ドロップアイテムの基底クラス
//
//================================================================================

using UnityEngine;

public class DropItemBehaviour : RigidbodyBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    /// <summary>
    /// 名前
    /// </summary>
    [field: SerializeField, RenameField("Name")]
    protected new string name{
        get;
        set;
    }

    /// <summary>
    /// 移動速度
    /// </summary>
    protected override Vector2 movementVelocity{
        get{
            if(!isLanding){
                return gameVelocity + fallVelocity + popVelocity;
            }
            else{
                return gameVelocity + fallVelocity;
            }
        }
    }

    /// <summary>
    /// ポップ速度
    /// </summary>
    private Vector2 popVelocity{
        get;
        set;
    }

    /// <summary>
    /// ポップの速さ
    /// </summary>
    [field: SerializeField, RenameField("Pop Speed")]
    protected float popSpeed{
        get;
        set;
    }

    /// <summary>
    /// 跳ねた回数
    /// </summary>
    private int boundCount{
        get;
        set;
    }

    /// <summary>
    /// 跳ねる最大回数
    /// </summary>
    [field: SerializeField, RenameField("Max Bound Count")]
    private int maxBoundCount{
        get;
        set;
    }

    /// <summary>
    /// 取得した際の効果音
    /// </summary>
    [field: SerializeField, RenameField("Get Sound")]
    protected AudioClip getSound{
        get;
        set;
    }

    /**************************************************
        Unity Event Functions
    **************************************************/

    override protected void FixedUpdate() {
        BouncingOffWall();
        base.FixedUpdate();
    }

    override public void OnTriggerEnter2D(Collider2D collision){
        base.OnTriggerEnter2D(collision);
        if(EmbeddingWall == null){
            if(collision.gameObject.tag == "Platform"){
                if(boundCount < maxBoundCount){
                    standingPlatformCount = 0;
                    boundCount++;
                    fallSpeed = 0;
                    popVelocity = new Vector2(popVelocity.x, 5.0f);
                }
            }
        }

        if(collision.gameObject.tag == "Player"){
            GiveEffect(collision.gameObject.GetComponent<PlayerBehaviour>());
            Destroy(gameObject);
        }
    }

    override public void OnTriggerExit2D(Collider2D collision){
        base.OnTriggerExit2D(collision);

        if(collision.gameObject.tag == "Environment Area"){
            Destroy(transform.gameObject);
        }
    }

    /**************************************************
        User Defined Functions
    **************************************************/

    /// <summary>
    /// ポップ
    /// </summary>
    /// <param name="direction">ポップする方向</param>
    public void Pop(Vector3 direction){
        if(1.0f < direction.x){
            direction = new Vector3(direction.x - 360.0f, direction.y, direction.z);
        }
        if(1.0f < direction.y){
            direction = new Vector3(direction.x, direction.y - 360.0f, direction.z);
        }

        popVelocity = direction * popSpeed;
    }

    /// <summary>
    /// 壁に当たって跳ね返る処理
    /// </summary>
    private void BouncingOffWall(){
        if(EmbeddingWall != null){
            popVelocity = new Vector2(-popVelocity.x, popVelocity.y);
        }
    }

    /// <summary>
    /// 効果の付与
    /// </summary>
    /// <param name="target">付与対象</param>
    virtual protected void GiveEffect(PlayerBehaviour target){
        GameManager.instance.PlayAudio(getSound);
    }

}
