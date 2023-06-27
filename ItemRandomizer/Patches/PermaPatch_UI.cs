using System.Collections.Generic;
using HarmonyLib;
using ItemRandomizer.Coordinator;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ItemRandomizer.Patches {
	[HarmonyPatch]
	public class PermaPatch_UI {

		public static bool BlockInputOnTitleScene = false;

		//Title Screen UI changes
		[HarmonyPostfix]
		[HarmonyPatch(typeof(TitleScreen), "hideOptions")]
		static void TitleScreen_hideOptions(TitleScreen __instance) {
			__instance.GetComponent<TitleScreenStuff>().HideOptions();
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(TitleScreen), "showOptions")]
		static void TitleScreen_showOptions(TitleScreen __instance) {
			__instance.GetComponent<TitleScreenStuff>()?.ShowOptions();
		}

		//Credits
		[HarmonyPrefix]
		[HarmonyPatch(typeof(Credits), "Awake")]
		static void Credits_Awake(Credits __instance) {
			if (!Configs.Enabled) return;

			__instance.gameObject.AddComponent<CreditsStuff>().Credits = __instance;
		}

		//"New" Title screen animations
		[HarmonyPostfix]
		[HarmonyPatch(typeof(TitleAnimation), "Start")]
		static void TitleAnimation_Start(GameObject ___larvaOracle) {
			___larvaOracle.gameObject.AddComponent<TitleOracle>();
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(TitleAnimation), "generateLoopers")]
		static void TitleAnimation_generateLoopers(GameObject ___larvaOracle, List<GameObject> ___platforms, List<GameObject> ___rails) {
			___larvaOracle.gameObject.GetComponent<TitleOracle>()?.UpdatePlatforms_Post(___platforms, ___rails);
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(Keys), "upPressed", MethodType.Getter)]
		[HarmonyPatch(typeof(Keys), "downPressed", MethodType.Getter)]
		[HarmonyPatch(typeof(Keys), "confirmPressed", MethodType.Getter)]
		[HarmonyPatch(typeof(Keys), "startPressed", MethodType.Getter)]
		static bool KeyBlockers(ref bool __result) {
			if (BlockInputOnTitleScene && SceneManager.GetActiveScene().name == "title_scene") {
				__result = false;
				return false;
			}

			return true;
		}
	}
}
