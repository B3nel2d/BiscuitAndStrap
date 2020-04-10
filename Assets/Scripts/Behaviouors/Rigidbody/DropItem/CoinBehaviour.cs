//================================================================================
//
//  CoinBehaviour
//
//  コインの挙動
//
//================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : DropItemBehaviour{

    /**************************************************
        Enumerations
    **************************************************/

    public enum Type{
        Bronze,
        Silver,
        Gold
    }

    /**************************************************
        Fields / Properties
    **************************************************/
    
    private Type type{
        get;
        set;
    }

    [field: SerializeField, RenameField("Roll Speed")]
    private float rollSpeed{
        get;
        set;
    }

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

    /**************************************************
        User Defined Functions
    **************************************************/

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
    override public void OnTriggerEnter2D(Collider2D collision){
        base.OnTriggerEnter2D(collision);

        if(collision.gameObject.tag == "Platform"){
            GameManager.instance.PlayAudio(coinDropSound);
        }
    }

    private void Roll(){
        if(!isLanding){
            transform.GetChild(0).Rotate(0f, 0f, rollSpeed);
        }
        else{
            transform.GetChild(0).rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    protected override void GiveEffect(PlayerBehaviour target){
        target.GetCoin(type);
        base.GiveEffect(target);
    }
}
