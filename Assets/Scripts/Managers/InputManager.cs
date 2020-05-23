//================================================================================
//
//  InputManager
//
//  入力情報の管理を行う
//
//================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour{

    /**************************************************
        Enumerations
    **************************************************/

    /// <summary>
    /// ボタンの種類
    /// </summary>
    public enum ButtonName{
        Jump,
        Fire
    }

    /// <summary>
    /// ボタンの状態
    /// </summary>
    public enum ButtonState{
        None,
        Press,
        Down,
        Up
    }

    /**************************************************
        Fields / Properties
    **************************************************/
    
    /// <summary>
    /// クラスのインスタンス
    /// </summary>
    public static InputManager instance{
        get;
        private set;
    }

    /// <summary>
    /// 各ボタンの連想配列
    /// </summary>
    public Dictionary<ButtonName, ButtonState> buttons{
        get;
        private set;
    }

    /**************************************************
        Unity Event Functions
    **************************************************/

    private void Start(){
        Initialize();
    }
    
    private void Update(){
        GetKeyInputs();
    }

    private void LateUpdate(){
        ResetInputs();
    }

    /**************************************************
        User Defined Functions
    **************************************************/

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Initialize(){
        if(instance == null){
            instance = this;
        }
        else if(instance != this){
            Destroy(gameObject);
        }

        buttons = new Dictionary<ButtonName, ButtonState>();
        buttons.Add(ButtonName.Jump, ButtonState.None);
        buttons.Add(ButtonName.Fire, ButtonState.None);

        SetButtonEventTriggers(ButtonName.Jump, UIManager.instance.leftActionButton.GetComponent<EventTrigger>());
        SetButtonEventTriggers(ButtonName.Fire, UIManager.instance.rightActionButton.GetComponent<EventTrigger>());
    }

    /// <summary>
    /// ボタンのイベントトリガー設定
    /// </summary>
    /// <param name="button">設定対象のボタン</param>
    /// <param name="eventTrigger">設定するイベントトリガー</param>
    private void SetButtonEventTriggers(ButtonName button, EventTrigger eventTrigger){
        EventTrigger.Entry buttonDownEntry = new EventTrigger.Entry();
        buttonDownEntry.eventID = EventTriggerType.PointerDown;
        buttonDownEntry.callback.AddListener((eventData) => {
            GetButtonInput(button, EventTriggerType.PointerDown);
        });

        EventTrigger.Entry buttonUpEntry = new EventTrigger.Entry();
        buttonUpEntry.eventID = EventTriggerType.PointerUp;
        buttonUpEntry.callback.AddListener((eventData) => {
            GetButtonInput(button, EventTriggerType.PointerUp);
        });
        
        eventTrigger.triggers.Add(buttonDownEntry);
        eventTrigger.triggers.Add(buttonUpEntry);
    }

    /// <summary>
    /// 毎フレームでの入力状態リセット
    /// </summary>
    private void ResetInputs(){
        if(GameManager.instance.targetPlatform != GameManager.TargetPlatform.Mobile){
            return;
        }

        foreach(ButtonName button in Enum.GetValues(typeof(ButtonName))){
            if(buttons[button] == ButtonState.Press || buttons[button] == ButtonState.Down){
                buttons[button] = ButtonState.Down;
            }
            else if(buttons[button] == ButtonState.Up || buttons[button] == ButtonState.None){
                buttons[button] = ButtonState.None;
            }
        }
    }

    /// <summary>
    /// キー入力の取得
    /// </summary>
    private void GetKeyInputs(){
        if(GameManager.instance.targetPlatform != GameManager.TargetPlatform.PC){
            return;
        }

        GetButtonInput(ButtonName.Jump, KeyCode.Space);
        GetButtonInput(ButtonName.Fire, KeyCode.F);
    }

    /// <summary>
    /// ボタン入力の取得(PC環境)
    /// </summary>
    /// <param name="button">対象のボタン</param>
    /// <param name="keyCode">対象キーのキーコード</param>
    private void GetButtonInput(ButtonName button, KeyCode keyCode){
        if(Input.GetKeyDown(keyCode)){
            buttons[button] = ButtonState.Press;
        }
        else if(Input.GetKey(keyCode)){
            buttons[button] = ButtonState.Down;
        }
        else if(Input.GetKeyUp(keyCode)){
            buttons[button] = ButtonState.Up;
        }
        else{
            buttons[button] = ButtonState.None;
        }
    }

    /// <summary>
    /// ボタン入力の取得(モバイル環境)
    /// </summary>
    /// <param name="button">対象のボタン</param>
    /// <param name="eventType">対象のイベントタイプ</param>
    private void GetButtonInput(ButtonName button, EventTriggerType eventType){
        switch(eventType){
            case EventTriggerType.PointerDown:
                buttons[button] = ButtonState.Press;

                break;
            case EventTriggerType.PointerUp:
                buttons[button] = ButtonState.Up;

                break;
        }
    }

}
