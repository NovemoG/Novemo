using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Core;
using Enums;
using UnityEngine;
using static Enums.ActionCode;

namespace Managers
{
	public static class InputManager
	{
		public static Dictionary<ActionCode, KeyCodes> KeyBinds { get; private set; } = new();

		#region DefaultKeyBinds

		private static readonly Dictionary<ActionCode, KeyCodes> DefaultKeyBinds = new()
		{
			{None, new KeyCodes(KeyCode.None, KeyCode.None)},
			{Escape, new KeyCodes(KeyCode.Escape, KeyCode.None)},
			{MoveUp, new KeyCodes(KeyCode.UpArrow, KeyCode.W)},
			{MoveDown, new KeyCodes(KeyCode.DownArrow, KeyCode.S)},
			{MoveLeft, new KeyCodes(KeyCode.LeftArrow, KeyCode.A)},
			{MoveRight, new KeyCodes(KeyCode.RightArrow, KeyCode.D)},
			{Attack, new KeyCodes(KeyCode.Mouse0, KeyCode.None)},
			{Block, new KeyCodes(KeyCode.Mouse1, KeyCode.None)},
			{Ability1, new KeyCodes(KeyCode.Q, KeyCode.None)},
			{Ability2, new KeyCodes(KeyCode.E, KeyCode.None)},
			{Ability3, new KeyCodes(KeyCode.Z, KeyCode.None)},
			{Ability4, new KeyCodes(KeyCode.C, KeyCode.None)},
			{Item1, new KeyCodes(KeyCode.Alpha0, KeyCode.None)},
			{Item2, new KeyCodes(KeyCode.Alpha1, KeyCode.None)},
			{Item3, new KeyCodes(KeyCode.Alpha2, KeyCode.None)},
			{Item4, new KeyCodes(KeyCode.Alpha3, KeyCode.None)},
			{Item5, new KeyCodes(KeyCode.Alpha4, KeyCode.None)},
			{Item6, new KeyCodes(KeyCode.Alpha5, KeyCode.None)},
			{Item7, new KeyCodes(KeyCode.Alpha6, KeyCode.None)},
			{Item8, new KeyCodes(KeyCode.Alpha7, KeyCode.None)},
			{Item9, new KeyCodes(KeyCode.Alpha8, KeyCode.None)},
			{Item10, new KeyCodes(KeyCode.Alpha9, KeyCode.None)},
			{Item11, new KeyCodes(KeyCode.Minus, KeyCode.None)},
			{Item12, new KeyCodes(KeyCode.Equals, KeyCode.None)},
			{Interact, new KeyCodes(KeyCode.F, KeyCode.None)},
			{Vault, new KeyCodes(KeyCode.V, KeyCode.None)},
			{Chest, new KeyCodes(KeyCode.B, KeyCode.None)},
			{Equipment, new KeyCodes(KeyCode.R, KeyCode.None)},
			{Crafting, new KeyCodes(KeyCode.G, KeyCode.None)},
			{Tooltip, new KeyCodes(KeyCode.T, KeyCode.None)},
		};

		#endregion

		#region AllowedKeys

		private static readonly ReadOnlyCollection<KeyCode> AllowedKeys = new(new List<KeyCode>{
			KeyCode.Q,
			KeyCode.W,
			KeyCode.E,
			KeyCode.R,
			KeyCode.T,
			KeyCode.Y,
			KeyCode.U,
			KeyCode.I,
			KeyCode.O,
			KeyCode.P,
			KeyCode.A,
			KeyCode.S,
			KeyCode.D,
			KeyCode.F,
			KeyCode.G,
			KeyCode.H,
			KeyCode.J,
			KeyCode.K,
			KeyCode.L,
			KeyCode.Z,
			KeyCode.X,
			KeyCode.C,
			KeyCode.V,
			KeyCode.B,
			KeyCode.N,
			KeyCode.M,
			KeyCode.Alpha0,
			KeyCode.Alpha1,
			KeyCode.Alpha2,
			KeyCode.Alpha3,
			KeyCode.Alpha4,
			KeyCode.Alpha5,
			KeyCode.Alpha6,
			KeyCode.Alpha7,
			KeyCode.Alpha8,
			KeyCode.Alpha9,
			KeyCode.BackQuote,
			KeyCode.Equals,
			KeyCode.Minus,
			KeyCode.UpArrow,
			KeyCode.LeftArrow,
			KeyCode.DownArrow,
			KeyCode.RightArrow,
			KeyCode.Tab,
			KeyCode.LeftShift,
			KeyCode.RightShift,
			KeyCode.LeftControl,
			KeyCode.RightControl,
			KeyCode.LeftAlt,
			KeyCode.RightAlt,
			KeyCode.CapsLock,
			KeyCode.LeftBracket,
			KeyCode.RightBracket,
			KeyCode.Semicolon,
			KeyCode.Quote,
			KeyCode.Comma,
			KeyCode.Period,
			KeyCode.F1,
			KeyCode.F2,
			KeyCode.F3,
			KeyCode.F4,
			KeyCode.F5,
			KeyCode.F6,
			KeyCode.F7,
			KeyCode.F8,
			KeyCode.F9,
			KeyCode.F10,
			KeyCode.F11,
			KeyCode.F12,
			KeyCode.Delete,
			KeyCode.Insert,
			KeyCode.Home,
			KeyCode.PageUp,
			KeyCode.PageDown,
			KeyCode.End,
			KeyCode.Keypad0,
			KeyCode.Keypad1,
			KeyCode.Keypad2,
			KeyCode.Keypad3,
			KeyCode.Keypad4,
			KeyCode.Keypad5,
			KeyCode.Keypad6,
			KeyCode.Keypad7,
			KeyCode.Keypad8,
			KeyCode.Keypad9,
			KeyCode.KeypadPlus,
			KeyCode.KeypadDivide,
			KeyCode.KeypadEnter,
			KeyCode.KeypadEquals,
			KeyCode.KeypadMinus,
			KeyCode.KeypadPeriod,
			KeyCode.Mouse0,
			KeyCode.Mouse1,
			KeyCode.Mouse2,
			KeyCode.Mouse3,
			KeyCode.Mouse4,
			KeyCode.Mouse5,
			KeyCode.Mouse6
		});

