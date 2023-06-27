using System;
using System.Linq;
using ItemRandomizer.Coordinator;
using ItemRandomizer.Resource;
using UnityEngine;
using static UnityEngine.Object;

namespace ItemRandomizer.PuzzleHelpers {
	public static class MusicPuzzle {

		public static void SceneChanges_Sail() {
			//Replace score sheet
			SpriteRenderer bars = GameObject.Find("song_panel").GetComponent<SpriteRenderer>();
			bars.sprite = Sprites.MusicBars;

			//Replace each of 4 letters with new solution
			{
				char[] sol = RandoState.Puzzle_MusicSolution.ToCharArray();
				float rBar = 18f / 64f;
				float noteCellWidth = (1 - rBar) / sol.Length;
				float barsLeftOffset = (-1f + noteCellWidth) / 2f + rBar;
				//             m----------|-----%----m----------|
				//             |          0     |               1
				//             -1/2             18/64
				float noteCellHeight = 4.5f / 37f;
				float barsBottomOffset = -0f / 2f;

				for (int i = 0; i < sol.Length; i++) {
					char solChar = sol[i];
					int noteIndex = solChar - 'a';
					float xOffset = (barsLeftOffset + i * noteCellWidth) * bars.sprite.bounds.size.x;
					float yOffset = (barsBottomOffset + _noteYOffset(solChar) * noteCellHeight) * bars.sprite.bounds.size.y;

					Sprites.NewSpriteRenderer($"puzzle_hint_note{solChar}", Sprites.MusicNotes[noteIndex], 6)
						.WithParent(bars.transform, new Vector3(xOffset, yOffset, 0))
						.Make();
				}
			}
		}

		private static float _noteYOffset(char note) {
			float notePos = 0;
			switch (note) {
				case 'a':
					notePos = -1;
					break;
				case 'c':
					notePos = 1;
					break;
				case 'd':
					notePos = 2;
					break;
				case 'e':
					notePos = 3;
					break;
				case 'f':
					notePos = -3;
					break;
				case 'g':
					notePos = -2;
					break;
				default:
					break;
			}

			return notePos;
		}

		public static string MakeAndSetNewSolution(System.Random rnd) {
			char[] chars = "abcdefg".ToCharArray();
			chars.Shuffle(rnd);

			string newSolution = "";

			int count = rnd.Next(2, 7);
			for (int i = 0; i < count; i++) {
				newSolution += chars[i];
			}

			SolatiaBossDoor.correctUncappedTotems = newSolution.Select(c => _GetEventForNote(c)).ToArray();

			return newSolution;
		}

		internal static void StickyNotes(StickyNotes sn) {
			Transform transform0 = sn.transform.Find("totem");

			SpriteRenderer[] totemNotes = transform0.GetComponentsInChildren<SpriteRenderer>();
			foreach (var item in totemNotes) {
				Destroy(item.gameObject);
			}

			for (int i = 0; i < RandoState.Puzzle_MusicSolution.Length; i++) {
				int letterIndex = RandoState.Puzzle_MusicSolution[i] - 'a';
				_newSticky(letterIndex, transform0);
			}

			totemNotes = transform0.GetComponentsInChildren<SpriteRenderer>();
			PrivateParts.SetPrivateField(sn, "totemNotes", totemNotes);
		}

		private static void _newSticky(int letterIndex, Transform parent) {
			//G goes on the bottom
			int adjSortOrder = ((letterIndex == 6 ? -1 : letterIndex) + 1) * 2 + 7;
			StickyNoteFactory.MakeNewStickyNote(Sprites.StickyNotes_MusicNotes[letterIndex], parent, _getOffset(letterIndex), $"TotemNote_{Convert.ToChar('a' + letterIndex)}", adjSortOrder);
		}

		private static Vector3 _getOffset(int index) => index switch {
			5 => new Vector3(0.93f, 1.6f, 0),
			4 => new Vector3(0.18f, 1.05f, 0),
			3 => new Vector3(0.96f, 0.36f, 0),
			2 => new Vector3(0.17f, -0.24f, 0),
			1 => new Vector3(0.93f, -0.86f, 0),
			0 => new Vector3(0.09f, -1.5f, 0),
			6 => new Vector3(0.94f, -2.06f, 0),
			_ => throw new System.Exception("Rip LabStickyOffsets")
		};

		private static AdventureEvent.Physical _GetEventForNote(char note) {
			AdventureEvent.Physical noteEvent = AdventureEvent.Physical.NONE;
			switch (note) {
				case 'a':
					noteEvent = AdventureEvent.Physical.TOTEM_A_UNCAPPED;
					break;
				case 'b':
					noteEvent = AdventureEvent.Physical.TOTEM_B_UNCAPPED;
					break;
				case 'c':
					noteEvent = AdventureEvent.Physical.TOTEM_C_UNCAPPED;
					break;
				case 'd':
					noteEvent = AdventureEvent.Physical.TOTEM_D_UNCAPPED;
					break;
				case 'e':
					noteEvent = AdventureEvent.Physical.TOTEM_E_UNCAPPED;
					break;
				case 'f':
					noteEvent = AdventureEvent.Physical.TOTEM_F_UNCAPPED;
					break;
				case 'g':
					noteEvent = AdventureEvent.Physical.TOTEM_G_UNCAPPED;
					break;
				default:
					Plugin.I.LogError($"What the crap?! TOTEM_UNCAPPED event not found for character `{note}`??");
					Plugin.I.LogError($"Rando's Music Puzzle Solution is: `{RandoState.Puzzle_MusicSolution}`");
					break;
			}

			return noteEvent;
		}
	}
}
