using UnityEngine;

namespace Items
{
	public class ItemDrop : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer spriteRenderer;
		[SerializeField] private SpriteRenderer shadowRenderer;
		
		[SerializeField] private Item item;
		public Item Item
		{
			get => item;
			set
			{
				if (value == null) return;
				
				spriteRenderer.sprite = value.Icon;
				shadowRenderer.sprite = value.Icon;
				item = value;
			}
		}
		
		[SerializeField] private int count;
		public int Count
		{
			get => count;
			set
			{
				if (value == 0) Destroy(gameObject);
				
				count = value;
			}
		}

		private void OnEnable()
		{
			Destroy(gameObject, 300);
		}
	}
}