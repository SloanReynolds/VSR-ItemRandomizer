using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace ItemRandomizer.Coordinator {
	class Reports {
		public static void Send_OutOfLogic(Location location, List<string> items) {
			if (!_UserHasAllowedSnooping()) {
				return;
			}

			RequestArguments ra = new RequestArguments() {
				ReportType = "OutOfLogic",
				Username = Configs.Username,
				GameVersion = PluginInfo.PLUGIN_VERSION,
				Seed = RandoState.Seed
			};

			ra.Add("location", location.ToString());
			ra.Add("options", Configs.RandomizePuzzles ? "puzzles" : "");
			ra.Add("decryptors", string.Join(",", items.Where(it => Definitions.ValidTokens.Contains(it))));

			_SendGet(ra);
		}

		private static bool _UserHasAllowedSnooping() {
			return Configs.NetworkEnabled;
		}

		private static async void _SendGet(RequestArguments args) {
			try {
				await Task.Run(() => {
					string url = $"http://www.lanimals.com/vsrir/{args.EndPoint}?{args.ArgumentsForGet()}";

					if (!_UserHasAllowedSnooping()) {
						return;
					}

					ItemRandomizer.Plugin.I.LogInfo($"Making webrequest");
					using (UnityWebRequest webRequest = UnityWebRequest.Get(url)) {
						webRequest.SendWebRequest();
						while (!webRequest.isDone) { }
					}
				});
			} catch (Exception ex) {
				Plugin.I.LogWarning($"Exception thrown while trying to report {args.ReportType}. Information DISCARDED.");
				Plugin.I.LogWarning(ex.ToString());
			}
		}

		private class RequestArguments {
			public string EndPoint { get; set; } = "out-of-logic.asp";
			public string ReportType { get; set; } = "unknown";
			public string GameVersion { get; set; }
			public string Username { get; set; } = "Anonymous Hyperebra";
			public uint Seed { get; set; }
			private Dictionary<string, string> _arguments = new Dictionary<string, string>();

			public void Add(string key, string value) {
				if (_arguments.ContainsKey(key)) {
					_arguments[key] += $",{value}";
					return;
				}

				_arguments.Add(key, value);
			}

			public string ArgumentsForGet() {
				string args = $"type={ReportType}&version={UnityWebRequest.EscapeURL(GameVersion)}&user={UnityWebRequest.EscapeURL(Username)}&seed={UnityWebRequest.EscapeURL($"{Seed}")}";
				foreach (string key in _arguments.Keys) {
					args += $"&{UnityWebRequest.EscapeURL(key)}={UnityWebRequest.EscapeURL(_arguments[key])}";
				}
				return args;
			}
		}
	}
}
