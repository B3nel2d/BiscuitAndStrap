//================================================================================
//
//  WeaponBehaviour
//
//  ����̋���
//
//================================================================================

using System.Collections;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    /// <summary>
    /// ����̃X�e�[�^�X
    /// </summary>
    [SerializeField]
    private Weapon stats;

    /// <summary>
    /// �ˌ��f�B���C���ł��邩
    /// </summary>
    private bool isDelayed{
        get;
        set;
    }

    /// <summary>
    /// �ˌ��{�^��
    /// </summary>
    private InputManager.ButtonState fireButton{
        get{
            return InputManager.instance.buttons[InputManager.ButtonName.Fire];
        }
    }

    /// <summary>
    /// �}�Y���t���b�V���̃G�t�F�N�g
    /// </summary>
    [field: SerializeField, RenameField("Muzzle Flash Effect")]
    private GameObject muzzleFlashEffect{
        get;
        set;
    }

    /// <summary>
    /// ���C��
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
    /// ����������
    /// </summary>
    private void Initialize(){
        isDelayed = false;

        if(GameManager.playerProfile != null){
            stats = GameManager.playerProfile.weapon;
        }
    }

    /// <summary>
    /// �ˌ�
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
    /// �o�[�X�g�ˌ�(������)
    /// </summary>
    /// <param name="roundCount">�e��</param>
    /// <param name="shotDelay">�f�B���C�̒���</param>
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
    /// �ˌ��f�B���C(���C�s�\�Ȏ���)�̓K�p
    /// </summary>
    /// <returns></returns>
    private IEnumerator Delay(){
        isDelayed = true;
        yield return new WaitForSeconds(stats.shotDelay);
        isDelayed = false;
    }

    /// <summary>
    /// �}�Y���t���b�V���G�t�F�N�g�̔���
    /// </summary>
    private void EmitMuzzleFlashEffect(){
        GameObject muzzleFlash = Instantiate(muzzleFlashEffect, transform.position, transform.rotation, transform);
        Destroy(muzzleFlash, 0.05f);
    }

}
