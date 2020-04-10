//================================================================================
//
//  EnemyBehaviour
//
//  敵キャラクターの基底クラス
//
//================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyBehaviour : CharacterBehavior{

    /**************************************************
        Enumerations
    **************************************************/

    /// <summary>
    /// 動きの種類
    /// </summary>
    public enum Type{
        Standing,
        Flying
    }

    /**************************************************
        Fields / Properties
    **************************************************/

    /// <summary>
    /// キャラクター名
    /// </summary>
    [field: SerializeField, RenameField("Name")]
    protected string name{
        get;
        set;
    }
    
    /// <summary>
    /// 種類
    /// </summary>
    protected Type type{
        get;
        set;
    }

    /// <summary>
    /// 減少した体力の何割毎にコインを落とすか
    /// </summary>
    [field: SerializeField, RenameField("Coin Drop Health Split")]
    float coinDropHealthSplit{
        get;
        set;
    }

    /// <summary>
    /// コインドロップのフラグ
    /// </summary>
    bool[] coinDropFlags{
        get;
        set;
    }

    /// <summary>
    /// 爆発のパーティクル
    /// </summary>
    [field: SerializeField, RenameField("Explosion Particle")]
    private GameObject explosionParticle{
        get;
        set;
    }

    /// <summary>
    /// 被ダメージ時の効果音
    /// </summary>
    [field: SerializeField, RenameField("Hit Sound")]
    protected AudioClip hitSound{
        get;
        set;
    }

    /// <summary>
    /// 撃破時の効果音
    /// </summary>
    [field: SerializeField, RenameField("Destroy Sound")]
    protected AudioClip destroySound{
        get;
        set;
    }

    /**************************************************
        Unity Event Functions
    **************************************************/

    override protected void Awake(){
        base.Awake();

        coinDropFlags = new bool[(int)coinDropHealthSplit];
        for(int count = 0; count < coinDropHealthSplit; count++){
            coinDropFlags[count] = false;
        }

        transform.GetComponent<Rigidbody2D>().velocity = Vector2.left * (GameManager.instance.currentGameSpeed);
    }


    override public void OnTriggerExit2D(Collider2D collision){
        base.OnTriggerExit2D(collision);

        switch(collision.gameObject.tag){
            case "Game Area":
                Destroy(transform.gameObject);

                break;
        }
    }

    /**************************************************
        User Defined Functions
    **************************************************/

    /// <summary>
    /// 落下処理
    /// </summary>
    protected override void Fall(){
        if(type == Type.Standing){
            base.Fall();
        }
    }

    /// <summary>
    /// 被ダメージ処理
    /// </summary>
    public override void TakeDamage(int damage, Vector3 direction){
        base.TakeDamage(damage, direction);

        transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = (float)currentHealth / (float)maximumHealth;
        GameManager.instance.PlayAudio(hitSound);

        int coinDropHealth = maximumHealth;
        for(int count = 0; count < coinDropHealthSplit; count++){
            coinDropHealth -= (int)((float)maximumHealth / coinDropHealthSplit);

            if(currentHealth <= coinDropHealth){
                if(coinDropFlags[count] == false){
                    DropCoin(direction);
                }

                coinDropFlags[count] = true;
            }
        }
    }

    /// <summary>
    /// コインのドロップ
    /// </summary>
    private void DropCoin(Vector3 direction){
        GameObject coin = Instantiate(GameManager.instance.coinPrefab, transform.position, Quaternion.identity);

        Quaternion angle = Quaternion.Euler(direction);
        angle = Quaternion.AngleAxis(UnityEngine.Random.Range(-GameManager.instance.coinSpreadRange / 2, GameManager.instance.coinSpreadRange / 2), Vector3.forward) * angle;

        coin.GetComponent<CoinBehaviour>().Pop(angle.eulerAngles);
    }

    /// <summary>
    /// 撃破処理
    /// </summary>
    override protected void Down(){
        DropRandomItem();
        GameManager.instance.DefeatEnemy();

        EmitExplosionParticle();
        GameManager.instance.PlayAudio(destroySound);

        base.Down();
    }

    /// <summary>
    /// ランダムなアイテムのドロップ
    /// </summary>
    private void DropRandomItem(){
        GameObject dropPrefab = GameManager.instance.dropItemPrefabs[UnityEngine.Random.Range(0, GameManager.instance.dropItemPrefabs.Length)];

        if(UnityEngine.Random.value <= GameManager.instance.itemDropChance){
            DropItem(dropPrefab);
        }
    }

    /// <summary>
    /// アイテムのドロップ
    /// </summary>
    private void DropItem(GameObject itemPrefab){
        GameObject item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        item.GetComponent<DropItemBehaviour>().Pop(Vector3.up);
    }

    /// <summary>
    /// パーティクルの発生
    /// </summary>
    private void EmitExplosionParticle(){
        GameObject particle = Instantiate(explosionParticle, transform.position, Quaternion.identity, transform.parent);
        Destroy(particle, 1.0f);
    }

}
