using Core;
using UnityEditor;
using UnityEngine;

namespace Editor
{
	[CustomPropertyDrawer(typeof(ListElementTitleAttribute))]
	public class ListElementTitleDrawer : PropertyDrawer
	{
		protected virtual ListElementTitleAttribute Attribute => (ListElementTitleAttribute)attribute;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(property, label, true);
		}
		
		private SerializedProperty _titleNameProp;
		
	    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	    {
	        var fullPropertyPath = property.propertyPath + "." + Attribute.ElementName;
	        _titleNameProp = property.serializedObject.FindProperty(fullPropertyPath);
	        
	        var newLabel = GetTitle();
	        if (string.IsNullOrEmpty(newLabel))
	        {
		        newLabel = label.text;
	        }
	        
	        EditorGUI.PropertyField(position, property, new GUIContent(newLabel, label.tooltip), true);
	    }
	    
	    private string GetTitle()
	    {
		    if (_titleNameProp.propertyType != SerializedPropertyType.Enum) return "";
		    
		    var enumValueName = _titleNameProp.enumNames[_titleNameProp.enumValueIndex];
		    var replaced = enumValueName.Replace("_", " ");
			    
		    return replaced;
	    }
	}
}