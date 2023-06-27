using ItemRandomizer.Coordinator;
using ItemRandomizer.PuzzleHelpers;
using ItemRandomizer.Resource;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ItemRandomizer {
	class PuzzlesManager : MonoBehaviour {
		void Awake() {
			SceneManager.sceneLoaded += this._SceneManager_sceneLoaded;
		}

		private void _SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode) {
			_Clean();

			if (scene.name == "base_landing") {
				GameObject.Find("table").GetComponent<SpriteRenderer>().sortingOrder = 60;
				GameObject.Find("Wally/spriteObject").GetComponent<SpriteRenderer>().sortingOrder = 61;

				//Seed Hash
				GameObject go = StickyNoteFactory.SeedHashCollection();
				go.transform.localPosition = new Vector2(81f, 25.3f);
				go.SetActive(true);

				if (!Configs.RandomizePuzzles) return;
				//Puzzle Stickies
				StickyNotes sn = GameObject.FindObjectOfType<StickyNotes>();

				PanelPuzzle.StickyNotes(sn);
				LabPuzzle.StickyNotes(sn);
				MusicPuzzle.StickyNotes(sn);
				return;
			}

			if (!Configs.RandomizePuzzles) return;

			{//Panel Puzzle
				if (scene.name == "panel_puzzle") {
					PanelPuzzle.SceneChanges_PuzzleRoom();
					return;
				}

				if (scene.name == "panel_solution") {
					PanelPuzzle.SceneChanges_SolutionRoom();
					return;
				}
			}

			{//Griger Lab Panel Puzzle
				if (scene.name == "griger_clue_1") {
					LabPuzzle.SceneChanges_Clue1();
					return;
				}
				if (scene.name == "griger_clue_2") {
					LabPuzzle.SceneChanges_Clue2();
					return;
				}
				if (scene.name == "griger_clue_3") {
					LabPuzzle.SceneChanges_Clue3();
					return;
				}
				if (scene.name == "griger_clue_4") {
					LabPuzzle.SceneChanges_Clue4();
					return;
				}
				if (scene.name == "griger_panel_puzzle") {
					LabPuzzle.SceneChanges_PanelRoom();
					return;
				}
			}

			{//Music Puzzle?
				if (scene.name == "music_floatlands_sail") {
					MusicPuzzle.SceneChanges_Sail();
					return;
				}
			}
		}

		private void _Clean() {
			//throw new NotImplementedException();
		}
	}
}
