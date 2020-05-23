//================================================================================
//
//  FloatingEnemyBehaviour
//
//  浮遊型の敵(風船)の挙動
//
//================================================================================

using UnityEngine;

public class FloatingEnemyBehavior : EnemyBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    /// <summary>
    /// 移動速度
    /// </summary>
    override protected Vector2 movementVelocity{
        get{
            return gameVelocity + floatingVelocity;
        }
    }

    /// <summary>
    /// 浮遊速度
    /// </summary>
    private Vector2 floatingVelocity{
        get;
        set;
    }

    /// <summary>
    /// 浮遊の速さ
    /// </summary>
    [field: SerializeField, RenameField("Floating Speed")]
    private float floatingSpeed{
        get;
        set;
    }

    /// <summary>
    /// 曲がる速度
    /// </summary>
    [field: SerializeField, RenameField("Angle Change Speed")]
    private float angleChangeSpeed{
        get;
        set;
    }

    /// <summary>
    /// 浮遊の角度
    /// </summary>
    private float angle{
        get;
        set;
    }

    /**************************************************
        Unity Event Functions
    **************************************************/

    protected override void FixedUpdate(){
        Fly();
        base.FixedUpdate();
    }

    /**************************************************
        User Defined Functions
    **************************************************/

    /// <summary>
    /// 初期化処理
    /// </summary>
    protected override void Initialize(){
        base.Initialize();

        type = Type.Flying;
        angle = 0.0f;
        transform.position = new Vector2(transform.position.x, 1.25f);
    }

    /// <summary>
    /// 浮遊
    /// </summary>
    private void Fly(){
        angle += angleChangeSpeed;
        angle %= 360.0f;

        floatingVelocity = new Vector2(0.0f, floatingSpeed * Mathf.Sin(angle * Mathf.PI / 180.0f));
    }

}
