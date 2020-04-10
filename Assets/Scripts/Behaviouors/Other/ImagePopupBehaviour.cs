//================================================================================
//
//  ImageBehaviour
//
//  画像ポップアップの挙動
//
//================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagePopupBehaviour : PopupBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    /// <summary>
    /// 画像のスプライト
    /// </summary>
    [field: SerializeField, RenameField("Sprite Renderer")]
    private SpriteRenderer spriteRenderer{
        get;
        set;
    }

    /**************************************************
        User Defined Functions
    **************************************************/

    /// <summary>
    /// ポップアップ
    /// </summary>
    override protected void Move(){
        base.Move();

        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, timer / lifeTime);
    }

}
