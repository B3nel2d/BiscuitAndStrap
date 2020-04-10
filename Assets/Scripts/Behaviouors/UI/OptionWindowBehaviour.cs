//================================================================================
//
//  OptionWindowBehaviour
//
//  オプションウィンドウの挙動
//
//================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionWindowBehaviour : MonoBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    //  Settings
    //==============================

    public float musicVolume{
        get;
        set;
    }

    public float soundEffectVolume{
        get;
        set;
    }

    //  References
    //==============================

    [field: SerializeField, RenameField("BGM Volume Slider")]
    private Slider musicVolumeSlider{
        get;
        set;
    }

    [field: SerializeField, RenameField("SE Volume Slider")]
    private Slider soundEffectVolumeSlider{
        get;
        set;
    }

    /**************************************************
        Unity Event Functions
    **************************************************/

    private void Awake(){
        
    }

    /**************************************************
        User Defined Functions
    **************************************************/

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
        });
    }

}
