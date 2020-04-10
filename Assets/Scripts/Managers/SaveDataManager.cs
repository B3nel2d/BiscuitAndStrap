//================================================================================
//
//  SaveDataManager
//
//  セーブデータの管理を行う
//
//================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

public class SaveDataManager : MonoBehaviour{

    /**************************************************
        Fields / Properties
    **************************************************/

    /// <summary>
    /// クラスのインスタンス
    /// </summary>
    public static SaveDataManager instance{
        get;
        private set;
    }

    /// <summary>
    /// セーブデータのバッキングフィールド
    /// </summary>
	private static SaveData saveData_value = null;
    /// <summary>
    /// セーブデータ
    /// </summary>
	private static SaveData saveData{
		get{
			if(saveData_value == null){
				string path = Application.persistentDataPath + "/";
				string fileName = "savedata.txt";

				saveData_value = new SaveData(path, fileName);
			}

			return saveData_value;
		}
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

    //  各データ型の保存・読込
    //==============================

    public static void SetInt(string key, int value){
		saveData.SetInt(key, value);
	}

	public static int GetInt(string key, int defaultValue = 0){
		return saveData.GetInt(key, defaultValue);
	}

	public static void SetFloat(string key, float value){
		saveData.SetFloat(key, value);
	}

	public static float GetFloat(string key, float defaultValue = 0.0f){
		return saveData.GetFloat(key, defaultValue);
	}

	public static void SetString(string key, string value){
		saveData.SetString(key, value);
	}

	public static string GetString(string key, string defaultValue = ""){
		return saveData.GetString(key, defaultValue);
	}

	public static void SetList<Type>(string key, List<Type> list){
		saveData.SetList<Type>(key, list);
	}

	public static List<T> GetList<T>(string key, List<T> defaultValue){
		return saveData.GetList<T>(key, defaultValue);
	}

	public static Type GetClass<Type>(string key, Type defaultValue) where Type : class, new(){
		return saveData.GetClass(key, defaultValue);
	}

	public static void SetClass<Type>(string key, Type value) where Type : class, new(){
		saveData.SetClass<Type>(key, value);
	}

    /// <summary>
    /// セーブデータのクリア
    /// </summary>
	public static void Clear(){
		saveData.Clear();
	}

    /// <summary>
    /// 指定データの削除
    /// </summary>
	public static void Remove(string key){
		saveData.Remove(key);
	}

    /// <summary>
    /// 指定したキーの存在判定
    /// </summary>
	public static bool ContainsKey(string key){
		return saveData.ContainsKey(key);
	}
    
    /// <summary>
    /// キー一覧の取得
    /// </summary>
	public static List<string> GetKeys(){
		return saveData.GetKeys();
	}

    /// <summary>
    /// セーブデータの保存
    /// </summary>
	public static void Save(){
		saveData.Save();
	}

    /// <summary>
    /// データ保存場所の取得
    /// </summary>
    private static string GetDataPath(){
        #if !UNITY_EDITOR && UNITY_ANDROID
            return Application.dataPath;
        #else
            return Application.persistentDataPath;
        #endif
    }

    /**************************************************
        Classes
    **************************************************/

    /// <summary>
    /// セーブデータのクラス
    /// </summary>
	[Serializable]
	private class SaveData{

        /**************************************************
            Fields / Properties
        **************************************************/

		private string path{
            get;
            set;
		}

		private string fileName{
            get;
            set;
		}

        private Dictionary<string, string> saveDataDictionary;

        /**************************************************
            Functions
        **************************************************/

		public SaveData(string path, string fileName){
			this.path = path;
			this.fileName = fileName;
			saveDataDictionary = new Dictionary<string, string>();

			Load();
		}

		~SaveData(){
			//Save();
		}

		public void SetInt(string key, int value){
			CheckKey(key);
			saveDataDictionary[key] = value.ToString();
		}

		public int GetInt(string key, int defaultValue = 0){
			CheckKey(key);

            if(!saveDataDictionary.ContainsKey(key)){
                return defaultValue;
            }

			int value;
			if(!int.TryParse(saveDataDictionary[key], out value)) {
				value = defaultValue;
			}

			return value;
		}

		public void SetFloat(string key, float value){
			CheckKey(key);
			saveDataDictionary[key] = value.ToString();
		}

		public float GetFloat(string key, float defaultValue = 0.0f){
			CheckKey(key);

            if(!saveDataDictionary.ContainsKey(key)){
                return defaultValue;
            }

            float value;
			if(!float.TryParse(saveDataDictionary[key], out value)){
				value = defaultValue;
			}

			return value;
		}

		public void SetString(string key, string value){
			CheckKey(key);
			saveDataDictionary[key] = value;
		}

		public string GetString(string key, string defaultValue = ""){
			CheckKey(key);

            if(!saveDataDictionary.ContainsKey(key)){
                return defaultValue;
            }
				
			return saveDataDictionary[key];
		}

		public void SetList<Type>(string key, List<Type> list){
			CheckKey(key);

			var serializableList = new Serialization<Type>(list);
			string json = JsonUtility.ToJson(serializableList);
			saveDataDictionary[key] = json;
		}

		public List<Type> GetList<Type>(string key, List<Type> defaultValue){
			CheckKey(key);

			if(!saveDataDictionary.ContainsKey(key)){
				return defaultValue;
			}

			string json = saveDataDictionary[key];
			Serialization<Type> deserializeList = JsonUtility.FromJson<Serialization<Type>>(json);

			return deserializeList.ToList();
		}

		public Type GetClass<Type>(string key, Type defaultValue) where Type : class, new(){
			CheckKey(key);

            if(!saveDataDictionary.ContainsKey(key)){
                return defaultValue;
            }

			string json = saveDataDictionary[key];
			Type value = JsonUtility.FromJson<Type>(json);
			return value;
		}

		public void SetClass<Type>(string key, Type value) where Type : class, new(){
			CheckKey(key);

			string json = JsonUtility.ToJson(value);
			saveDataDictionary[key] = json;
		}

		public void Clear(){
			saveDataDictionary.Clear();
		}

		public void Remove(string key){
			CheckKey(key);

			if(saveDataDictionary.ContainsKey(key)){
				saveDataDictionary.Remove(key);
			}
		}

		public bool ContainsKey(string key){
			return saveDataDictionary.ContainsKey(key);
		}

		public List<string> GetKeys(){
			return saveDataDictionary.Keys.ToList<string>();
		}

		public void Save(){
			using(StreamWriter streamWriter = new StreamWriter(Path.Combine(path, fileName), false, Encoding.GetEncoding("utf-8"))){	
				Serialization<string, string> serializedDictionary = new Serialization<string, string>(saveDataDictionary);
				serializedDictionary.OnBeforeSerialize();
                string encryptedData = Encryption.Encrypt(JsonUtility.ToJson(serializedDictionary));

				streamWriter.WriteLine(encryptedData);
			}
		}

		public void Load(){
			if(File.Exists(Path.Combine(path, fileName))){
				using(StreamReader streamReader = new StreamReader(Path.Combine(path, fileName), Encoding.GetEncoding("utf-8"))){
					if(saveDataDictionary != null){
                        string decryptedData = Encryption.Decrypt(streamReader.ReadToEnd());
						Serialization<string, string> serealizedDictionary = JsonUtility.FromJson<Serialization<string, string>>(decryptedData);
						serealizedDictionary.OnAfterDeserialize();

						saveDataDictionary = serealizedDictionary.ToDictionary();
					}
				}
			}
			else{
                saveDataDictionary = new Dictionary<string, string>();
            }
		}

		public string GetJsonString(string key){
			CheckKey(key);

			if(saveDataDictionary.ContainsKey(key)){
				return saveDataDictionary[key];
			}
            else{
				return null;
			}
		}

		private void CheckKey(string key){
			if(string.IsNullOrEmpty(key)){
				throw new ArgumentException("Invalid key.");
			}
		}
	}

    /// <summary>
    /// リストのシリアライズ化データのクラス
    /// </summary>
	[Serializable]
	private class Serialization<Type>{

        /**************************************************
            Fields / Properties
        **************************************************/

        public List<Type> target;

        /**************************************************
            Functions
        **************************************************/

		public List<Type> ToList(){
			return target;
		}

		public Serialization(){

		}

		public Serialization(List<Type> target){
			this.target = target;
		}

	}

    /// <summary>
    /// ディクショナリーのシリアライズ化データのクラス
    /// </summary>
	[Serializable]
	private class Serialization<KeyType, ValueType>{

        /**************************************************
            Fields / Properties
        **************************************************/

        public List<KeyType> keys;
        public List<ValueType> values;
        private Dictionary<KeyType, ValueType> target;

        /**************************************************
            Functions
        **************************************************/

		public Dictionary<KeyType, ValueType> ToDictionary(){
			return target;
		}

		public Serialization(){

		}

		public Serialization(Dictionary<KeyType, ValueType> target){
			this.target = target;
		}

		public void OnBeforeSerialize(){
			keys = new List<KeyType>(target.Keys);
			values = new List<ValueType>(target.Values);
		}

		public void OnAfterDeserialize(){
            int count = Math.Min(keys.Count, values.Count);
			target = new Dictionary<KeyType, ValueType>(count);
			Enumerable.Range(0, count).ToList().ForEach(i => target.Add(keys[i], values[i]));
		}
	}

}
