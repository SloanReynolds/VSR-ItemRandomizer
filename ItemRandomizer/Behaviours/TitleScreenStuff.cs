using System;
using System.Collections.Generic;
using System.Linq;
using ItemRandomizer.Coordinator;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ItemRandomizer {

	public class TitleScreenStuff : MonoBehaviour {
		private GlyphBox _playBox = null;
		private DapperEditTextBox _setSeedTextBox = null;
		private DapperBox[] _allTextBoxes = new DapperBox[] { };
		private const string _F1_TEXT = "F1 - Randomizer: {0}";
		private const string _F2_TEXT = "F2 - Username: {0}";
		private const string _F3_TEXT = "F3 - Randomize Puzzles: {0}";
		private const string _F5_TEXT = "F5 - Edit Seed";
		private const string _F6_TEXT = "F6 - Random Seed";

		void Awake() {
			TitleAnimation ta = Resources.FindObjectsOfTypeAll<TitleAnimation>().First();
			ta.gameObject.SetActive(true);
		}

		void Start() {
			DapperBox.Reset();

			_playBox = GameObject.Find("PlayGameText").GetComponent<GlyphBox>();

			GameObject companyBox = GameObject.Find("CompanyBox");
			GlyphBox _newBox = Instantiate(companyBox.GetComponent<GlyphBox>(), companyBox.transform.parent);
			DapperBox.Reference = _newBox;
			_newBox.gameObject.SetActive(false);

			_allTextBoxes = new DapperBox[] {
				new DapperActionBox(_F1_TEXT, KeyCode.F1, Configs.EnabledVerbiage, _ActionF1).CanBypassEnabled(),			//Randomizer Enable
				new DapperEditTextBox(_F2_TEXT, KeyCode.F2, Configs.Username).WithEditDone(_OnEditDoneF2),							//Username
				new DapperActionBox(_F3_TEXT, KeyCode.F3, Configs.PuzzlesVerbiage, _ActionF3),			//Randomize Puzzles
				new DapperEditTextBox(_F5_TEXT, KeyCode.F5, $"{RandoState.Seed}", _NewSeedTextF5, 10, true).WithEditDone(_OnEditDoneF5),	//Edit Seed
				new DapperActionBox(_F6_TEXT, KeyCode.F6, "", _ActionF6),								//Random Seed
			};
			_setSeedTextBox = (DapperEditTextBox)_allTextBoxes[3];

			_RandomizeWithSeed(RandoState.Seed);
		}

		private void _OnEditDoneF2(string newValue) {
			Configs.Username = newValue;
			Configs.SaveToFile();
		}

		private void _ActionF1(DapperActionBox caller) {
			_setSeedTextBox.EditMode = false;
			Configs.Enabled = !Configs.Enabled;
			foreach (var item in _allTextBoxes) {
				item.Enabled = Configs.Enabled;
			}
			caller.UpdateText(Configs.EnabledVerbiage);
			if (!Configs.Enabled) {
				_NewSeedTextF5("Play Game");
			} else {
				_RandomizeWithSeedTextBox();
			}
		}

		private void _ActionF3(DapperActionBox caller) {
			Configs.RandomizePuzzles = !Configs.RandomizePuzzles;
			caller.UpdateText(Configs.PuzzlesVerbiage);
		}

		private void _NewSeedTextF5(string newText) {
			_playBox.setText(newText);
			TitleScreen ts = GetComponent<TitleScreen>();
			List<GlyphBox> list = (List<GlyphBox>)PrivateParts.GetPrivateField(ts, "options");
			int selectedIndex = (int)PrivateParts.GetPrivateField(ts, "selectionIndex");
			if (list[selectedIndex] == _playBox) {
				_playBox.setColor(PauseScreen.SELECTED_COLOR);
			} else {
				_playBox.setColor(PauseScreen.DEFAULT_COLOR);
			}
		}

		private void _OnEditDoneF5(string newValue) {
			if (Configs.Enabled) {
				_RandomizeWithSeedTextBox();
			}
		}

		private void _ActionF6(DapperActionBox caller) {
			_RandomSeed();
		}

		public void HideOptions() {
			foreach (DapperBox box in _allTextBoxes) {
				box.hide();
			}
		}

		public void ShowOptions() {
			foreach (DapperBox box in _allTextBoxes) {
				box.Show();
			}
		}

		private void _RandomizeWithSeedTextBox() {
			if (uint.TryParse(_setSeedTextBox.Current, out uint parsed)) {
				_RandomizeWithSeed(parsed == 0 ? (uint)new System.Random().Next() : parsed);
			} else {
				Plugin.I.LogWarning($"Rando Seed Value `{_setSeedTextBox.Current}` wasn't parsed correctly- skipping randomization.");
			}
		}

		private void _RandomizeWithSeed(uint seed) {
			RandoState.Seed = seed;
			_setSeedTextBox.EditMode = false;
			_setSeedTextBox.Current = RandoState.Seed.ToString();
			RandoBrain.Randomize();
		}

		private void _RandomSeed() {
			uint newSeed = (uint)new System.Random().Next();
			_RandomizeWithSeed(newSeed);
		}

		void Update() {
			if (SceneManager.GetActiveScene().name != "title_scene") return;

			foreach (DapperBox box in _allTextBoxes) {
				box.DapperBoxUpdate();
			}
		}
	}
}
