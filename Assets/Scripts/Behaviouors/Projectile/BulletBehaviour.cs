//================================================================================
//
//  BulletBehaviour
//
//  弾丸の挙動
//
//================================================================================

using UnityEngine;

public class BulletBehaviour : ProjectileBehaviour{

    /// <summary>
    /// 発射
    /// </summary>
    /// <param name="direction">向き</param>
    /// <param name="speed">弾速</param>
    public override void Shoot(Vector3 direction, float speed){
        base.Shoot(direction, speed);

        if(1.0f < direction.x){
            direction = new Vector3(direction.x - 360.0f, direction.y, direction.z);
        }
        if(1.0f < direction.y){
            direction = new Vector3(direction.x, direction.y - 360.0f, direction.z);
        }
        
        transform.GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

}
