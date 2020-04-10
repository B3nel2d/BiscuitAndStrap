//================================================================================
//
//  StreetLampBehaviour
//
//  街灯の挙動や設定
//
//================================================================================

using UnityEngine;

public class StreetLampBehaviour : MonoBehaviour{

    /// <summary>
    /// ゲームスピード
    /// </summary>
    private Vector2 gameVelocity{
        get{
            return Vector2.left * GameManager.instance.currentGameSpeed;
        }
    }

    /**************************************************
        Unity Event Functions
    **************************************************/

    private void FixedUpdate(){
        Move();
    }

    /**************************************************
        User Defined Functions
    **************************************************/

    /// <summary>
    /// ゲームスピードに沿った移動
    /// </summary>
    private void Move(){
        transform.position = (Vector2)transform.position + gameVelocity * Time.fixedDeltaTime;
    }

    /// <summary>
    /// 画面外に出た際の処理
    /// </summary>
    public void OnTriggerExit2D(Collider2D collision){
        if(collision.gameObject.tag == "Environment Area"){
            Destroy(transform.gameObject);
        }
    }

}
