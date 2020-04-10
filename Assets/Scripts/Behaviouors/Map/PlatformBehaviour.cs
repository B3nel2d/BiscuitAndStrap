//================================================================================
//
//  PlatformBehaviour
//
//  ����̋�����ݒ�
//
//================================================================================

using UnityEngine;

public class PlatformBehaviour : MonoBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    /// <summary>
    /// �Q�[���X�s�[�h
    /// </summary>
    private Vector2 gameVelocity{
        get{
            return Vector2.left * GameManager.instance.currentGameSpeed;
        }
    }

    /// <summary>
    /// ����̑傫���̃o�b�L���O�t�B�[���h
    /// </summary>
    private Vector2 size_field;
    /// <summary>
    /// ����̑傫��
    /// </summary>
    public Vector2 size{
        get{
            return size_field;
        }
        set{
            //�֌W����e�R���|�[�l���g�̒l����x�ɕύX
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
    /// �Q�[���X�s�[�h�ɉ������ړ�
    /// </summary>
    private void Move(){
        transform.position= (Vector2)transform.position + gameVelocity * Time.fixedDeltaTime;
    }

    /// <summary>
    /// ��ʊO�ɏo���ۂ̏���
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerExit2D(Collider2D collision){
        if(collision.gameObject.tag == "Game Area"){
            Destroy(transform.gameObject);
        }
    }

}
