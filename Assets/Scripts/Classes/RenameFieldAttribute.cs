//================================================================================
//
//  RenameFieldAttribute
//
//  Unityインスペクター上でフィールドの名前を変更する為の属性
//
//================================================================================

using UnityEditor;
using UnityEngine;

class RenameFieldAttribute : PropertyAttribute{

    /// <summary>
    /// フィールド名
    /// </summary>
    public string fieldName{
        get;
        private set;
    }

    /// <summary>
    /// フィールドアトリビュートの名前変更
    /// </summary>
    /// <param name="name">変更後の名前</param>
    public RenameFieldAttribute(string name){
        fieldName = name;
    }

    /// <summary>
    /// エディターGUI上での処理
    /// </summary>
    #if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(RenameFieldAttribute))]
        class FieldNameDrawer : PropertyDrawer{
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
                string[] path = property.propertyPath.Split('.');
                bool isArray = 1 < path.Length && path[1] == "Array";

                if(!isArray && attribute is RenameFieldAttribute fieldName){
                    label.text = fieldName.fieldName;
                }

                EditorGUI.PropertyField(position, property, label, true);
            }
        }
    #endif

}
