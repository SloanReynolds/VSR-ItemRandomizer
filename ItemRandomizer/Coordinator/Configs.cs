using System.IO;
using UnityEngine;

namespace ItemRandomizer.Coordinator {
	public static class Configs {
		public static bool Enabled { get; set; } = true;
		public static bool RandomizePuzzles { get; set; } = true;

		public static bool NetworkEnabled = true;
		public static string Username { get; set; } = "Anonymous Hyperebra";

		public static string EnabledVerbiage => Enabled ? "Enabled" : "Disabled";
		public static string PuzzlesVerbiage => RandomizePuzzles ? "Yes" : "No";
		public static string NetworkVerbiage => NetworkEnabled ? "Yes" : "No";



		private static string _ConfigPath => Application.persistentDataPath + "/rando-settings.cfg";

		private static bool _TryGetBool(string line, string key, out bool value) {
			if (line.StartsWith(key)) {
				value = bool.Parse(line.Replace($"{key}=", ""));
				return true;
			}

			value = false;
			return false;
		}

		private static bool _TryGetString(string line, string key, out string value) {
			if (line.StartsWith(key)) {
				value = line.Replace($"{key}=", "");
				return true;
			}

			value = "";
			return false;
		}

		public static void LoadFromFile() {
			if (!File.Exists(_ConfigPath)) return;
			using (StreamReader file = File.OpenText(_ConfigPath)) {
				do {
					string line = file.ReadLine();
					if (_TryGetBool(line, "Enabled", out bool enabled)) { Enabled = enabled; continue; }
					if (_TryGetBool(line, "RandomizePuzzles", out bool puzzles)) { RandomizePuzzles = puzzles; continue; }
					if (_TryGetBool(line, "NetworkEnabled", out bool network)) { NetworkEnabled = network; continue; }
					if (_TryGetString(line, "Username", out string username)) { Username = username; continue; }
				} while (!file.EndOfStream);
			}
		}

		public static void SaveToFile() {
			using (StreamWriter file = File.CreateText(_ConfigPath)) {
				file.WriteLine($"Enabled={(Enabled ? "true" : "false")}");
				file.WriteLine($"RandomizePuzzles={(RandomizePuzzles ? "true" : "false")}");
				file.WriteLine($"NetworkEnabled={(NetworkEnabled ? "true" : "false")}");
				file.WriteLine($"Username={Username}");
			}
		}
	}
}
