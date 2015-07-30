using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DisappearAttachedFieldAttribute : PropertyAttribute{}

#if UNITY_EDITOR
[CustomPropertyDrawer( typeof ( DisappearAttachedFieldAttribute ) )]
public class DisappearAttachedField : PropertyDrawer
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		if (property.propertyType == SerializedPropertyType.ObjectReference) {
			if( property.objectReferenceValue == null ){
				EditorGUI.PropertyField(position, property, label, true);
			}
		}
	}
	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		if (property.objectReferenceValue != null) {
			return 0;
		}
		
		return base.GetPropertyHeight (property, label);
	}
}
#endif