//================================================================================
//
//  PopupBehaviour
//
//  ポップアップエフェクトの基底クラス
//
//================================================================================

using UnityEngine;

public class PopupBehaviour : MonoBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    /// <summary>
    /// ポップアップ速度
    /// </summary>
    [field: SerializeField, RenameField("Speed")]
    protected float speed{
        get;
        set;
    }

    /// <summary>
    /// タイマー
    /// </summary>
    protected float timer = 1;

    /// <summary>
    /// 表示時間
    /// </summary>
    [field: SerializeField, RenameField("Life Time")]
    protected float lifeTime{
        get;
        set;
    }

    /// <summary>
    /// 初期化されているか
    /// </summary>
    protected bool isInitialized;

    /**************************************************
        Unity Event Functions
    **************************************************/
    
    private void Update(){
        Move();
        Destroy();
    }

    /**************************************************
        User Defined Functions
    **************************************************/

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize(){
        timer = lifeTime;

        isInitialized = true;
    }

    /// <summary>
    /// ポップアップ
    /// </summary>
    virtual protected void Move(){
        if(!isInitialized){
            return;
        }

        transform.Translate(Vector3.up * speed * Time.deltaTime);
        timer -= Time.deltaTime;
    }

    /// <summary>
    /// 破棄処理
    /// </summary>
    private void Destroy(){
        if(timer <= 0){
            Destroy(gameObject);
        }
    }

}
