//================================================================================
//
//  MessageWindowBehaviour
//
//  メッセージウィンドウの挙動
//
//================================================================================

using System;
using UnityEngine;
using TMPro;

public class MessageWindowBehaviour : MonoBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    //  References
    //==============================

    [field: SerializeField, RenameField("Title Text")]
    public TextMeshProUGUI titleText{
        get;
        set;
    }

    [field: SerializeField, RenameField("Message Text")]
    public TextMeshProUGUI messageText{
        get;
        set;
    }

    /**************************************************
        Unity Event Functions
    **************************************************/

    private void Awake(){
        Initialize();
    }

    /**************************************************
        User Defined Functions
    **************************************************/

    private void Initialize(){

    }

}
