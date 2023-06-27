using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ItemRandomizer.Coordinator;
using ItemRandomizer.PuzzleHelpers;

namespace ItemRandomizer {
	public static class RandoBrain {
		private const int CIRCUIT_LIMIT = 5000;

		private static Random Rnd;
		private static Dictionary<string, Random> _hashedRnds;

		public static uint CurrentlyRandomizedSeed = 0;

		public static void Randomize() {
			if (CurrentlyRandomizedSeed == RandoState.Seed) return;

			int seed = (int)RandoState.Seed;
			int bestCount = 0;
			int bestSeed = 0;
			bool circuitbreak = false;
			bool testPassed;

			Rnd = new Random(seed);
			_hashedRnds = new Dictionary<string, Random>();
			GameState.ResetGame();

			int seedCount = 0;
			Stopwatch sw = new();
			sw.Start();
			do {
				seedCount++;

				if (Configs.RandomizePuzzles) {
					RandoState.Puzzle_PanelSolution = PanelPuzzle.MakeAndSetNewSolution(Rnd);
					RandoState.Puzzle_LabSolution = LabPuzzle.MakeAndSetNewSolution(Rnd);
					RandoState.Puzzle_MusicSolution = MusicPuzzle.MakeAndSetNewSolution(Rnd);
				}

				//Randomly shuffle all the items. Naive; Easy; Incredibly Fast. //Actually probably not incredibly fast in this case? But uh... you know.
				_BogotizeMeCaptain();

				//Test it, if it's bad logic, do it again.
				testPassed = _LogicTest(out int reachable);

				//Circuitbreaker if seedgen takes waaay too long
				if (circuitbreak && seedCount == bestSeed) break;

				if (reachable > bestCount) {
					bestCount = reachable;
					bestSeed = seedCount;
				}

				if (seedCount >= CIRCUIT_LIMIT) {
					circuitbreak = true;
					Rnd = new Random(seed);
					seedCount = 0;
				}
			} while (testPassed == false);
			sw.Stop();
			Plugin.I.LogInfo($"{(circuitbreak ? "Circuitbroken" : "Successful")} rando (seed {seed}) found after {seedCount} attempts! Took {sw.ElapsedMilliseconds}ms.");

			//Plugin.I.LogWarning(RandoState.Puzzle_MusicSolution);
			CurrentlyRandomizedSeed = RandoState.Seed;
		}

		public static void RndReset(string name) {
			if (_hashedRnds.ContainsKey(name)) _hashedRnds.Remove(name);
		}

		public static int RndNext(string name, int maxVal = int.MaxValue) {
			if (!_hashedRnds.ContainsKey(name)) _hashedRnds.Add(name, new Random((int)RandoState.Seed ^ name.GetHashCode()));

			Random rnd = _hashedRnds[name];
			if (maxVal == int.MaxValue) {
				return rnd.Next();
			}

			return rnd.Next(maxVal);
		}

		public static float RndNext(string name, float maxVal = float.MaxValue) {
			if (!_hashedRnds.ContainsKey(name)) _hashedRnds.Add(name, new Random((int)RandoState.Seed ^ name.GetHashCode()));

			Random rnd = _hashedRnds[name];
			if (maxVal == float.MaxValue) {
				return (float)rnd.NextDouble();
			}

			return (float)rnd.NextDouble() * maxVal;
		}

		private static void _BogotizeMeCaptain() {
			Locations locations = RandoState.Locations;

			Item[] items = RandoState.Locations.Select(l => l.OriginalItem).ToArray();

			items.Shuffle(Rnd);

			for (int i = 0; i < locations.Count; i++) {
				locations[i].SetItem(items[i]);
			}
		}

		private static bool _LogicTest(out int reachable) {
			bool hasChanged = false;
			List<Item> currentItems = new();

			do {
				hasChanged = false;
				List<Item> newItems = new();
				//Plugin.I.LogInfo($"LogicTest Current Items ({currentItems.Count}):");
				//Plugin.I.LogInfo(string.Join(", ", currentItems.Where(it => it is Item.Decrypt)));
				foreach (Location location in Coordinator.RandoState.Locations) {
					if (location.Logic.Evaluate(currentItems)) {
						if (!currentItems.Contains(location.CurrentItem)) {
							//Plugin.I.LogInfo($" {location.Scene}/{location.OriginalItem.IDStr} holds {location.CurrentItem.IDStr}");
							//Plugin.I.LogInfo($"  New Item!");
							newItems.Add(location.CurrentItem);
							hasChanged = true;
						}
					}
				}

				//Plugin.I.LogInfo($" newItems this loop: {newItems.Count}");
				currentItems.AddRange(newItems);

				//Plugin.I.LogInfo("foundCount " + currentItems.Count);
			} while (hasChanged);

			reachable = currentItems.Count;

			//Plugin.I.LogInfo($"Count: {currentItems.Count}/{Coordinator.Rando.Locations.Count}");
			if (currentItems.Count != Coordinator.RandoState.Locations.Count) {
				return false;
			}

			return true;
		}
	}
}
