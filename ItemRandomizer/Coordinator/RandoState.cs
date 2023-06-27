using System.IO;
using ItemRandomizer.PuzzleHelpers;
using Newtonsoft.Json;
using Random = System.Random;

namespace ItemRandomizer.Coordinator {
	public static class RandoState {
		public static int[] Puzzle_PanelSolution { get; internal set; }
		public static string Puzzle_LabSolution { get; internal set; } = "2973";
		public static string Puzzle_MusicSolution { get; internal set; } = "aceg";

		public static void Reset() {
			LabPuzzle.Reset();
		}

		private static Locations _locations;
		public static Locations Locations => _locations ??= Locations.Initial;

		private static Macros _macros;
		public static Macros Macros {
			get {
				if (_macros == null) {
					_InitJsonMacros();
				}
				return _macros;
			}
		}

		private static uint _seed = 0;
		public static uint Seed {
			get {
				if (_seed == 0) {
					Random rnd = new Random();
					_seed = (uint)rnd.Next();
				}
				return _seed;
			}
			set {
				_seed = value;
			}
		}

		private static void _InitJsonMacros() {
			using (StreamReader sr = new StreamReader(typeof(Plugin).Assembly.GetManifestResourceStream("ItemRandomizer.Data.Logic.macros.json"))) {
				string json = sr.ReadToEnd();
				_macros = JsonConvert.DeserializeObject<Macros>(json);
			}

			//_macros.ForEach(m => Console.WriteLine(m));
		}
	}
}