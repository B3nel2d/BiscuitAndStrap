//================================================================================
//
//  FloatingEnemyBehaviour
//
//  浮遊型の敵(風船)の挙動
//
//================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingEnemyBehavior : EnemyBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    override protected Vector2 movementVelocity{
        get{
            return gameVelocity + floatingVelocity;
        }
    }

    private Vector2 floatingVelocity{
        get;
        set;
    }

    [field: SerializeField, RenameField("Floating Speed")]
    private float floatingSpeed{
        get;
        set;
    }

    [field: SerializeField, RenameField("Angle Change Speed")]
    private float angleChangeSpeed {
        get;
        set;
    }

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

    protected override void Initialize(){
        base.Initialize();

        type = Type.Flying;
        angle = 0.0f;
        transform.position = new Vector2(transform.position.x, 1.25f);
    }

    private void Fly(){
        angle += angleChangeSpeed;
        angle %= 360.0f;

        floatingVelocity = new Vector2(0.0f, floatingSpeed * Mathf.Sin(angle * Mathf.PI / 180.0f));
    }

}
