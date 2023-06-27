using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ItemRandomizer;
using ItemRandomizer.Coordinator;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DapperHelper {
	public class DapperDebug : MonoBehaviour {
		private static DapperLog _Log = new DapperLog();

		bool listeningForDecryptID = false;
		string decryptID = "";
		DecryptorText newText = null;

		void Awake() {
			//SceneManager.sceneLoaded += this._SceneManager_sceneLoaded;
		}

		private void _SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1) {
			Plugin.I.LogInfo(scene.name);
			if (_killed.Count > 0) {
				foreach (var item in _killed) {
					GameObject.Find(item).SetActive(false);
				}
			}
		}

		void Update() {
			if (Input.GetKeyDown(KeyCode.Joystick8Button14)) {
				DebugAllObjects(new[] { typeof(GlyphBox), typeof(Orb), typeof(HealthUpgradePickup), typeof(PhaseUpgradePickup), typeof(CreatureCardPickup), typeof(DecryptorPickup) });
			}

			if (Input.GetKeyDown(KeyCode.Joystick8Button14)) {
				//ToggleDecryptor(Decryptor.ID.SPIN_DODGE);
				foreach (Location loc in RandoState.Locations) {
					Plugin.I.LogInfo($"{loc.Scene}:{loc.OriginalItem.IDStr} <= contains => {loc.CurrentItem.IDStr}");
				}
			}

			if (Input.GetKeyDown(KeyCode.F4)) {
				Vars.collectDecryptor(Decryptor.ID.CHARGE_SHOT);
				Vars.collectDecryptor(Decryptor.ID.DOUBLE_SHOT);
				Vars.collectDecryptor(Decryptor.ID.SPIN_DODGE);
				Vars.collectDecryptor(Decryptor.ID.WALL_RUN);

				//_loadLevel("griger_clue_4", new Vector2(30.4f, 11.8f));
				{
					_loadLevel("base_landing", new Vector2(79f, 23.5f));
					//_kill("Cottospark");
				}
			}

			if (Input.GetKeyDown(KeyCode.F1)) {
				Vars.eventHappen(AdventureEvent.Info.PUZZLE_PANEL_SOLVED_CUTSCENE);
			}

			if (Input.GetKeyDown(KeyCode.F2)) {
				Vars.eventHappen(AdventureEvent.Info.CONFIRMED_GRIGER_SOLUTION);
			}

			if (Input.GetKeyDown(KeyCode.F3)) {
				Vars.eventHappen(AdventureEvent.Info.CONFIRMED_MUSICLAND_SOLUTION);
			}

			if (Input.GetKeyDown(KeyCode.F9)) {
				foreach (var location in RandoState.Locations) {
					Plugin.I.LogInfo($" {location.Scene}/{location.OriginalItem.IDStr} holds {location.CurrentItem.IDStr}");
				}
			}

			if (Input.GetKeyDown(KeyCode.Joystick8Button14)) {
				Vars.loadLevel("panel_solution");
			}

			if (Input.GetKeyDown(KeyCode.Joystick8Button14)) {
				if (listeningForDecryptID) {
					if (int.TryParse(decryptID, out int parsed)) {
						if (parsed > 0 && parsed < 41) {
							ToggleDecryptor((Decryptor.ID)parsed);
						}

					}
					decryptID = "";
					listeningForDecryptID = false;
				} else {
					listeningForDecryptID = true;
				}
			}

			if (listeningForDecryptID) {
				decryptID += Input.inputString;
			}

			if (Input.GetKeyDown(KeyCode.Joystick8Button14)) {
				//Console.WriteLine("Load Scene # " + scene);
				SceneManager.LoadScene(scene);
				scene++;
			}

			if (Input.GetKeyDown(KeyCode.F12) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftControl))) {
				StartCoroutine(SearchAndDebug(new[] { typeof(DecryptorPickup), typeof(CreatureCardPickup), typeof(HealthUpgradePickup), typeof(PhaseUpgradePickup) }));
			}

			if (newText != null) {
				if (newText.closed) {
					//Destroy(newText.gameObject);
					newText = null;
				}
			}

			void ToggleDecryptor(Decryptor.ID decryptor) {
				_Log.Write(decryptor.ToString()); ;
				_Log.Write(Vars.abilityKnown(decryptor).ToString());
				if (Vars.abilityKnown(decryptor)) {
					if (Vars.decryptors.Contains(decryptor)) {
						Vars.decryptors.Remove(decryptor);
					}
				} else {
					Vars.collectDecryptor(decryptor);

					GameObject go = Instantiate(Resources.FindObjectsOfTypeAll<DecryptorText>()[0].gameObject);
					go.SetActive(true);
					go.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, worldPositionStays: false);
					newText = go.GetComponent<DecryptorText>();
					newText.display(decryptor);
				}
				_Log.Flush();
			}
		}

		private List<string> _killed = new();
		private void _kill(string v) {
			_killed.Add(v);
		}

		private void _loadLevel(string sceneName, Vector2 vector2) {
			Vars.loadLevel(sceneName);
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			player.transform.localPosition = vector2;
		}

		static int scene = -1;

		static IEnumerator SearchAndDebug(Type[] T) {
			for (int i = 0; i < 500; i++) {
				yield return SceneManager.LoadSceneAsync(i);
				while (SceneManager.GetActiveScene().buildIndex != i) {
					yield return new WaitForEndOfFrame();
				}

				Plugin.I.LogInfo(SceneManager.GetActiveScene().name);
				DebugAllObjects(T);
			}

			_Log.Flush();
		}

		static void DebugAllObjects(Type[] T) {
			foreach (Type type in T) {
				DebugAllObjects(type);
			}
		}

		static void DebugAllObjects(Type T) {
			UnityEngine.Object[] all = Resources.FindObjectsOfTypeAll(T);

			foreach (UnityEngine.Object obj in all) {
				if (obj is Component) {
					if (obj.name.Contains("Clone"))
						continue;
					DebugObject(((Component)obj).gameObject);
				}
			}
		}

		public static void DebugObject(GameObject go, string preface = "") {
			_Log.Write($"GameObject Dump: {go.name}     [Scene: {SceneManager.GetActiveScene().name}]");
			Transform parent = go.transform.parent;
			if (parent != null) {
				_Log.Write("=> Parents:");
				while (parent != null) {
					_LogObject(parent.gameObject, "=> ");

					parent = parent.parent;
				}
			}
			if (preface != "") {
				_Log.Write($"----{preface}----");
			}
			_RecurseLogObj(go);
			_Log.Flush();
		}














		private static void _RecurseLogObj(GameObject go, List<GameObject> filter = null, string depth = "") {
			//If there's a filter, and this current object is not in it, we stop.
			if (filter != null && !filter.Contains(go)) {
				return;
			}

			_LogObject(go, depth);

			foreach (Transform transform in go.transform) {
				_RecurseLogObj(transform.gameObject, filter, ">>" + depth);
			}
		}

		private static void _LogObject(GameObject go, string prefix = "") {
			_Log.Write("");

			_Log.Write($"{prefix}({(go.activeInHierarchy ? 'a' : 'i')}, {go.gameObject.scene.buildIndex}) {go.name} [{go.layer}, {go.tag}] @ {go.transform.position.x}, {go.transform.position.y} ({go.transform.localPosition.x}, {go.transform.localPosition.y})");

			string[] p = go.GetComponents<Component>()
				.Select(com => com.GetType().ToString())
				.ToArray();

			_Log.Write($"{prefix}  <{string.Join(", ", p)}>");

			foreach (GlyphBox glyphBox in go.GetComponents<GlyphBox>()) {
				_Log.Write($"{prefix}  GlyphBox - {glyphBox.formattedText}");
			}

			foreach (Orb orb in go.GetComponents<Orb>()) {
				string orbState = "null";
				try {
					orbState = orb?.state.ToString();
				} catch { }
				_Log.Write($"{prefix}  Orb - state {orbState} | flashColor {orb?.flashColor} | orb {orb?.orb} | obtained {Vars.currentNodeData?.orbCollected(orb.orb)}");
				_Log.Write($"{prefix}  |*|Orb|{SceneManager.GetActiveScene().name}|{orb.gameObject.name}");
			}
			foreach (DecryptorPickup dp in go.GetComponents<DecryptorPickup>()) {
				_Log.Write($"{prefix}  |*|DecryptorPickup|{SceneManager.GetActiveScene().name}|{dp.gameObject.name}|{dp.decryptor}");
			}
			foreach (CreatureCardPickup ccp in go.GetComponents<CreatureCardPickup>()) {
				_Log.Write($"{prefix}  |*|CreatureCardPickup|{SceneManager.GetActiveScene().name}|{ccp.gameObject.name}|\"{ccp.creature}\"");
			}
			foreach (PhaseUpgradePickup pup in go.GetComponents<PhaseUpgradePickup>()) {
				_Log.Write($"{prefix}  |*|PhaseUpgradePickup|{SceneManager.GetActiveScene().name}|{pup.gameObject.name}|{pup.upgradeID}");
			}
			foreach (HealthUpgradePickup hup in go.GetComponents<HealthUpgradePickup>()) {
				_Log.Write($"{prefix}  |*|HealthUpgradePickup|{SceneManager.GetActiveScene().name}|{hup.gameObject.name}|{hup.upgradeID}");
			}
		}
		//DecryptorPickup		public Decryptor.ID decryptor (public enum)
		//CreatureCardPickup	public string creature (Need to have Start() get called somehow)
		//PhaseUpgradePickup	public PhysicalUpgrade.PhaseUpgrade upgradeid (public enum)
		//HealthUpgradePickup	public PhysicalUpgrade.HealthUpgrade upgradeid (public enum)
		//Orb					public PhysicalUpgrade.Orb upgradeid (public enum)

		// 24 Cards
		// 4 Hearts
		// 4 Phases
		// 16 Upgrades
		// 4 Orbs

		//Magoom
		//Ciurivy
		//Smosey
		//Wavemoth
		//Midow
		//Prittle
		//Sealime
		//Toucade
		//Shaifi
		//Pengrunt
		//Thermal Spade
		//Cottospark
		//Cottocache
		//Drilbas
		//Jinvell
		//Royalrose
		//Rupo
		//Froesburn
		//Ghostily
		//Sherivice
		//Griger
		//Solatia
		//Salesman
		//Oracle
		//Oracle-L
		//Wally
	}
}
