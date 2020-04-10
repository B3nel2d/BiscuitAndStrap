//================================================================================
//
//  TitleManager
//
//  タイトル画面の管理を行う
//
//================================================================================

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    //  Singleton Instance
    //==============================
    
    /// <summary>
    /// クラスのインスタンス
    /// </summary>
    public static TitleManager instance{
        get;
        private set;
    }

    //  Animation
    //==============================
    
    /// <summary>
    /// オーバーレイのアニメーター
    /// </summary>
    [field: SerializeField, RenameField("Overlay Animator")]
    private Animator overlayAnimator{
        get;
        set;
    }

    //  Audio
    //==============================
    
    /// <summary>
    /// オーディオソース
    /// </summary>
    [field: SerializeField, RenameField("Audio Sources")]
    public List<AudioSource> audioSources{
        get;
        set;
    }

    //  References
    //==============================

    /// <summary>
    /// オーバーレイ
    /// </summary>
    [field: SerializeField, RenameField("Overlay")]
    private GameObject overlay{
        get;
        set;
    }

    /**************************************************
        Unity Event Functions
    **************************************************/

    private void Start(){
        Initialize();
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
    }

    /// <summary>
    /// メニュー画面への遷移
    /// </summary>
    public async void Enter(){
        overlay.SetActive(true);
        overlayAnimator.SetTrigger("FadeIn");

        await Task.Delay(3000);
        
        SceneManager.LoadScene("MenuScene");
    }

    /// <summary>
    /// 音声の再生
    /// </summary>
    public void PlayAudio(AudioClip audioClip){
        audioSources[1].PlayOneShot(audioClip);
    }

}
