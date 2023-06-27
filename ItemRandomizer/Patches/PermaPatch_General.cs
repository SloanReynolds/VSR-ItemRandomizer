using HarmonyLib;
using ItemRandomizer.Coordinator;

namespace ItemRandomizer.Patches {
	[HarmonyPatch]
	class PermaPatch_General {
		[HarmonyPrefix]
		[HarmonyPatch(typeof(TitleScreen), "playGameSelected")]
		static bool startGame() {
			Configs.SaveToFile();

			//If Rando is disabled, do the normal thing
			if (!Configs.Enabled) {
				Plugin.I.UnapplyRandoPatches();
				return true;
			}

			Plugin.I.ApplyRandoPatches();

			GameState.BeginNewGame();
			GameState.SkipTutorial();

			//Skip normal behavior.
			return false;
		}

		/*
		[HarmonyTranspiler]
		[HarmonyPatch(typeof(FileSelectScreen), "createFileSelects")]
		static IEnumerable<CodeInstruction> createFileSelects_Transpiler(IEnumerable<CodeInstruction> inst) {
			List<CodeInstruction> list = new List<CodeInstruction>(inst);
			for (var i = 0; i < list.Count; i++) {
				if (list[i].opcode == Rflt.OpCodes.Ldc_I4_4 && (list[i + 1].opcode == Rflt.OpCodes.Blt || list[i + 1].opcode == Rflt.OpCodes.Blt_S)) {
					//line 'Blt' = i < 4
					//line 'Blt_s' = j < 4
					list[i].opcode = Rflt.OpCodes.Ldc_I4_5;
				}
			}
			return list;
		}
		*/
	}
}
