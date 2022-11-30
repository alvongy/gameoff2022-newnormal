using UnityEditor;
using UnityEngine;
using StateMachine;

[CustomEditor(typeof(AnimatorParameterActionSO)), CanEditMultipleObjects]
public class AnimatorParameterActionSOEditor : CustomBaseEditor
{
	public override void OnInspectorGUI()
	{
		base.DrawNonEditableScriptReference<AnimatorParameterActionSO>();

		serializedObject.Update();

		EditorGUILayout.PropertyField(serializedObject.FindProperty("description"));
		EditorGUILayout.Space();

		EditorGUILayout.PropertyField(serializedObject.FindProperty("timeToRun"));
		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Animator Parameter", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("parameterName"), new GUIContent("Name"));

		// Draws the appropriate value depending on the type of parameter this SO is going to change on the Animator
		SerializedProperty animParamValue = serializedObject.FindProperty("parameterType");

		EditorGUILayout.PropertyField(animParamValue, new GUIContent("Type"));

		switch (animParamValue.intValue)
		{
			case (int)AnimatorParameterActionSO.ParameterType.BOOL:
				EditorGUILayout.PropertyField(serializedObject.FindProperty("boolValue"), new GUIContent("Desired value"));
				break;
			case (int)AnimatorParameterActionSO.ParameterType.INT:
				EditorGUILayout.PropertyField(serializedObject.FindProperty("intValue"), new GUIContent("Desired value"));
				break;
			case (int)AnimatorParameterActionSO.ParameterType.FLOAT:
				EditorGUILayout.PropertyField(serializedObject.FindProperty("floatValue"), new GUIContent("Desired value"));
				break;

		}

		serializedObject.ApplyModifiedProperties();
	}
}
