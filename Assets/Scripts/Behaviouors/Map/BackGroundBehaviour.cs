//================================================================================
//
//  BackGroundBehaviour
//
//  背景の建物の挙動や設定
//
//================================================================================

using UnityEngine;

public class BackGroundBehaviour : MonoBehaviour{

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
    /// ゲームスピードに従った移動
    /// </summary>
    private void Move(){
        transform.position = (Vector2)transform.position + gameVelocity * GameManager.instance.backgroundSpeedMultiplier * Time.fixedDeltaTime;
    }

    /// <summary>
    /// 画面(ゲームエリア)外に出た際の処理
    /// </summary>
    /// <param name="collision">離れたコリジョン</param>
    public void OnTriggerExit2D(Collider2D collision){
        if(collision.gameObject.tag == "Game Area"){
            Destroy(transform.gameObject);
        }
    }
}
