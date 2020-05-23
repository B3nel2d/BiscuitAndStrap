//================================================================================
//
//  OptionWindowBehaviour
//
//  オプションウィンドウの挙動
//
//================================================================================

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class OptionWindowBehaviour : MonoBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    //  Settings
    //==============================

    /// <summary>
    /// BGMの音量
    /// </summary>
    public float musicVolume{
        get;
        set;
    }

    /// <summary>
    /// 効果音の音量
    /// </summary>
    public float soundEffectVolume{
        get;
        set;
    }

    //  References
    //==============================

    /// <summary>
    /// タイトルのテキスト
    /// </summary>
    [field: SerializeField, RenameField("Title Text")]
    private TextMeshProUGUI titleText{
        get;
        set;
    }

    /// <summary>
    /// メニュー
    /// </summary>
    [field: SerializeField, RenameField("Menu")]
    private GameObject menu{
        get;
        set;
    }

    /// <summary>
    /// オーディオ設定
    /// </summary>
    [field: SerializeField, RenameField("Audio Settings")]
    private GameObject audioSettings{
        get;
        set;
    }

    /// <summary>
    /// BGM音量のスライダー
    /// </summary>
    [field: SerializeField, RenameField("BGM Volume Slider")]
    private Slider musicVolumeSlider{
        get;
        set;
    }

    /// <summary>
    /// 効果音音量のスライダー
    /// </summary>
    [field: SerializeField, RenameField("SE Volume Slider")]
    private Slider soundEffectVolumeSlider{
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
        musicVolume = SaveDataManager.GetFloat("BGM Volume", 1.0f);
        musicVolumeSlider.value = musicVolume;

        switch(SceneManager.GetActiveScene().name){
            case "GameScene":
                GameManager.instance.audioSources[0].volume = musicVolume;

                break;
            case "MenuScene":
                MenuManager.instance.audioSources[0].volume = musicVolume;

                break;
        }

        musicVolumeSlider.onValueChanged.AddListener((value) => {
            musicVolume = value;

            switch(SceneManager.GetActiveScene().name){
                case "GameScene":
                    GameManager.instance.audioSources[0].volume = musicVolume;

                    break;
                case "MenuScene":
                    MenuManager.instance.audioSources[0].volume = musicVolume;

                    break;
            }

            SaveDataManager.SetFloat("BGM Volume", musicVolume);
            SaveDataManager.Save();
        });

        soundEffectVolume = SaveDataManager.GetFloat("SE Volume", 1.0f);
        soundEffectVolumeSlider.value = soundEffectVolume;

        switch(SceneManager.GetActiveScene().name){
            case "GameScene":
                GameManager.instance.audioSources[1].volume = soundEffectVolume;

                break;
            case "MenuScene":
                MenuManager.instance.audioSources[1].volume = soundEffectVolume;

                break;
        }

        soundEffectVolumeSlider.onValueChanged.AddListener((value) => {
            soundEffectVolume = value;

            switch(SceneManager.GetActiveScene().name){
                case "GameScene":
                    GameManager.instance.audioSources[1].volume = soundEffectVolume;

                    break;
                case "MenuScene":
                    MenuManager.instance.audioSources[1].volume = soundEffectVolume;

                    break;
            }

            SaveDataManager.SetFloat("SE Volume", soundEffectVolume);
            SaveDataManager.Save();
        });
    }

    /// <summary>
    /// メニューの表示切替
    /// </summary>
    /// <param name="active">表示するか</param>
    public void ToggleMenu(bool active){
        menu.SetActive(active);

        if(active){
            titleText.text = "OPTIONS";
        }
    }

    /// <summary>
    /// オーディオ設定の表示切替
    /// </summary>
    /// <param name="active">表示するか</param>
    public void ToggleAudioSettings(bool active){
        audioSettings.SetActive(active);

        if(active){
            titleText.text = "AUDIO";
        }
    }

}
