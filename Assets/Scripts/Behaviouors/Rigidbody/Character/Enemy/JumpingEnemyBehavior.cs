//================================================================================
//
//  JumpingEnemyBehaviour
//
//  ƒWƒƒƒ“ƒvŒ^‚Ì“G(’M)‚Ì‹““®
//
//================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemyBehavior : EnemyBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    override protected Vector2 movementVelocity{
        get{
            return gameVelocity + fallVelocity + runningVelocity + jumpVelocity;
        }
    }

    private Vector2 runningVelocity{
        get{
            return Vector2.left * runningSpeed;
        }
    }

    [field: SerializeField, RenameField("Running Speed")]
    private float runningSpeed{
        get;
        set;
    }

    private Vector2 jumpVelocity{
        get{
            return Vector2.up * jumpSpeed;
        }
    }

    private float jumpSpeed{
        get;
        set;
    }

    [field: SerializeField, RenameField("Jump Strength")]
    private float jumpStrength{
        get;
        set;
    }

    [field: SerializeField, RenameField("Jump Interval")]
    private float jumpInterval{
        get;
        set;
    }

    [field: SerializeField, RenameField("Jump Time Length")]
    protected float jumpTimeLength {
        get;
        set;
    }

    private bool isJumping {
        get;
        set;
    }

    private float jumpTimer {
        get;
        set;
    }

    private bool jumpStartInput {
        get;
        set;
    }

    private bool jumpEndInput {
        get;
        set;
    }

    [field: SerializeField, RenameField("Roll Speed")]
    private float rollSpeed {
        get;
        set;
    }

    /**************************************************
        Unity Event Functions
    **************************************************/
    override protected void FixedUpdate() {
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
    private void Roll() {
        transform.GetChild(0).Rotate(new Vector3(0f, 0f, rollSpeed));
    }

    override protected void Initialize(){
        base.Initialize();

        type = Type.Standing;
        jumpSpeed = 0.0f;
    }

    protected IEnumerator Jump(){
        yield return new WaitForSeconds(jumpInterval);

        if(isLanding){
            jumpStartInput = true;
        }
    }
    private void DoJump() {
        if (GameManager.instance.isPaused) {
            return;
        }

        jumpTimer -= Time.fixedDeltaTime;
        if (jumpTimer <= 0.0f) {
            jumpTimer = 0.0f;
        }

        if (jumpStartInput && isLanding && !isJumping) {
            fallSpeed = -jumpStrength;
            isJumping = true;
            standingPlatformCount = 0;
            jumpTimer = jumpTimeLength;
            jumpStartInput = false;
        }

        if (isJumping && 0.0f < jumpTimer) {
            fallSpeed = -jumpStrength * (0.1f + 0.9f * (jumpTimer / jumpTimeLength));
        }

        if ((jumpEndInput || jumpTimer == 0.0f) && isJumping) {
            isJumping = false;
            jumpEndInput = false;
        }
    }
}
