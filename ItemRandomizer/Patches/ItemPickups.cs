using System;
using System.Linq;
using HarmonyLib;
using ItemRandomizer.Coordinator;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ItemRandomizer.Patches {
	[HarmonyPatch]
	class ItemPickups {
		[HarmonyPrefix]
		[HarmonyPatch(typeof(NodeData), "healthUpgradeCollect")]
		static void healthObtain(NodeData __instance, PhysicalUpgrade.HealthUpgrade healthUpgrade) {
			if (Vars.currentNodeData != __instance) return;
			GameState.ReportObtained(new Item.Heart(healthUpgrade));
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(NodeData), "healthUpgradeUndo")]
		static void healthUndo(NodeData __instance, PhysicalUpgrade.HealthUpgrade healthUpgrade) {
			if (Vars.currentNodeData != __instance) return;
			GameState.ReportUndo(new Item.Heart(healthUpgrade));
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(NodeData), "phaseUpgradeCollect")]
		static void phaseObtain(NodeData __instance, PhysicalUpgrade.PhaseUpgrade phaseUpgrade) {
			if (Vars.currentNodeData != __instance) return;
			GameState.ReportObtained(new Item.Phase(phaseUpgrade));
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(NodeData), "phaseUpgradeUndo")]
		static void phaseUndo(NodeData __instance, PhysicalUpgrade.PhaseUpgrade phaseUpgrade) {
			if (Vars.currentNodeData != __instance) return;
			GameState.ReportUndo(new Item.Phase(phaseUpgrade));
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(NodeData), "orbCollect")]
		static void orbObtain(NodeData __instance, PhysicalUpgrade.Orb orb) {
			if (Vars.currentNodeData != __instance) return;
			GameState.ReportObtained(new Item.Orb(orb));
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(NodeData), "orbUndo")]
		static void orbUndo(NodeData __instance, PhysicalUpgrade.Orb orb) {
			if (Vars.currentNodeData != __instance) return;
			GameState.ReportUndo(new Item.Orb(orb));
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(NodeData), "creatureCardCollect", new Type[] { typeof(int) })]
		static void cardObtain(NodeData __instance, int creatureID) {
			if (Vars.currentNodeData != __instance) return;
			GameState.ReportObtained(new Item.Card(CreatureCard.getCardNameFromID(creatureID)));
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(NodeData), "creatureCardCollectUndo", new Type[] { typeof(int) })]
		static void cardUndo(NodeData __instance, int creatureID) {
			if (Vars.currentNodeData != __instance) return;
			GameState.ReportUndo(new Item.Card(CreatureCard.getCardNameFromID(creatureID)));
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(DecryptorPickup), "obtain")]
		static void decryptObtain(DecryptorPickup __instance) {
			//obtained
			GameState.ReportObtained(new Item.Decrypt(__instance.decryptor));
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(Vars), "orbGridCoords")]
		static bool Vars_orbGridCoords_Pre(ref Vector2 __result, PhysicalUpgrade.Orb orb) {
			Location orbLoc = RandoState.Locations.Where(l => l.CurrentItem is Item.Orb && ((Item.Orb)l.CurrentItem).ID == orb).FirstOrDefault();
			if (orbLoc != null) {
				Vector2 orbVec = orbLoc.MapCoords;

				__result = new Vector2(orbVec.x, orbVec.y);
			} else {
				Plugin.I.LogWarning($"Orb {orb} Map Location not found!");
				__result = new Vector2(1f, 1f);
			}

			return false;
		}
	}
}
