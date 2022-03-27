using UnityEditor;
using UnityEngine;

namespace SafeEnum.Editor
{
	[CustomPropertyDrawer(typeof(SafeEnum<>), true)]
	public class SafeEnumDrawer : PropertyDrawer
	{
		const string ERROR_WARNING_TEXT = "_MISSING_";
		const int ERROR_INDEX = -1;
		static readonly Color ERROR_COLOR = Color.red;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty enumProperty = property.FindPropertyRelative("_enumValue");
			bool isErrorDetected = enumProperty.enumValueIndex <= ERROR_INDEX;
			Color defaultGUIColor = GUI.color;

			label.text = !isErrorDetected ? FirstCharToUpper(label.text) : ERROR_WARNING_TEXT;

			if (isErrorDetected)
			{
				GUI.color = ERROR_COLOR;
			}

			EditorGUI.BeginChangeCheck();
			EditorGUI.PropertyField(position, enumProperty, label);

			if (EditorGUI.EndChangeCheck())
			{
				SerializedProperty stringValueProperty = property.FindPropertyRelative("_stringValue");
				stringValueProperty.stringValue = enumProperty.enumNames[enumProperty.enumValueIndex];
			}

			GUI.color = defaultGUIColor;
		}

		static string FirstCharToUpper(string input)
		{
			return string.IsNullOrWhiteSpace(input) ? ERROR_WARNING_TEXT : input[0].ToString().ToUpper() + input.Substring(1);
		}
	}
}