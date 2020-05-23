//================================================================================
//
//  RunningEnemyBehaviour
//
//  平行移動型の敵(箱)の挙動
//
//================================================================================

using UnityEngine;

public class RunningEnemyBehaviour : EnemyBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    /// <summary>
    /// 移動速度
    /// </summary>
    override protected Vector2 movementVelocity{
        get{
            return gameVelocity + fallVelocity + runningVelocity;
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
    protected float runningSpeed{
        get;
        set;
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
    }

}
