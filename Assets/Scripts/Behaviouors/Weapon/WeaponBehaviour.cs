//================================================================================
//
//  WeaponBehaviour
//
//  武器の挙動
//
//================================================================================

using System.Collections;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    /// <summary>
    /// 武器のステータス
    /// </summary>
    [SerializeField]
    private Weapon stats;

    /// <summary>
    /// 射撃ディレイ中であるか
    /// </summary>
    private bool isDelayed{
        get;
        set;
    }

    /// <summary>
    /// 射撃ボタン
    /// </summary>
    private InputManager.ButtonState fireButton{
        get{
            return InputManager.instance.buttons[InputManager.ButtonName.Fire];
        }
    }

    /// <summary>
    /// マズルフラッシュのエフェクト
    /// </summary>
    [field: SerializeField, RenameField("Muzzle Flash Effect")]
    private GameObject muzzleFlashEffect{
        get;
        set;
    }

    /// <summary>
    /// 発砲音
    /// </summary>
    [field: SerializeField, RenameField("Fire Sound")]
    public AudioClip fireSound{
        get;
        set;
    }

    /**************************************************
        Unity Event Functions
    **************************************************/

    private void Awake(){
        Initialize();
    }
    
    private void Update(){
        Fire();
    }

    /**************************************************
        Functions
    **************************************************/

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Initialize(){
        isDelayed = false;

        if(GameManager.playerProfile != null){
            stats = GameManager.playerProfile.weapon;
        }
    }

    /// <summary>
    /// 射撃
    /// </summary>
    private void Fire(){
        switch(stats.fireMode){
            case Weapon.FireMode.SemiAutomatic:
                if(fireButton == InputManager.ButtonState.Press && !isDelayed){
                    for(int count = 0; count < stats.projectileCount; count++){
                        GameObject projectile = Instantiate(GameManager.instance.projectilePrefabs[stats.projectile], transform.position, Quaternion.identity);

                        projectile.GetComponent<ProjectileBehaviour>().Initialize(stats.projectileDamage, stats.criticalChance);

                        Quaternion angle = Quaternion.Euler(transform.right);
                        angle = Quaternion.AngleAxis(UnityEngine.Random.Range(-stats.spreadRange / 2, stats.spreadRange / 2), Vector3.forward) * angle;

                        projectile.GetComponent<ProjectileBehaviour>().Shoot(angle.eulerAngles, stats.projectileSpeed);
                    }

                    EmitMuzzleFlashEffect();
                    GameManager.instance.PlayAudio(fireSound);
                    StartCoroutine(Delay());
                }

                break;
            case Weapon.FireMode.Burst:
                if(fireButton == InputManager.ButtonState.Press && !isDelayed){
                    isDelayed = true;
                    StartCoroutine(BurstFire(stats.burstRoundCount, stats.burstShotDelay));
                }

                break;
            case Weapon.FireMode.FullAutomatic:
                if(fireButton == InputManager.ButtonState.Down && !isDelayed){
                    for(int count = 0; count < stats.projectileCount; count++){
                        GameObject projectile = Instantiate(GameManager.instance.projectilePrefabs[stats.projectile], transform.position, transform.rotation);

                        projectile.GetComponent<ProjectileBehaviour>().Initialize(stats.projectileDamage, stats.criticalChance);

                        Quaternion angle = Quaternion.Euler(transform.right);
                        angle = Quaternion.AngleAxis(UnityEngine.Random.Range(-stats.spreadRange / 2, stats.spreadRange / 2), Vector3.forward) * angle;

                        projectile.GetComponent<ProjectileBehaviour>().Shoot(angle.eulerAngles, stats.projectileSpeed);
                    }

                    StartCoroutine(Delay());
                }

                break;
        }
    }

    /// <summary>
    /// バースト射撃(未実装)
    /// </summary>
    /// <param name="roundCount">弾数</param>
    /// <param name="shotDelay">ディレイの長さ</param>
    /// <returns></returns>
    private IEnumerator BurstFire(int roundCount, float shotDelay){
        for(int round = 0; round < roundCount; round++){
            for(int count = 0; count < stats.projectileCount; count++){
                GameObject projectile = Instantiate(GameManager.instance.projectilePrefabs[stats.projectile], transform.position, transform.rotation);

                projectile.GetComponent<ProjectileBehaviour>().Initialize(stats.projectileDamage, stats.criticalChance);

                Quaternion angle = Quaternion.Euler(transform.right);
                angle = Quaternion.AngleAxis(UnityEngine.Random.Range(-stats.spreadRange / 2, stats.spreadRange / 2), Vector3.forward) * angle;

                projectile.GetComponent<ProjectileBehaviour>().Shoot(angle.eulerAngles, stats.projectileSpeed);
            }

            yield return new WaitForSeconds(shotDelay);
        }

        StartCoroutine(Delay());
    }

    /// <summary>
    /// 射撃ディレイ(発砲不可能な時間)の適用
    /// </summary>
    /// <returns></returns>
    private IEnumerator Delay(){
        isDelayed = true;
        yield return new WaitForSeconds(stats.shotDelay);
        isDelayed = false;
    }

    /// <summary>
    /// マズルフラッシュエフェクトの発生
    /// </summary>
    private void EmitMuzzleFlashEffect(){
        GameObject muzzleFlash = Instantiate(muzzleFlashEffect, transform.position, transform.rotation, transform);
        Destroy(muzzleFlash, 0.05f);
    }

}
