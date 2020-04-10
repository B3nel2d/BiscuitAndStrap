//================================================================================
//
//  RunningEnemyBehaviour
//
//  平行移動型の敵(箱)の挙動
//
//================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningEnemyBehaviour : EnemyBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    override protected Vector2 movementVelocity{
        get{
            return gameVelocity + fallVelocity + runningVelocity;
        }
    }

    private Vector2 runningVelocity{
        get{
            return Vector2.left * runningSpeed;
        }
    }

    [field: SerializeField, RenameField("Running Speed")]
    protected float runningSpeed{
        get;
        set;
    }

    /**************************************************
        Unity Event Functions
    **************************************************/


    /**************************************************
        User Defined Functions
    **************************************************/

    override protected void Initialize(){
        base.Initialize();

        type = Type.Standing;
    }

}
