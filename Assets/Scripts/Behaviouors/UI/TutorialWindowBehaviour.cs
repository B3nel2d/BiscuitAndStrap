//================================================================================
//
//  TutorialWindowBehaviour
//
//  チュートリアルウィンドウの挙動
//
//================================================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialWindowBehaviour : MonoBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    /// <summary>
    /// チュートリアル画像のスプライト
    /// </summary>
    [field: SerializeField, RenameField("Sprites")]
    private List<Sprite> sprites{
        get;
        set;
    }

    /// <summary>
    /// 画像表示用のイメージ
    /// </summary>
    [field:SerializeField,RenameField("Image")]
    private Image image{
        get;
        set;
    }

    /// <summary>
    /// バックボタン
    /// </summary>
    [field:SerializeField,RenameField("Back Button")]
    private Button backButton{
        get;
        set;
    }

    /// <summary>
    /// ネクストボタンのテキスト
    /// </summary>
    [field:SerializeField,RenameField("Next Button Text")]
    private TextMeshProUGUI nextButtonText{
        get;
        set;
    }

    /// <summary>
    /// 現在のページ番号
    /// </summary>
    private int currentPage{
        get;
        set;
    }

    /// <summary>
    /// チュートリアル後にゲームスタートするかどうか
    /// </summary>
    public bool playAfterTutorial{
        get;
        set;
    }

    /**************************************************
        User Defined Functions
    **************************************************/

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize(){
        currentPage = 0;
        UpdateImage();
    }

    /// <summary>
    /// 画像の更新
    /// </summary>
    public void UpdateImage(){
        image.sprite = sprites[currentPage];

        if(currentPage <= 0){
            backButton.gameObject.SetActive(false);
        }
        else{
            backButton.gameObject.SetActive(true);
        }

        if(sprites.Count - 1 <= currentPage){
            if(playAfterTutorial){
                nextButtonText.text = "START";
            }
            else{
                nextButtonText.text = "OK";
            }
        }
        else{
            nextButtonText.text = "NEXT";
        }
    }

    /// <summary>
    /// 前のページの表示
    /// </summary>
    public void ShowPreviousPage(){
        if(currentPage <= 0){
            return;
        }

        --currentPage;
        UpdateImage();
    }

    /// <summary>
    /// 次のページの表示
    /// </summary>
    public void ShowNextPage(){
        if(sprites.Count - 1 <= currentPage){
            MenuManager.instance.ToggleTutorialWindow(false);

            if(playAfterTutorial){
                playAfterTutorial = false;
                MenuManager.instance.Play();
            }

            return;
        }

        ++currentPage;
        UpdateImage();
    }

}
