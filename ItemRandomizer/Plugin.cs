using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using ItemRandomizer.Coordinator;
using UnityEngine.SceneManagement;

namespace ItemRandomizer {
	[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	public class Plugin : BaseUnityPlugin {
		public static Plugin I;
		private static List<Harmony> _unpatchable = null;
		private static List<Harmony> _permaPatches = null;

		public event EventHandler EStart;

		private void Awake() {
			I = this;
			_LoadEmbeddedDLLs();

			//Coordinator.Config.LoadConfigFile();

			gameObject.AddComponent<Definitions>();
			gameObject.AddComponent<Replacement>();
			gameObject.AddComponent<PuzzlesManager>();

			SceneManager.sceneLoaded += this._SceneManager_sceneLoaded;

			_ApplyAllPatches();
			Configs.LoadFromFile();

			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} v{PluginInfo.PLUGIN_VERSION} is loaded!");
		}

		private void _SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode) {
			if (scene.name == "title_scene") {
				FindObjectOfType<TitleScreen>().gameObject.AddComponent<TitleScreenStuff>();
			}
		}

		private void Start() {
			EStart?.Invoke(this, EventArgs.Empty);
		}

		private void Update() {
		}

		private void _LoadEmbeddedDLLs() {
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(
				(s, a) => {
					string reqName = a.Name.Substring(0, a.Name.IndexOf(","));
					string resourceName = $"ItemRandomizer.Data.Lib.{reqName}.dll";
					if (typeof(Plugin).Assembly.GetManifestResourceNames().Contains(resourceName)) {
						using (BinaryReader br = new BinaryReader(typeof(Plugin).Assembly.GetManifestResourceStream(resourceName))) {
							return Assembly.Load(br.ReadBytes((int)br.BaseStream.Length));
						}
					}

					return null;
				});
		}

		private void _ApplyAllPatches() {
			if (_permaPatches == null) {
				_permaPatches = new List<Harmony>();
				_permaPatches.Add(Harmony.CreateAndPatchAll(typeof(Patches.PermaPatch_UI), PluginInfo.PLUGIN_GUID));
				_permaPatches.Add(Harmony.CreateAndPatchAll(typeof(Patches.PermaPatch_General), PluginInfo.PLUGIN_GUID));
			}
			ApplyRandoPatches();
		}

		public void ApplyRandoPatches() {
			if (_unpatchable == null) {
				_unpatchable = new List<Harmony>();
				_unpatchable.Add(Harmony.CreateAndPatchAll(typeof(Patches.GrigerFight), PluginInfo.PLUGIN_GUID));
				_unpatchable.Add(Harmony.CreateAndPatchAll(typeof(Patches.ItemPickups), PluginInfo.PLUGIN_GUID));
				_unpatchable.Add(Harmony.CreateAndPatchAll(typeof(Patches.GlitchEncounter), PluginInfo.PLUGIN_GUID));
				_unpatchable.Add(Harmony.CreateAndPatchAll(typeof(Patches.General), PluginInfo.PLUGIN_GUID));
			}
		}

		public void UnapplyRandoPatches() {
			if (_unpatchable != null) {
				foreach (Harmony item in _unpatchable) {
					item.UnpatchSelf();
				}
				_unpatchable = null;
			}
		}

		public void LogInfo(string msg) {
			Logger.LogInfo(msg);
		}

		internal void LogWarning(string msg) {
			Logger.LogWarning(msg);
		}

		public void LogError(string msg) {
			Logger.LogError(msg);
		}
	}
}
