//================================================================================
//
//  PlatformBehaviour
//
//  足場の挙動や設定
//
//================================================================================

using UnityEngine;

public class PlatformBehaviour : MonoBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    /// <summary>
    /// ゲームスピード
    /// </summary>
    private Vector2 gameVelocity{
        get{
            return Vector2.left * GameManager.instance.currentGameSpeed;
        }
    }

    /// <summary>
    /// 足場のサイズのバッキングフィールド
    /// </summary>
    private Vector2 size_field;
    /// <summary>
    /// 足場のサイズ
    /// </summary>
    public Vector2 size{
        get{
            return size_field;
        }
        set{
            size_field = value;

            transform.GetComponent<BoxCollider2D>().size = size_field;
            transform.GetChild(0).GetComponent<SpriteRenderer>().size = size_field;
            transform.GetChild(1).GetComponent<SpriteRenderer>().size = new Vector2(size_field.x, 1f);
            transform.GetChild(1).localPosition = new Vector2(0, size_field.y / 2);
            transform.GetChild(2).localScale = new Vector2(size_field.x + 0.05f, size_field.y + 0.25f + 0.05f);
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
        transform.position= (Vector2)transform.position + gameVelocity * Time.fixedDeltaTime;
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
