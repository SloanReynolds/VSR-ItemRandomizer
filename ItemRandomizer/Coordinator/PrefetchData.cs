using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ItemRandomizer.Coordinator {
	public static class PrefetchData {
		public static bool BusyLoading { get; private set; } = false;
		public static bool FullyLoaded { get; private set; } = false;

		private static Dictionary<string, GameObject> _prefetched = new();

		public static MonoBehaviour FetchItem(Item item) {
			if (item is Item.Card) {
				return FetchCard(((Item.Card)item).ID);
			}
			if (item is Item.Decrypt) {
				return FetchDecryptor(((Item.Decrypt)item).ID);
			}
			if (item is Item.Heart) {
				return FetchHeartUpg(((Item.Heart)item).ID);
			}
			if (item is Item.Orb) {
				return FetchOrb(((Item.Orb)item).ID);
			}
			if (item is Item.Phase) {
				return FetchPhaseUpg(((Item.Phase)item).ID);
			}
			throw new Exception("lol u died");
		}

		public static CreatureCardPickup FetchCard(string creature) {
			GameObject target = _prefetched["card"];
			CreatureCardPickup ccp = target.GetComponent<CreatureCardPickup>();
			ccp.creature = creature;

			return GameObject.Instantiate(target).GetComponent<CreatureCardPickup>();
		}

		public static DecryptorPickup FetchDecryptor(Decryptor.ID v) {
			GameObject target = _prefetched["decrypt"];
			DecryptorPickup dp = target.GetComponent<DecryptorPickup>();
			dp.decryptor = v;

			return GameObject.Instantiate(target).GetComponent<DecryptorPickup>();
		}

		public static HealthUpgradePickup FetchHeartUpg(PhysicalUpgrade.HealthUpgrade v) {
			GameObject target = _prefetched["heart"];
			HealthUpgradePickup hup = target.GetComponent<HealthUpgradePickup>();
			hup.upgradeID = v;

			return GameObject.Instantiate(target).GetComponent<HealthUpgradePickup>();
		}

		public static Orb FetchOrb(PhysicalUpgrade.Orb v) {
			GameObject target = _prefetched[$"orb{(int)v}"];
			//Orb orb = target.GetComponent<Orb>();

			return GameObject.Instantiate(target).GetComponent<Orb>();
		}

		public static PhaseUpgradePickup FetchPhaseUpg(PhysicalUpgrade.PhaseUpgrade v) {
			GameObject target = _prefetched["phase"];
			PhaseUpgradePickup pup = target.GetComponent<PhaseUpgradePickup>();
			pup.upgradeID = v;

			return GameObject.Instantiate(target).GetComponent<PhaseUpgradePickup>();
		}

		private static bool _Contains(string objName) {
			return _prefetched.ContainsKey(objName);
		}

		private static void _Add(string objName, GameObject obj) {
			obj.AddComponent<RandoPickup>();
			_prefetched.Add(objName, obj);
		}

		public static IEnumerator PrefetchAll(Definitions instance) {
			_StartPreloading();

			List<PrefetchRequest> prs = new List<PrefetchRequest>() {
				new PrefetchRequest("calm_tundra_1", "DecryptorPickup", "decrypt"),
				new PrefetchRequest("calm_tundra_1", "CreatureCardPickup", "card"),
				new PrefetchRequest("first_phase_upgrade", "PhaseUpgradePickup", "phase"),
				new PrefetchRequest("first_health_upgrade", "HealthUpgradePickup", "heart"),
				new PrefetchRequest("orb0", "Orb0", "orb0", (go) => _PreventDelete(go)),
				new PrefetchRequest("orb1", "Orb1", "orb1", (go) => _PreventDelete(go)),
				new PrefetchRequest("orb2", "Orb2", "orb2", (go) => _PreventDelete(go)),
				new PrefetchRequest("orb3", "Orb3", "orb3", (go) => _PreventDelete(go))
			};

			//Load each scene, once.
			foreach (string sceneName in prs.Select(pr => pr.SceneName).Distinct()) {
				yield return SceneManager.LoadSceneAsync(sceneName);
				while (SceneManager.GetActiveScene().name != sceneName) {
					yield return new WaitForEndOfFrame();
				}

				//For each scene, fetch all the items
				foreach (PrefetchRequest pr in prs.Where(pr => pr.SceneName == sceneName)) {
					if (_Contains(pr.NameInDictionary)) {
						//We've already prefetched this object.
						continue;
					}

					GameObject targ = GameObject.Find(pr.TargetName);

					pr.PreInstantiationCallback?.Invoke(targ);

					GameObject newO = GameObject.Instantiate(targ);
					newO.transform.SetParent(instance.transform, false);
					newO.SetActive(false);

					_Add(pr.NameInDictionary, newO);
				}
			}

			_CompletePreloading();
			Vars.goToTitleScreen();

			//Orb nodeData is funky...
			void _PreventDelete(GameObject go) {
				PhysicalUpgrade.Orb orb = go.GetComponent<Orb>().orb;
				if (Vars.currentNodeData.orbCollected(orb)) {
					Vars.currentNodeData.orbUndo(orb);
				}
			}
		}

		private static void _StartPreloading() {
			BusyLoading = true;
		}

		private static void _CompletePreloading() {
			BusyLoading = false;
			FullyLoaded = true;
		}

		private class PrefetchRequest {
			public string SceneName { get; private set; } = "";
			public string TargetName { get; private set; } = "";
			public string NameInDictionary { get; private set; } = "";
			public Action<GameObject> PreInstantiationCallback { get; private set; } = null;

			public PrefetchRequest(string sceneName, string targetName) : this(sceneName, targetName, targetName) { }

			public PrefetchRequest(string sceneName, string targetName, string nameInDict) : this(sceneName, targetName, nameInDict, null) { }

			public PrefetchRequest(string sceneName, string targetName, string nameInDict, Action<GameObject> preCallback) {
				SceneName = sceneName;
				TargetName = targetName;
				NameInDictionary = nameInDict;
				PreInstantiationCallback = preCallback;
			}
		}
	}
}