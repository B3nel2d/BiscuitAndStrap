//================================================================================
//
//  PlayerBehaviour
//
//  プレイヤーキャラクターの挙動
//
//================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : CharacterBehavior{

    /**************************************************
        Enumerations
    **************************************************/

    /// <summary>
    /// アニメーションの状態
    /// </summary>
    public enum AnimationState{
        Idle,
        Running,
        Jumping,
        Falling,
        Rolling
    }

    /**************************************************
        Fields / Properties
    **************************************************/

    //  States
    //==============================

    /// <summary>
    /// ジャンプ中であるか
    /// </summary>
    private bool isJumping{
        get;
        set;
    }

    /// <summary>
    /// ロール中であるか
    /// </summary>
    private bool isRolling{
        get;
        set;
    }

    /// <summary>
    /// 無敵状態であるか
    /// </summary>
    private bool isInvincible{
        get;
        set;
    }

    //  Movement
    //==============================

    /// <summary>
    /// 移動速度
    /// </summary>
    protected override Vector2 movementVelocity{
        get{
            if (EmbeddingWall == null && transform.position.x < -6) {
                return fallVelocity + Vector2.right * 1f;
            }
            return fallVelocity;
        }
    }

    /// <summary>
    /// ジャンプ力
    /// </summary>
    [field: Header("Movement")]
    [field: SerializeField, RenameField("Jump Strength")]
    private float jumpStrength{
        get;
        set;
    }

    /// <summary>
    /// ジャンプの長さ
    /// </summary>
    [field: SerializeField, RenameField("Jump Time Length")]
    protected float jumpTimeLength{
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
    /// ロールの速度
    /// </summary>
    [field: SerializeField, RenameField("Roll Speed")]
    private float rollSpeed{
        get;
        set;
    }

    /// <summary>
    /// ロールのクールダウン時間
    /// </summary>
    [field: SerializeField, RenameField("Roll Cooldown")]
    private float rollCooldown{
        get;
        set;
    }

    /// <summary>
    /// 視点回転の速度
    /// </summary>
    [field: SerializeField, RenameField("Weapon Rotation Speed")]
    private float weaponRotationSpeed{
        get;
        set;
    }

    //  Inputs
    //==============================

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
    /// ジャンプボタン
    /// </summary>
    private InputManager.ButtonState jumpButton{
        get{
            return InputManager.instance.buttons[InputManager.ButtonName.Jump];
        }
    }

    //  Particle
    //==============================

    /// <summary>
    /// 爆発のパーティクル
    /// </summary>
    [field: Header("Particle")]
    [field: SerializeField, RenameField("Explosion Particle")]
    private GameObject explosionParticle{
        get;
        set;
    }

    //  Audio
    //==============================

    /// <summary>
    /// 着地時の効果音
    /// </summary>
    [field: Header("Audio")]
    [field: SerializeField, RenameField("Landing Sound")]
    public AudioClip landingSound{
        get;
        set;
    }

    /// <summary>
    /// プレイヤーがダメージを受けた際の効果音
    /// </summary>
    [field: SerializeField, RenameField("Player Damage Sound")]
    public AudioClip playerDamageSound{
        get;
        set;
    }

    //  References
    //==============================

    /// <summary>
    /// 頭
    /// </summary>
    [field: Header("References")]
    [field: SerializeField, RenameField("Head")]
    private GameObject head{
        get;
        set;
    }

    /// <summary>
    /// 左腕
    /// </summary>
    [field: SerializeField, RenameField("LeftArm")]
    private GameObject leftArm{
        get;
        set;
    }

    /// <summary>
    /// 右腕
    /// </summary>
    [field: SerializeField, RenameField("RightArm")]
    private GameObject rightArm{
        get;
        set;
    }

    /// <summary>
    /// アニメーター
    /// </summary>
    [field: SerializeField, RenameField("Animation")]
    private Animator animator{
        get;
        set;
    }

    /**************************************************
        Unity Event Functions
    **************************************************/

    override protected void Update(){
        GetInputs();
        ChangeAnimationSpeed();
        RotateArms();

        TransitAnimation();
        ChangeAnimationSpeed();
    }

    override protected void FixedUpdate(){
        Jump();
        //Roll();

        base.FixedUpdate();

        GetPushedByWall();
    }

    public override void OnTriggerEnter2D(Collider2D collision){
        base.OnTriggerEnter2D(collision);

        switch(collision.gameObject.tag){
            case "Enemy":
                TakeDamage(1);
                EmitExplosionParticle();

                break;
            case "Platform":
                if(standingPlatformCount == 1){
                    GameManager.instance.PlayAudio(landingSound);
                }

                break;
        }
    }

    override public void OnTriggerExit2D(Collider2D collision) {
        base.OnTriggerExit2D(collision);

        switch(collision.gameObject.tag){
            case "Game Area":
                GameManager.instance.PlayAudio(playerDamageSound);
                Down();

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

        isRolling = false;
        isInvincible = false;

        UIManager.instance.UpdateHealthText(currentHealth);
    }

    /// <summary>
    /// 入力の取得
    /// </summary>
    private void GetInputs(){
        if(jumpButton == InputManager.ButtonState.Press){
            jumpStartInput = true;
        }
        else{
            jumpStartInput = false;
        }

        if(jumpButton == InputManager.ButtonState.Up){
            jumpEndInput = true;
        }
        else{
            jumpEndInput = false;
        }
    }

    /// <summary>
    /// ジャンプ
    /// </summary>
    private void Jump(){
        if(GameManager.instance.isPaused){
            return;
        }

        jumpTimer -= Time.fixedDeltaTime;
        if(jumpTimer <= 0.0f){
            jumpTimer = 0.0f;
        }

        //ジャンプ開始
        if(jumpStartInput && isLanding && !isJumping){
            fallSpeed = -jumpStrength;
            isJumping = true;
            standingPlatformCount = 0;
            jumpTimer = jumpTimeLength;
            jumpStartInput = false;
        }

        //ボタンが離されるか一定時間過ぎたら急に減速
        if(jumpButton == InputManager.ButtonState.Down && isJumping && 0.0f < jumpTimer){
            fallSpeed = -jumpStrength * (0.1f + 0.9f * (jumpTimer / jumpTimeLength));
        }


        if((jumpEndInput || jumpTimer == 0.0f) && isJumping){
            isJumping = false;
            jumpEndInput = false;
        }
    }

    private void Roll(){
        if(Input.GetKeyDown(KeyCode.LeftShift) && isLanding && !isRolling){
            StartCoroutine(Roll(rollSpeed, 0.3f));
        }
    }

    private IEnumerator Roll(float speed, float time){
        isRolling = true;
        GameManager.instance.additionalGameSpeed += speed;

        yield return new WaitForSeconds(time);

        GameManager.instance.additionalGameSpeed = 0.0f;

        yield return new WaitForSeconds(rollCooldown);

        isRolling = false;
    }

    /// <summary>
    /// 壁に押される処理
    /// </summary>
    private void GetPushedByWall(){
        if(EmbeddingWall != null){
            transform.position = new Vector2(EmbeddingWall.transform.position.x - EmbeddingWall.transform.GetComponent<BoxCollider2D>().bounds.size.x / 2.0f - transform.GetComponent<BoxCollider2D>().bounds.size.x / 2.0f, transform.position.y);
        }
    }

    /// <summary>
    /// 腕(照準)の回転
    /// </summary>
    private void RotateArms(){
        if(GameManager.instance.isPaused){
            return;
        }

        switch(GameManager.instance.targetPlatform){
            case GameManager.TargetPlatform.PC:
                switch(GameManager.instance.rotationStyle){
                    case GameManager.RotationStyle.RotatePlayer:
                        if(Input.GetKey(KeyCode.RightArrow)){
                            rightArm.transform.Rotate(Vector3.back * weaponRotationSpeed);
                            head.transform.Rotate(Vector3.back * (weaponRotationSpeed * 40.0f / 100.0f));
                        }
                        if(Input.GetKey(KeyCode.LeftArrow)){
                            rightArm.transform.Rotate(Vector3.forward * weaponRotationSpeed);
                            head.transform.Rotate(Vector3.forward * (weaponRotationSpeed * 40.0f / 100.0f));
                        }

                        break;
                    case GameManager.RotationStyle.RotateWorld:
                        if(Input.GetKey(KeyCode.RightArrow)){
                            GameManager.instance.cameraAxis.transform.Rotate(Vector3.back * weaponRotationSpeed);
                            rightArm.transform.Rotate(Vector3.back * weaponRotationSpeed);
                        }
                        if(Input.GetKey(KeyCode.LeftArrow)){
                            GameManager.instance.cameraAxis.transform.Rotate(Vector3.forward * weaponRotationSpeed);
                            rightArm.transform.Rotate(Vector3.forward * weaponRotationSpeed);
                        }

                        break;
                }

                break;
            case GameManager.TargetPlatform.Mobile:
                Quaternion gyroRotation = Quaternion.Euler(Vector3.back * 180.0f) * Quaternion.Euler(Vector3.left * 90.0f) * Input.gyro.attitude * Quaternion.Euler(Vector3.forward * 180.0f);

                switch(GameManager.instance.rotationStyle){
                    case GameManager.RotationStyle.RotatePlayer:
                        rightArm.transform.rotation = Quaternion.Euler(Vector3.forward * CorrectAngle(gyroRotation.eulerAngles.z - 32.0f));
                        head.transform.localRotation = Quaternion.Euler(Vector3.forward * CorrectAngle(gyroRotation.eulerAngles.z - 20.0f));

                        break;
                    case GameManager.RotationStyle.RotateWorld:
                        GameManager.instance.cameraAxis.transform.rotation = Quaternion.Euler(0, 0, gyroRotation.eulerAngles.z);
                        rightArm.transform.rotation = Quaternion.Euler(0, 0, gyroRotation.eulerAngles.z);

                        break;
                }

                break;
        }

        float armAngle = CorrectAngle(rightArm.transform.localRotation.eulerAngles.z - 247.0f);
        if(50.0f < armAngle && armAngle <= 180.0f){
            rightArm.transform.localRotation = Quaternion.Euler(Vector3.forward * CorrectAngle(50.0f + 247.0f));
        }
        else if(180.0f < armAngle && armAngle < 310.0f){
            rightArm.transform.localRotation = Quaternion.Euler(Vector3.forward * CorrectAngle(310.0f + 247.0f));
        }

        float headAngle = CorrectAngle(head.transform.localRotation.eulerAngles.z + 20.0f);
        if(20.0f < headAngle && headAngle <= 180.0f){
            head.transform.localRotation = Quaternion.Euler(Vector3.forward * CorrectAngle(20.0f - 20.0f));
        }
        else if(180.0f < headAngle && headAngle < 340.0f){
            head.transform.localRotation = Quaternion.Euler(Vector3.forward * CorrectAngle(340.0f - 20.0f));
        }
    }

    /// <summary>
    /// 角度の補正
    /// </summary>
    private float CorrectAngle(float angle){
        while(angle < 0.0f){
            angle += 360.0f; 
        }
        while(360.0f < angle){
            angle -= 360.0f;
        }

        return angle;
    }

    /// <summary>
    /// 被ダメージ処理
    /// </summary>
    public override void TakeDamage(int damage, Vector3 direction){
        if(isInvincible){
            return;
        }

        GameManager.instance.PlayAudio(playerDamageSound);

        base.TakeDamage(damage, direction);

        StartCoroutine(BecomeInvincible(2.0f));
        UIManager.instance.UpdateHealthText(currentHealth);
    }

    private void EmitExplosionParticle(){
        GameObject particle = Instantiate(explosionParticle, transform.position, Quaternion.identity, transform.parent);
        Destroy(particle, 1.0f);
    }

    /// <summary>
    /// 回復
    /// </summary>
    /// <param name="healingAmount">回復量</param>
    public void GetHealed(int healingAmount){
        currentHealth += healingAmount;
        UIManager.instance.UpdateHealthText(currentHealth);
    }

    /// <summary>
    /// コインの取得
    /// </summary>
    public void GetCoin(CoinBehaviour.Type type){
        GameManager.instance.GetCoin(type);
    }

    /// <summary>
    /// 指定時間分の無敵化
    /// </summary>
    private IEnumerator BecomeInvincible(float time){
        isInvincible = true;

        for(int count = 0; count < 18; count++){
            yield return new WaitForSeconds(time / 18.0f);
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = !transform.GetChild(0).GetComponent<SpriteRenderer>().enabled;
        }

        isInvincible = false;
    }

    /// <summary>
    /// ゲームオーバー処理
    /// </summary>
    protected override void Down(){
        GameManager.instance.FinishGame();
        base.Down();
    }

    /// <summary>
    /// アニメーションの遷移
    /// </summary>
    private void TransitAnimation(){
        if(isLanding){
            if(EmbeddingWall != null){
                animator.SetInteger("AnimationState", (int)AnimationState.Idle);
            }
            else{
                animator.SetInteger("AnimationState", (int)AnimationState.Running);
            }
        }
        else{
            if(0.0f < movementVelocity.y){
                animator.SetInteger("AnimationState", (int)AnimationState.Jumping);
            }
            else{
                animator.SetInteger("AnimationState", (int)AnimationState.Falling);
            }
        }
    }

    /// <summary>
    /// アニメーション速度の変更
    /// </summary>
    private void ChangeAnimationSpeed(){
        animator.SetFloat("Speed", GameManager.instance.baseGameSpeed / GameManager.instance.maximumGameSpeed);
    }

}
