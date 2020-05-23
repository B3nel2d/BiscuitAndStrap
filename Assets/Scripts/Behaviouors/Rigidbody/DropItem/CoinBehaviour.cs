//================================================================================
//
//  CoinBehaviour
//
//  コインの挙動
//
//================================================================================

using UnityEngine;

public class CoinBehaviour : DropItemBehaviour{

    /**************************************************
        Enumerations
    **************************************************/

    /// <summary>
    /// コインの種類
    /// </summary>
    public enum Type{
        Bronze,
        Silver,
        Gold
    }

    /**************************************************
        Fields / Properties
    **************************************************/
    
    /// <summary>
    /// 種類
    /// </summary>
    private Type type{
        get;
        set;
    }

    /// <summary>
    /// 回転速度
    /// </summary>
    [field: SerializeField, RenameField("Roll Speed")]
    private float rollSpeed{
        get;
        set;
    }

    /// <summary>
    /// ドロップ時の効果音
    /// </summary>
    [field: SerializeField, RenameField("Coin Drop Sound")]
    public AudioClip coinDropSound{
        get;
        set;
    }

    /**************************************************
        Unity Event Functions
    **************************************************/

    override protected void FixedUpdate(){
        Roll();
        base.FixedUpdate();
    }

    override public void OnTriggerEnter2D(Collider2D collision){
        base.OnTriggerEnter2D(collision);

        if(collision.gameObject.tag == "Platform"){
            GameManager.instance.PlayAudio(coinDropSound);
        }
    }

    /**************************************************
        User Defined Functions
    **************************************************/

    /// <summary>
    /// 初期化処理
    /// </summary>
    protected override void Initialize(){
        base.Initialize();

        float randomValue = UnityEngine.Random.value;
        if(randomValue <= 0.6f){
            type = Type.Bronze;
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.0f, 0.0f);
        }
        else if(randomValue <= 0.9f){
            type = Type.Silver;
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.gray;
        }
        else if(randomValue < 1.0f){
            type = Type.Gold;
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }

    /// <summary>
    /// 回転
    /// </summary>
    private void Roll(){
        if(!isLanding){
            transform.GetChild(0).Rotate(0f, 0f, rollSpeed);
        }
        else{
            transform.GetChild(0).rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    /// <summary>
    /// 効果の付与
    /// </summary>
    /// <param name="target">付与対象</param>
    protected override void GiveEffect(PlayerBehaviour target){
        target.GetCoin(type);
        base.GiveEffect(target);
    }
}
