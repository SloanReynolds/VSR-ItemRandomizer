using System;
using System.Linq;
using BepInEx;

namespace DapperHelper {
#if DEBUG
	[BepInPlugin("SDIFUSHGIRUGHSIURGBNSIGUBNSIDUVBBDapperDebug", "DapperDebug", "0.0.0")]
	[BepInDependency(ItemRandomizer.PluginInfo.PLUGIN_GUID)]
#endif
	class Plugin : BaseUnityPlugin {
		public static Plugin I;

		private void Awake() {
			I = this;

#if DEBUG
			ItemRandomizer.Plugin.I.EStart += HandleAwake;
#endif
		}

		private void HandleAwake(object sender, EventArgs e) {
			var open = WindowsHelper.GetOpenWindows();

			WindowsHelper.SetWindowPosition(open.Where(kvp => kvp.Value == "BepInEx 5.4.19.0 - Vision Soft Reset").FirstOrDefault().Key, -1200, 50);
			//WindowsHelper.SetFocus(open.Where(kvp => kvp.Value == "BepInEx 5.4.19.0 - Vision Soft Reset").FirstOrDefault().Key);
			WindowsHelper.SetFocus(open.Where(kvp => kvp.Value == "Vision Soft Reset").FirstOrDefault().Key);

			((ItemRandomizer.Plugin)sender).gameObject.AddComponent<DapperDebug>();
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
