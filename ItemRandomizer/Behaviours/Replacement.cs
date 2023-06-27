using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using ItemRandomizer.Coordinator;
using ItemRandomizer.Resource;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ItemRandomizer {
	public class Replacement : MonoBehaviour {
		void Awake() {
			SceneManager.sceneLoaded += this._SceneManager_sceneLoaded;
		}

		private void _SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode) {
			if (!PrefetchData.FullyLoaded) {
				return;
			}

			string curScene = SceneManager.GetActiveScene().name;
			foreach (var item in Resources.FindObjectsOfTypeAll<GlyphBox>()) {
				var rect = item.GetComponent<RectTransform>();
				if (rect!=null) {
					//Plugin.I.LogInfo($"{rect.gameObject.name}");
					//Plugin.I.LogInfo($"=> anchorMin - {rect.anchorMin.x} {rect.anchorMin.y}");
					//Plugin.I.LogInfo($"=> anchorMax - {rect.anchorMax.x} {rect.anchorMax.y}");
					//Plugin.I.LogInfo($"=> anchoredPosition - {rect.anchoredPosition.x} {rect.anchoredPosition.y}");
					//Plugin.I.LogInfo($"=> sizeDelta - {rect.sizeDelta.x} {rect.sizeDelta.y}");
					//Plugin.I.LogInfo($"=> offsetMin - {rect.offsetMin.x} {rect.offsetMin.y}");
					//Plugin.I.LogInfo($"=> offsetMax - {rect.offsetMax.x} {rect.offsetMax.y}");
					//Plugin.I.LogInfo($"=> localPosition - {rect.localPosition.x} {rect.localPosition.y}");
					//Plugin.I.LogInfo($"=> localScale - {rect.localScale.x} {rect.localScale.y}");
					//Plugin.I.LogInfo($"=> rect - {rect.rect.x} {rect.rect.y} {rect.rect.width} {rect.rect.height}");
				}
			}

			//Various Level Changes
			if (curScene == "all_chamber_puzzle") {
				AllChamberPuzzle acp = GameObject.FindObjectOfType<AllChamberPuzzle>();
				List<AllChamberPuzzle.Icon> iconList = acp.chamberIcons.ToList();
				SpriteRenderer sr = iconList[4].spriteRenderer;
				iconList.RemoveAt(4);
				Destroy(sr.gameObject);
				acp.chamberIcons = iconList.ToArray();
			} else if (curScene == "totem_c") {
				var pgcs = Resources.FindObjectsOfTypeAll<PolygonCollider2D>();
				foreach (var pgc in pgcs) {
					if (pgc.transform.parent?.name == "rocks_02") {
						var points = pgc.GetPath(0);
						points[5] = new Vector2(76, -11);
						points[6] = new Vector2(71.5f, -23);
						pgc.SetPath(0, points);
					}
				}

				//var mrs = Resources.FindObjectsOfTypeAll<MeshFilter>();
				//foreach (var mr in mrs) {
				//	Plugin.I.LogInfo(mr.name);
				//	foreach (var item in mr.mesh.vertices) {
				//		Plugin.I.LogInfo($"[{item.x}, {item.y}, {item.z}]");
				//	}

				//	for (int i = 0; i < mr.mesh.triangles.Length; i+=3) {
				//		Plugin.I.LogInfo($"<{mr.mesh.triangles[i]}, {mr.mesh.triangles[i + 1]}, {mr.mesh.triangles[i + 2]}>");
				//	}
				//}

				GameObject rail = new GameObject("RailsWall");
				SpriteRenderer sr = rail.AddComponent<SpriteRenderer>();
				sr.sprite = Sprites.RailsSprite;
				sr.sortingGroupID = 1048575;        //1111 1111 1111 1111 1111
				sr.sortingLayerID = 1772338085;     //Platforms
				sr.sortingOrder = 0;
				rail.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
				rail.transform.localPosition = new Vector3(75.55f, 5.7f, 0);
				rail.transform.eulerAngles = new Vector3(0, 0, 71f);
				rail.SetActive(true);
			}

			//Actual Item changes
			//Level lvl = FindObjectOfType<Level>();
			//if (lvl != null) Plugin.I.LogInfo($"{curScene} x{lvl.mapX} y{lvl.mapY}");
			if (RandoState.Locations.SceneNames().Contains(curScene)) {
				foreach (Location location in RandoState.Locations.ForScene(curScene)) {
					GameObject orig = GameObject.Find(location.GameObject);

					//Decryptor Components:
					//<UnityEngine.Transform, UnityEngine.SpriteRenderer, UnityEngine.Animator, UnityEngine.Rigidbody2D, UnityEngine.BoxCollider2D, TimeUser, UnityEngine.AudioSource, DecryptorPickup, RevealDecryptorDecryptor>

					//<UnityEngine.Transform, UnityEngine.SpriteRenderer, UnityEngine.Animator, UnityEngine.Rigidbody2D, UnityEngine.BoxCollider2D, TimeUser, CreatureCardPickup, InactiveIfPhysicalNotHappen>
					//  |*|CreatureCardPickup|solatia_run|CreatureCardBossPickup|"Solatia"
					//  |*|CreatureCardPickup|griger_pre_boss_elevator|CreatureCardBossPickup|"Griger"

					Item toFetch = location.CurrentItem;
					
					if (toFetch is Item.Decrypt) {
						if (toFetch.IDStr == "ALTERED_SHOT" && GameState.HasItem("ALTERED_SHOT")) {
							toFetch = new Item.Decrypt(Decryptor.ID.DOUBLE_SHOT);
						} else if (toFetch.IDStr == "DOUBLE_SHOT" && !GameState.HasItem("ALTERED_SHOT")) {
							toFetch = new Item.Decrypt(Decryptor.ID.ALTERED_SHOT);
						}
					}

					MonoBehaviour mb = PrefetchData.FetchItem(toFetch);
					
					mb.transform.SetParent(orig.transform.parent);
					mb.transform.position = orig.transform.position;

					if (curScene == "solatia_run") {
						if (location.OriginalItem.IDStr == "Card_Solatia".ToUpper()) {
							var inactive = mb.gameObject.AddComponent<InactiveIfPhysicalNotHappen>();
							inactive.physEvent = AdventureEvent.Physical.SOLATIA_DEFEATED;
						} else if (curScene == "griger_pre_boss_elevator") {
							var inactive = mb.gameObject.AddComponent<InactiveIfPhysicalNotHappen>();
							inactive.physEvent = AdventureEvent.Physical.GRIGER_DEFEATED;
						} else if (curScene == "altered_griger_arena") {
							//Plugin.I.LogInfo($"{location.CurrentItem}");
							//AlteredGrigerArenaContainer2 container = mb.gameObject.AddComponent<AlteredGrigerArenaContainer2>();
							if (toFetch is Item.Decrypt) {
								AlteredGrigerArena arena = FindObjectOfType<AlteredGrigerArena>();

								typeof(AlteredGrigerArenaContainer).GetField("decryptorRef", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).SetValue(arena.importantContainer, mb.GetComponent<DecryptorPickup>());
							}
						} else if (curScene == "totem_c") {
							mb.transform.localPosition = new Vector2(71.5f, 5.5f);
						}
					} else if (curScene == "griger_pre_boss_elevator") {
						var inactive = mb.gameObject.AddComponent<InactiveIfPhysicalNotHappen>();
						inactive.physEvent = AdventureEvent.Physical.GRIGER_DEFEATED;
					} else if (curScene == "altered_griger_arena") {
						//Plugin.I.LogInfo($"{location.CurrentItem}");
						//AlteredGrigerArenaContainer2 container = mb.gameObject.AddComponent<AlteredGrigerArenaContainer2>();
						if (toFetch is Item.Decrypt) {
							AlteredGrigerArena arena = FindObjectOfType<AlteredGrigerArena>();

							typeof(AlteredGrigerArenaContainer).GetField("decryptorRef", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).SetValue(arena.importantContainer, mb.GetComponent<DecryptorPickup>());
						}
					} else if (curScene == "totem_c") {
						mb.transform.localPosition = new Vector2(71.5f, 5.5f);
					}

					Destroy(orig);
					mb.gameObject.SetActive(true);
				}
			}
		}
	}
}
