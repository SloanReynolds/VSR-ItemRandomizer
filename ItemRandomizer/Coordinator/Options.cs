using System;
using System.Collections.Generic;
using System.Linq;

namespace ItemRandomizer.Coordinator {
	public static class Options { //Class Data
		private static readonly Dictionary<string, Func<bool>> _options = new() {
			{ "opt_randomized_puzzles", () => Configs.RandomizePuzzles }
		};

		public static bool TryGetOption(string key, out bool val) {
			if (_options.ContainsKey(key)) {
				val = _options[key].Invoke();
				return true;
			} else {
				Plugin.I.LogError($"Logic Option `{key}` unrecognized!");
			}

			val = false;
			return false;
		}
	}
}