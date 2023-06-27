using HarmonyLib;
using UnityEngine;

namespace ItemRandomizer.Patches {
	[HarmonyPatch]
	class GrigerFight {

		[HarmonyPrefix]
		[HarmonyPatch(typeof(AlteredGrigerArenaContainer), "Awake")]
		static void AlteredGrigerArenaContainer_AwakePre(AlteredGrigerArenaContainer __instance) {
			__instance.gameObject.AddComponent<GrigerContainer>();
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(AlteredGrigerArenaContainer), "Update")]
		static bool AlteredGrigerArenaContainer_Update(AlteredGrigerArenaContainer __instance, TimeUser ___timeUser, DecryptorPickup ___decryptorRef, TUBool ___decryptorExists) {
			if (___decryptorRef != null) return true;

			if (!___timeUser.shouldNotUpdate && ___decryptorExists.v && !__instance.GetComponent<GrigerContainer>().RandoPickup.GetComponent<TimeUser>().exists) {
				___decryptorExists.v = false;
			}

			return false;
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(AlteredGrigerArenaContainer), "FixedUpdate")]
		static bool AlteredGrigerArenaContainer_FixedUpdate(AlteredGrigerArenaContainer __instance, TimeUser ___timeUser, DecryptorPickup ___decryptorRef, TUFloat ___time, TUInt ___tuState, Vector2 ___startPos, Vector2 ___position1, Vector2 ___position2, float ___duration1, float ___duration2, float ___startRotation, float ___rotation1, float ___rotation2, bool ___moveDecryptor, TUBool ___decryptorExists, Vector2 ___decryptorPos, Rigidbody2D ___rb2d) {
			if (___decryptorRef != null) return true;

			if (!___timeUser.shouldNotUpdate) {
				___time.v += Time.fixedDeltaTime;
				Vector2 position = ___rb2d.position;
				float angle = ___rb2d.rotation;
				switch (___tuState.v) {
					case 1:
						position = Utilities.easeInQuadClamp(___time.v, ___startPos, ___position1 - ___startPos, ___duration1);
						angle = Utilities.easeInQuadClamp(___time.v, ___startRotation, ___rotation1 - ___startRotation, ___duration1);
						break;
					case 2:
						position = Utilities.easeInQuadClamp(___time.v, ___position1, ___position2 - ___position1, ___duration2);
						angle = Utilities.easeInQuadClamp(___time.v, ___rotation1, ___rotation2 - ___rotation1, ___duration2);
						break;
				}
				___rb2d.MoveRotation(angle);
				___rb2d.MovePosition(position);
				if (___moveDecryptor && ___decryptorExists.v) {
					__instance.GetComponent<GrigerContainer>().RandoPickup.changePosition(__instance.transform.TransformPoint(___decryptorPos));
				}
			}

			return false;
		}
	}
}
