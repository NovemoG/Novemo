using UnityEngine;

namespace Core
{
	public class ListElementTitleAttribute : PropertyAttribute
	{
		public readonly string ElementName;

		public ListElementTitleAttribute(string elementName)
		{
			ElementName = elementName;
		}
	}
}