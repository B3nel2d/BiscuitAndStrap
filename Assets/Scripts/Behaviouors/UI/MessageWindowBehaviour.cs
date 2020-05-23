//================================================================================
//
//  MessageWindowBehaviour
//
//  メッセージウィンドウの挙動
//
//================================================================================

using UnityEngine;
using TMPro;

public class MessageWindowBehaviour : MonoBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    //  References
    //==============================

    /// <summary>
    /// タイトルのテキスト
    /// </summary>
    [field: SerializeField, RenameField("Title Text")]
    public TextMeshProUGUI titleText{
        get;
        set;
    }

    /// <summary>
    /// メッセージのテキスト
    /// </summary>
    [field: SerializeField, RenameField("Message Text")]
    public TextMeshProUGUI messageText{
        get;
        set;
    }

}
