//================================================================================
//
//  JumpingEnemyBehaviour
//
//  �W�����v�^�̓G(�M)�̋���
//
//================================================================================

using System.Collections;
using UnityEngine;

public class JumpingEnemyBehavior : EnemyBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    /// <summary>
    /// �ړ����x
    /// </summary>
    override protected Vector2 movementVelocity{
        get{
            return gameVelocity + fallVelocity + runningVelocity + jumpVelocity;
        }
    }

    /// <summary>
    /// ���鑬�x
    /// </summary>
    private Vector2 runningVelocity{
        get{
            return Vector2.left * runningSpeed;
        }
    }

    /// <summary>
    /// ���鑬��
    /// </summary>
    [field: SerializeField, RenameField("Running Speed")]
    private float runningSpeed{
        get;
        set;
    }

    /// <summary>
    /// �W�����v���x
    /// </summary>
    private Vector2 jumpVelocity{
        get{
            return Vector2.up * jumpSpeed;
        }
    }

    /// <summary>
    /// �W�����v�̑���
    /// </summary>
    private float jumpSpeed{
        get;
        set;
    }

    /// <summary>
    /// �W�����v��
    /// </summary>
    [field: SerializeField, RenameField("Jump Strength")]
    private float jumpStrength{
        get;
        set;
    }

    /// <summary>
    /// �W�����v�̎��ԊԊu
    /// </summary>
    [field: SerializeField, RenameField("Jump Interval")]
    private float jumpInterval{
        get;
        set;
    }

    /// <summary>
    /// �W�����v����
    /// </summary>
    [field: SerializeField, RenameField("Jump Time Length")]
    protected float jumpTimeLength{
        get;
        set;
    }

    /// <summary>
    /// �W�����v���ł��邩
    /// </summary>
    private bool isJumping{
        get;
        set;
    }

    /// <summary>
    /// �W�����v�p�^�C�}�[
    /// </summary>
    private float jumpTimer{
        get;
        set;
    }

    /// <summary>
    /// �W�����v�J�n�̓���
    /// </summary>
    private bool jumpStartInput{
        get;
        set;
    }

    /// <summary>
    /// �W�����v�I���̓���
    /// </summary>
    private bool jumpEndInput{
        get;
        set;
    }

    /// <summary>
    /// �]���鑬�x
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
    /// ����������
    /// </summary>
    override protected void Initialize(){
        base.Initialize();

        type = Type.Standing;
        jumpSpeed = 0.0f;

        transform.position = new Vector2(transform.position.x, 1.25f);
    }

    /// <summary>
    /// ��]
    /// </summary>
    private void Roll(){
        transform.GetChild(0).Rotate(new Vector3(0f, 0f, rollSpeed));
    }

    /// <summary>
    /// �W�����v
    /// </summary>
    /// <returns></returns>
    protected IEnumerator Jump(){
        yield return new WaitForSeconds(jumpInterval);

        if(isLanding){
            jumpStartInput = true;
        }
    }

    /// <summary>
    /// �W�����v
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
