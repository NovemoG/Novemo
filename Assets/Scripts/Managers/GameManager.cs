using UnityEngine;

namespace Managers
{
	[RequireComponent(typeof(InventoryManager))]
	[RequireComponent(typeof(EquipmentManager))]
	[RequireComponent(typeof(PlayerManager))]
	[RequireComponent(typeof(UIManager))]
	[RequireComponent(typeof(AudioManager))]
	public class GameManager : MonoBehaviour
	{
		#region Singleton

		public static GameManager Instance { get; private set; }
		public InventoryManager InventoryManager { get; private set; }
		public EquipmentManager EquipmentManager { get; private set; }
		public PlayerManager PlayerManager { get; private set; }
		public UIManager UIManager { get; private set; }
		public AudioManager AudioManager { get; private set; }

		public Camera mainCamera;

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Destroy(this);
				return;
			}
			Instance = this;
            
			InventoryManager = GetComponent<InventoryManager>();
			EquipmentManager = GetComponent<EquipmentManager>();
			PlayerManager = GetComponent<PlayerManager>();
			UIManager = GetComponent<UIManager>();
			AudioManager = GetComponent<AudioManager>();
		}

		#endregion
	}
}
