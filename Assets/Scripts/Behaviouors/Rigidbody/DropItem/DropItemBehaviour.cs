//================================================================================
//
//  DropItemBehaviour
//
//  ドロップアイテムの基底クラス
//
//================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coffee.UIExtensions;

public class DropItemBehaviour : RigidbodyBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    [field: SerializeField, RenameField("Name")]
    protected string name{
        get;
        set;
    }

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

    private Vector2 popVelocity{
        get;
        set;
    }

    [field: SerializeField, RenameField("Pop Speed")]
    protected float popSpeed{
        get;
        set;
    }

    private int boundCount{
        get;
        set;
    }

    [field: SerializeField, RenameField("Max Bound Count")]
    private int maxBoundCount{
        get;
        set;
    }

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
                    //床で跳ねる
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

    public void Pop(Vector3 direction){
        if(1.0f < direction.x){
            direction = new Vector3(direction.x - 360.0f, direction.y, direction.z);
        }
        if(1.0f < direction.y){
            direction = new Vector3(direction.x, direction.y - 360.0f, direction.z);
        }

        popVelocity = direction * popSpeed;
    }

    private void BouncingOffWall() {
        if(EmbeddingWall != null){
            //跳ねる
            popVelocity = new Vector2(-popVelocity.x, popVelocity.y);
        }
    }

    virtual protected void GiveEffect(PlayerBehaviour target){
        GameManager.instance.PlayAudio(getSound);
    }

}
