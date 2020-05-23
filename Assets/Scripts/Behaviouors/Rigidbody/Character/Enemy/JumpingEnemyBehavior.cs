//================================================================================
//
//  JumpingEnemyBehaviour
//
//  ジャンプ型の敵(樽)の挙動
//
//================================================================================

using System.Collections;
using UnityEngine;

public class JumpingEnemyBehavior : EnemyBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    /// <summary>
    /// 移動速度
    /// </summary>
    override protected Vector2 movementVelocity{
        get{
            return gameVelocity + fallVelocity + runningVelocity + jumpVelocity;
        }
    }

    /// <summary>
    /// 走る速度
    /// </summary>
    private Vector2 runningVelocity{
        get{
            return Vector2.left * runningSpeed;
        }
    }

    /// <summary>
    /// 走る速さ
    /// </summary>
    [field: SerializeField, RenameField("Running Speed")]
    private float runningSpeed{
        get;
        set;
    }

    /// <summary>
    /// ジャンプ速度
    /// </summary>
    private Vector2 jumpVelocity{
        get{
            return Vector2.up * jumpSpeed;
        }
    }

    /// <summary>
    /// ジャンプの速さ
    /// </summary>
    private float jumpSpeed{
        get;
        set;
    }

    /// <summary>
    /// ジャンプ力
    /// </summary>
    [field: SerializeField, RenameField("Jump Strength")]
    private float jumpStrength{
        get;
        set;
    }

    /// <summary>
    /// ジャンプの時間間隔
    /// </summary>
    [field: SerializeField, RenameField("Jump Interval")]
    private float jumpInterval{
        get;
        set;
    }

    /// <summary>
    /// ジャンプ時間
    /// </summary>
    [field: SerializeField, RenameField("Jump Time Length")]
    protected float jumpTimeLength{
        get;
        set;
    }

    /// <summary>
    /// ジャンプ中であるか
    /// </summary>
    private bool isJumping{
        get;
        set;
    }

    /// <summary>
    /// ジャンプ用タイマー
    /// </summary>
    private float jumpTimer{
        get;
        set;
    }

    /// <summary>
    /// ジャンプ開始の入力
    /// </summary>
    private bool jumpStartInput{
        get;
        set;
    }

    /// <summary>
    /// ジャンプ終了の入力
    /// </summary>
    private bool jumpEndInput{
        get;
        set;
    }

    /// <summary>
    /// 転がる速度
    /// </summary>
    [field: SerializeField, RenameField("Roll Speed")]
    private float rollSpeed{
        get;
        set;
    }

    /**************************************************
        Unity Event Functions
    **************************************************/

    override protected void FixedUpdate(){
        DoJump();
        Roll();

        base.FixedUpdate();
    }

    override public void OnTriggerEnter2D(Collider2D collision){
        base.OnTriggerEnter2D(collision);

        switch(collision.gameObject.tag){
            case "Platform":
                jumpSpeed = 0.0f;
                StartCoroutine(Jump());

                break;
        }
    }

    /**************************************************
        User Defined Functions
    **************************************************/

    /// <summary>
    /// 初期化処理
    /// </summary>
    override protected void Initialize(){
        base.Initialize();

        type = Type.Standing;
        jumpSpeed = 0.0f;

        transform.position = new Vector2(transform.position.x, 1.25f);
    }

    /// <summary>
    /// 回転
    /// </summary>
    private void Roll(){
        transform.GetChild(0).Rotate(new Vector3(0f, 0f, rollSpeed));
    }

    /// <summary>
    /// ジャンプ
    /// </summary>
    /// <returns></returns>
    protected IEnumerator Jump(){
        yield return new WaitForSeconds(jumpInterval);

        if(isLanding){
            jumpStartInput = true;
        }
    }

    /// <summary>
    /// ジャンプ
    /// </summary>
    private void DoJump(){
        if (GameManager.instance.isPaused){
            return;
        }

        jumpTimer -= Time.fixedDeltaTime;
        if(jumpTimer <= 0.0f){
            jumpTimer = 0.0f;
        }

        if(jumpStartInput && isLanding && !isJumping){
            fallSpeed = -jumpStrength;
            isJumping = true;
            standingPlatformCount = 0;
            jumpTimer = jumpTimeLength;
            jumpStartInput = false;
        }

        if(isJumping && 0.0f < jumpTimer){
            fallSpeed = -jumpStrength * (0.1f + 0.9f * (jumpTimer / jumpTimeLength));
        }

        if((jumpEndInput || jumpTimer == 0.0f) && isJumping){
            isJumping = false;
            jumpEndInput = false;
        }
    }

}
