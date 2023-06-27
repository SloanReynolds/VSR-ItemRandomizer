using System;
using System.Linq;
using ItemRandomizer.Coordinator;
using ItemRandomizer.Resource;
using UnityEngine;
using static UnityEngine.Object;

namespace ItemRandomizer.PuzzleHelpers {
	public static class LabPuzzle {
		private static readonly int[] _RoomOrder = new[] { 0, 1, 2, 3 };

		private static int[] _passcode = null;
		private static int[] _Passcode => _passcode ??= RandoState.Puzzle_LabSolution.Select(c => c - '0').ToArray();


		public static void Reset() {
			_passcode = null;
		}

		public static void SceneChanges_Clue1() {
			_replaceLabClue(0, "griger_clue_1 (1)");
		}
		public static void SceneChanges_Clue2() {
			_replaceLabClue(1, "clue");
		}
		public static void SceneChanges_Clue3() {
			_replaceLabClue(2, "clue");
		}
		public static void SceneChanges_Clue4() {
			_replaceLabClue(3, "clue");
		}

		public static void SceneChanges_PanelRoom() {
			GrigerPanelMonitor go = GameObject.FindObjectOfType<GrigerPanelMonitor>();
			go.correctPasscode = RandoState.Puzzle_LabSolution;
		}

		internal static string MakeAndSetNewSolution(System.Random rnd) {
			//Game's natural solution isn't static for the LabPuzzle...
			_RoomOrder.Shuffle(rnd);
			return rnd.Next(0, 10000).ToString().PadLeft(4, '0');
		}

		internal static void StickyNotes(StickyNotes sn) {
			Transform transform0 = sn.transform.Find("griger");

			SpriteRenderer[] grigerNotes = transform0.GetComponentsInChildren<SpriteRenderer>();
			foreach (var item in grigerNotes) {
				Destroy(item.gameObject);
			}

			//Console.WriteLine(RandoState.Puzzle_LabSolution);

			_newSticky(0, transform0);
			_newSticky(1, transform0);
			_newSticky(2, transform0);
			_newSticky(3, transform0);

			grigerNotes = transform0.GetComponentsInChildren<SpriteRenderer>();
			PrivateParts.SetPrivateField(sn, "grigerNotes", grigerNotes);
		}

		private static void _newSticky(int index, Transform parent) {
			Vector3 offset = _getOffset(index);
			//I see: TR BR TL BL
			//I need 2 0 3 1
			StickyNoteFactory.MakeNewStickyNote(Sprites.StickyNotes_NumberCorners[index], parent, offset, $"LabNote{index}", 7, Sprites.StickyNotes_Numbers[_Passcode[index]]);
		}

		private static Vector3 _getOffset(int index) => index switch {
			0 => new Vector3(1.029999f, 2.17f, 0),
			1 => new Vector3(2.229999f, 2.17f, 0),
			2 => new Vector3(1.019997f, 0.9299991f, 0),
			3 => new Vector3(2.269998f, 1.0f, 0),
			_ => throw new System.Exception("Rip LabStickyOffsets")
		};

		private static void _replaceLabClue(int roomIndex, string goName) {
			int room = _RoomOrder[roomIndex];
			//Corner Piece
			SpriteRenderer sr = GameObject.Find(goName).GetComponent<SpriteRenderer>();
			sr.sprite = Sprites.LabCorners[room];
			int dY = -1;
			if (room < 2) {
				dY = 1;
			}

			//Number Piece
			//SpriteRenderer sr2 =
			Sprites.NewSpriteRenderer("clue_part_number", Sprites.LabNumbers[_Passcode[room]], 3)
				.WithParent(sr.transform, new Vector3(0, dY * 0.08f, 0))
				.Make();

			float newZ = (roomIndex * 73f + room * 36f + _Passcode[room] * 153f) / 4f;
			sr.transform.localEulerAngles += new Vector3(0, 0, newZ);
		}
	}
}
