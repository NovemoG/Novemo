using System.IO;
using System.Linq;
using Core;
using Saves;
using UnityEngine;
using UnityEngine.Tilemaps;

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

		public ItemDatabase itemDatabase;
		public Camera mainCamera;
		public Tilemap[] tilemaps;

		public bool saveGame;

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Destroy(this);
				return;
			}
			Instance = this;

			if (!Directory.Exists(Metrics.GameDataPath))
			{
				Directory.CreateDirectory(Metrics.GameDataPath);
			}
			
			ConfigHandler.ReadConfigFile();
			InputManager.LoadKeyBinds();
            
			InventoryManager = GetComponent<InventoryManager>();
			EquipmentManager = GetComponent<EquipmentManager>();
			PlayerManager = GetComponent<PlayerManager>();
			UIManager = GetComponent<UIManager>();
			AudioManager = GetComponent<AudioManager>();
			
			LoadSaveData();
		}

		#endregion

		private void OnApplicationQuit()
		{
			if (!Directory.Exists(Metrics.GameDataPath))
			{
				Directory.CreateDirectory(Metrics.GameDataPath);
			}
			
			ConfigHandler.WriteConfigFile();
			
			if (saveGame) SaveData();
		}

		private void LoadSaveData()
		{
			var directory = new DirectoryInfo(Metrics.SavesPath);

			var files = directory.GetFiles();
			if (files.Length == 0) return;
			
			var newestSave = files.Last();
			using var sr = new StreamReader(newestSave.OpenRead());

			var content = sr.ReadToEnd();
			var saveData = (GameSaveData)Serializer.Deserialize(typeof(GameSaveData), content);
			
			PlayerManager.playerClass.LoadSaveData(saveData.playerSaveData);
			InventoryManager.playerInventory.LoadSaveData(saveData.inventorySaveData);
			InventoryManager.vaultInventory.LoadSaveData(saveData.vaultSaveData);
			InventoryManager.equipmentInventory.LoadSaveData(saveData.equipmentSaveData);
		}

		private void SaveData()
		{
			var saveData = new GameSaveData
			{
				playerSaveData = new PlayerSaveData(PlayerManager.playerClass),
				inventorySaveData = new InventorySaveData(InventoryManager.playerInventory),
				vaultSaveData = new InventorySaveData(InventoryManager.vaultInventory),
				equipmentSaveData = new EquipmentSaveData(InventoryManager.equipmentInventory),
			};
			
			var text = Serializer.Serialize(typeof(GameSaveData), saveData);
			
			var savesPath = Metrics.SavesPath;
			if (!Directory.Exists(savesPath))
			{
				Directory.CreateDirectory(savesPath);
			}

			var savesCount = Directory.EnumerateFiles(savesPath, "*.data", SearchOption.TopDirectoryOnly).Count();
			var filePath = Path.Combine(savesPath, $"save{savesCount}.data");
			
			using var sw = File.AppendText(filePath);
			sw.Write(text);
		}
	}
}