		#endregion

		public static void SetKeyBind(ActionCode actionCode, bool primary)
		{
			foreach (var keyCode in AllowedKeys.Where(Input.GetKeyDown))
			{
				if (keyCode == KeyCode.Escape)
					Unbind(actionCode, primary);
				else
					KeyBinds[actionCode] = primary
						? new KeyCodes(KeyBinds[actionCode].main, keyCode)
						: new KeyCodes(keyCode, KeyBinds[actionCode].primary);
			}
		}

		public static void Unbind(ActionCode actionCode, bool primary)
		{
			KeyBinds[actionCode] = primary 
				? new KeyCodes(KeyBinds[actionCode].main, KeyCode.None)
				: new KeyCodes(KeyCode.None, KeyBinds[actionCode].primary);
		}

		public static bool GetKeyDown(this ActionCode actionCode)
		{
			return Input.GetKeyDown(GetActionKey(actionCode).main) || 
			       Input.GetKeyDown(GetActionKey(actionCode).primary);
		}

		public static bool GetKeyUp(this ActionCode actionCode)
		{
			return Input.GetKeyUp(GetActionKey(actionCode).main) || 
			       Input.GetKeyUp(GetActionKey(actionCode).primary);
		}

		public static bool GetKey(this ActionCode actionCode)
		{
			return Input.GetKey(GetActionKey(actionCode).main) || 
			       Input.GetKey(GetActionKey(actionCode).primary);
		}

		public static int GetAxis(Axis axis)
		{
			if (axis == Axis.Horizontal)
			{
				return MoveRight.GetKey() ? 1 : MoveLeft.GetKey() ? -1 : 0;
			}

			return MoveUp.GetKey() ? 1 : MoveDown.GetKey() ? -1 : 0;
		}
		
		public static void LoadKeyBinds()
		{
			var binds = ConfigHandler.GetSection(Section.KeyBinds);
			
			if (binds == null)
			{
				LoadDefaultKeyBinds();
				return;
			}
			
			KeyBinds.Clear();
			
			KeyBinds.Add(None, new KeyCodes(KeyCode.None, KeyCode.None));
			KeyBinds.Add(Escape, new KeyCodes(KeyCode.Escape, KeyCode.None));

			var loadDefault = false;
			foreach (var bind in new Dictionary<string, string>(binds))
			{
				var actionCode = bind.Key.ToEnum(None);
				if (actionCode == None)
				{
					ConfigHandler.RemoveKey(Section.KeyBinds, bind.Key);
					loadDefault = true;
				}
				
				var bindValues = bind.Value.Split(',');
				
				var mainKey = bindValues[0].ToEnum(DefaultKeyBinds[actionCode].main);
				var primaryKey = bindValues[1].ToEnum(DefaultKeyBinds[actionCode].primary);
				
				ConfigHandler.PopulateConfig(Section.KeyBinds, actionCode.ToString(), $"{mainKey.ToString()}, {primaryKey.ToString()}");
				
				KeyBinds.Add(actionCode, new KeyCodes(mainKey, primaryKey));
			}
			if (loadDefault) LoadDefaultKeyBinds();
		}

		private static void SaveBinds()
		{
			foreach (var bind in KeyBinds.Skip(2))
			{
				ConfigHandler.PopulateConfig(Section.KeyBinds, bind.Key.ToString(), $"{bind.Value.main.ToString()}, {bind.Value.primary.ToString()}");
			}
			
			ConfigHandler.WriteConfigFile();
		}
		
		private static void LoadDefaultKeyBinds()
		{
			KeyBinds = DefaultKeyBinds;
			SaveBinds();
		}
		
		private static KeyCodes GetActionKey(ActionCode actionCode)
		{
			return KeyBinds[actionCode];
		}
	}
	
	[Serializable]
	public struct KeyCodes
	{
		public KeyCode main;
		public KeyCode primary;

		public KeyCodes(KeyCode main, KeyCode primary)
		{
			this.main = main;
			this.primary = primary;
		}
	}

	public enum Axis
	{
		Horizontal,
		Vertical
	}
}