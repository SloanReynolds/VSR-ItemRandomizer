using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ItemRandomizer.Coordinator {
	public static class GameState {
		private static List<Item> _collected = new();

		internal static void ResetGame() {
			_collected = new();
			RandoState.Reset();
		}

		public static bool HasItem(string IDStr) {
			return _collected.Any(it => it.IDStr == IDStr);
		}

		public static void ReportObtained(Item item) {
			if (!PrefetchData.FullyLoaded || PrefetchData.BusyLoading) {
				return;
			}

			if (!_collected.Contains(item)) {
				_VerifyLogic(item, _collected);
				_collected.Add(item);
			}
		}

		private static void _VerifyLogic(Item item, List<Item> collected) {
			Location currentLocation = RandoState.Locations.WithCurrentItem(item);
			if (!currentLocation.Logic.Evaluate(collected)) {
#if DEBUG
				ItemRandomizer.Plugin.I.LogInfo($"Location reached without all known requisite items!!");
				ItemRandomizer.Plugin.I.LogInfo($"  Location: {currentLocation}");
				ItemRandomizer.Plugin.I.LogInfo($"  Decryptors Known:");
#endif

				List<string> list = new List<string>();
				foreach (Decryptor.ID id in Enum.GetValues(typeof(Decryptor.ID))) {
					if (Vars.abilityKnown(id)) {
#if DEBUG
						ItemRandomizer.Plugin.I.LogInfo($"    {id}");
#endif
						list.Add(id.ToString());
					}
				}
				Reports.Send_OutOfLogic(currentLocation, list);
			}
		}

		public static void ReportUndo(Item item) {
			if (_collected.Contains(item)) _collected.Remove(item);
		}

		public static void BeginNewGame() {
			int fileIndex = 4;

			Vars.deleteData(fileIndex);

			Properties properties = new Properties(string.Empty);
			string path = Application.persistentDataPath + "/quickData.sav";
			if (File.Exists(path)) {
				byte[] bytes = File.ReadAllBytes(path);
				string str = Utilities.bytesToString(bytes);
				properties.parse(str);
			}
			properties.remove("fn" + fileIndex);
			properties.remove("diff" + fileIndex);
			properties.remove("t" + fileIndex);
			properties.remove("info" + fileIndex);
			properties.remove("phys" + fileIndex);
			byte[] bytes2 = Utilities.stringToBytes(properties.convertToString());
			File.WriteAllBytes(path, bytes2);

			Vars.saveFileIndexLastUsed = fileIndex;
			Vars.loadData(fileIndex);
			Vars.difficulty = Vars.Difficulty.STANDARD;

			string level = Vars.currentNodeData.level;

			Vars.loadLevel(level);
			__uncollectOrb(PhysicalUpgrade.Orb.ORB0);
			__uncollectOrb(PhysicalUpgrade.Orb.ORB1);
			__uncollectOrb(PhysicalUpgrade.Orb.ORB2);
			__uncollectOrb(PhysicalUpgrade.Orb.ORB3);

			void __uncollectOrb(PhysicalUpgrade.Orb orb) {
				if (Vars.currentNodeData.orbCollected(orb)) Vars.currentNodeData.orbUndo(orb);
			}

			ChamberScreen.forceWaitOption = true;
		}

		public static void SkipTutorial() {
			VarsLoadData.loadBaseLandingData();
			Vars.eventHappen(AdventureEvent.Info.BASE_LANDING_TALK);
			Level.doNotStartBGMusic = true;
			Vars.restartFromLastSave();
		}
	}
}