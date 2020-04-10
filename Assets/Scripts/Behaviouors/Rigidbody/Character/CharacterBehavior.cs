//================================================================================
//
//  CharacerBehaviour
//
//  �L�����N�^�[�̊��N���X
//
//================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehavior : RigidbodyBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    /// <summary>
    /// �̗͂̍ő�l
    /// </summary>
    [field: Header("Stats")]
    [field: SerializeField, RenameField("Maximum Health")]
    protected int maximumHealth{
        get;
        set;
    }

    /// <summary>
    /// ���݂̗̑�
    /// </summary>
    [field: SerializeField, RenameField("Current Health")]
    private int currentHealth_field;
    protected int currentHealth{
        get{
            return currentHealth_field;
        }
        set{
            if(value <= maximumHealth){
                currentHealth_field = value;
            }
            else{
                currentHealth = maximumHealth;
            }
        }
    }

    /**************************************************
        User Defined Functions
    **************************************************/

    /// <summary>
    /// ����������
    /// </summary>
    override protected void Initialize(){
        base.Initialize();

        currentHealth = maximumHealth;
    }

    /// <summary>
    /// �󂯂��_���[�W�̌v�Z
    /// </summary>
    virtual public void TakeDamage(int damage, Vector3 direction){
        currentHealth -= damage;

        if(currentHealth <= 0){
            currentHealth = 0;
            Down();
        }
    }
    /// <summary>
    /// �󂯂��_���[�W�̌v�Z
    /// </summary>
    virtual public void TakeDamage(int damage){
        TakeDamage(damage, Vector3.zero);
    }

    /// <summary>
    /// ���j���ꂽ�ۂ̏���
    /// </summary>
    virtual protected void Down(){
        Destroy(transform.gameObject);
    }

}
