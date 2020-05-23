//================================================================================
//
//  DamagePopupBehaviour
//
//  ダメージポップアップの挙動
//
//================================================================================

using UnityEngine;
using TMPro;

public class DamagePopupBehaviour : PopupBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    /// <summary>
    /// ダメージテキスト
    /// </summary>
    [field: SerializeField, RenameField("Text Mesh")]
    private TextMeshPro textMesh{
        get;
        set;
    }

    /// <summary>
    /// ダメージ量
    /// </summary>
    public int damage{
        get;
        set;
    }

    /// <summary>
    /// テキストの色
    /// </summary>
    private Color color;

    /// <summary>
    /// 通常の色
    /// </summary>
    [field: SerializeField, RenameField("Normal Color")]
    private Color normalColor{
        get;
        set;
    }

    /// <summary>
    /// クリティカル時の色
    /// </summary>
    [field: SerializeField, RenameField("Critical Color")]
    private Color criticalColor{
        get;
        set;
    }

    /**************************************************
        User Defined Functions
    **************************************************/

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    /// <param name="critical">クリティカルであるか</param>
    public void Initialize(float damage, bool critical){
        textMesh.text = damage.ToString();
        timer = lifeTime;

        if(critical){
            color = criticalColor;
        }
        else{
            color = normalColor;
        }

        textMesh.color = color;

        isInitialized = true;
    }

    /// <summary>
    /// ポップアップ
    /// </summary>
    override protected void Move(){
        base.Move();

        textMesh.color = new Color(color.a, color.g, color.b, timer / lifeTime);
    }

}
