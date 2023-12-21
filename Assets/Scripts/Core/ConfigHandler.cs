using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Core
{
	public static class ConfigHandler
	{
		private static readonly string PathToCfg = Path.Combine(Metrics.GameDataPath, "settings.cfg");
		private static Dictionary<Section, Dictionary<string, string>> _cfgDictionary = new();
		private static bool _initialized;
		
		public static void ReadConfigFile()
		{
			if (!File.Exists(PathToCfg))
			{
				_initialized = false;

				if (_cfgDictionary.Count > 0)
				{
					WriteConfigFile();
				}
				
				_cfgDictionary.Clear();
				return;
			}

			if (_initialized)
			{
				Debug.Log("Config file already initialized");
				return;
			}

			Debug.Log("Reading config file");
			
			using var sr = new StreamReader(PathToCfg);

			var section = "";
			var key = "";
			var value = "";

			while (!sr.EndOfStream)
			{
				string line;
				if (string.IsNullOrEmpty(line = sr.ReadLine())) continue;
				
				line = line.Trim();
				if (line.StartsWith(';')) continue; //it is a comment
				
				if (line.StartsWith('[') && line.EndsWith(']'))
				{
					section = line.Substring(1, line.Length - 2);
				}
				else if (line.Contains('='))
				{
					var split = line.Split('=');
					key = split[0].Trim();
					value = split[1].Trim();
				}
				
				if (string.IsNullOrEmpty(section) || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value)) continue;
				
				PopulateConfig(section.ToEnum<Section>(), key, value);
			}

			_initialized = true;
		}

		public static void PopulateConfig(Section section, string key, string value)
		{
			if (_cfgDictionary.ContainsKey(section))
			{
				if (_cfgDictionary[section].ContainsKey(key))
				{
					_cfgDictionary[section][key] = value;
				}
				else
				{
					_cfgDictionary[section].Add(key, value);
				}
			}
			else
			{
				var newSection = new Dictionary<string, string> { { key, value } };
				_cfgDictionary.Add(section, newSection);
			}
		}
		
		public static void WriteConfigFile()
		{
			if (File.Exists(PathToCfg))
			{
				File.Delete(PathToCfg);
			}
			
			using var sw = File.AppendText(PathToCfg);

			foreach (var sectionDict in _cfgDictionary)
			{
				sw.WriteLine($"[{sectionDict.Key.ToString()}]");
				foreach (var pair in sectionDict.Value)
				{
					sw.WriteLine($"{pair.Key} = {pair.Value}");
				}
				sw.WriteLine();
			}
		}

		public static bool RemoveKey(Section section, string key)
		{
			if (!_initialized)
			{
				return false;
			}

			if (_cfgDictionary.TryGetValue(section, out var sectionDict))
			{
				if (sectionDict.ContainsKey(key))
				{
					sectionDict.Remove(key);
					return true;
				}
			}

			return false;
		}

		public static bool RemoveSection(Section section)
		{
			if (!_initialized)
			{
				return false;
			}
			
			if (_cfgDictionary.ContainsKey(section))
			{
				_cfgDictionary.Remove(section);
				return true;
			}

			return false;
		}

		public static string GetValue(Section section, string key)
		{
			if (!_initialized)
			{
				return string.Empty;
			}

			if (_cfgDictionary.TryGetValue(section, out var sectionDict))
			{
				if (sectionDict.TryGetValue(key, out var value))
				{
					return value;
				}
			}

			return string.Empty;
		}
		
		public static Dictionary<string, string> GetSection(Section section)
		{
			if (!_initialized)
			{
				return null;
			}

			return _cfgDictionary.GetValueOrDefault(section);
		}
	}

	public enum Section
	{
		KeyBinds,
	}
}