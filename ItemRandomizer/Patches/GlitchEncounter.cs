using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace ItemRandomizer.Patches {
	[HarmonyPatch]
	class GlitchEncounter {
		[HarmonyPostfix]
		[HarmonyPatch(typeof(GlitchDecryptorAttack), "Start")]
		static void GlitchDecryptorAttack_Start(GlitchDecryptorAttack __instance) {
			MethodInfo mi = typeof(GlitchDecryptorAttack).GetMethod("hide", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			mi?.Invoke(__instance, new object[] { });

			GameObject.Destroy(GameObject.FindObjectOfType<DecryptorPickup>().gameObject);
		}
	}
}
