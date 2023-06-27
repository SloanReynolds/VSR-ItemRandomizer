using System;
using System.Collections.Generic;
using System.Linq;
using ItemRandomizer.Coordinator;
using ItemRandomizer.Resource;
using UnityEngine;
using static UnityEngine.Object;

namespace ItemRandomizer.PuzzleHelpers {
	public static class PanelPuzzle {

		public static void SceneChanges_SolutionRoom() {
			//Object Creation
			GameObject panelSol0 = GameObject.Find("panel_solution_1");
			GameObject panelSol1 = GameObject.Find("panel_solution_2");
			GameObject panelSol2 = GameObject.Find("panel_solution_3");

			//panelSol2.transform.localPosition = panelSol1.transform.localPosition + (panelSol2.transform.localPosition - panelSol1.transform.localPosition) / 3;

			Destroy(panelSol0.GetComponent<Animator>());
			Destroy(panelSol1.GetComponent<Animator>());
			Destroy(panelSol2.GetComponent<Animator>());

			GameObject background0 = Instantiate(panelSol0, panelSol0.transform);
			Destroy(background0.GetComponent<TimeUser>());
			GameObject background1 = Instantiate(background0, panelSol1.transform);
			GameObject background2 = Instantiate(background0, panelSol2.transform);

			//The rest of the owl
			Manumator panelMan0 = panelSol0.AddComponent<Manumator>();
			Manumator panelMan1 = panelSol1.AddComponent<Manumator>();
			Manumator panelMan2 = panelSol2.AddComponent<Manumator>();

			panelMan0.Manumation = Sprites.PanelPuzzle[RandoState.Puzzle_PanelSolution[0]];
			panelMan1.Manumation = Sprites.PanelPuzzle[RandoState.Puzzle_PanelSolution[1]];
			panelMan2.Manumation = Sprites.PanelPuzzle[RandoState.Puzzle_PanelSolution[2]];

			background0.GetComponent<SpriteRenderer>().sprite = Sprites.PanelBackground0;
			background1.GetComponent<SpriteRenderer>().sprite = Sprites.PanelBackground1;
			background2.GetComponent<SpriteRenderer>().sprite = Sprites.PanelBackground2;

			background0.transform.localPosition = new Vector3(0, 0, 0);
			background1.transform.localPosition = new Vector3(0, 0, 0);
			background2.transform.localPosition = new Vector3(0, 0, 0);
		}

		public static int[] MakeAndSetNewSolution(System.Random rnd) {
			int[] newSolution = new int[3] { rnd.Next(0, Sprites.PanelPuzzle.Count), rnd.Next(0, Sprites.PanelPuzzle.Count), rnd.Next(0, Sprites.PanelPuzzle.Count) };
			PuzzlePanelSolver.solution = newSolution;
			return newSolution;
		}

		public static void SceneChanges_PuzzleRoom() {
			Panel.Selection newSelection = new Panel.Selection();
			newSelection.name = "hourglass";
			newSelection.sprite = Sprites.PanelSmallHourglass;

			//Add Hourglass Shape
			foreach (Panel panel in Resources.FindObjectsOfTypeAll<Panel>()) {
				List<Panel.Selection> list = panel.selections.ToList();
				list.Add(newSelection);
				panel.selections = list.ToArray();
			}
		}

		public static void StickyNotes(StickyNotes sn) {
			Transform transform0 = sn.transform.Find("panel");

			SpriteRenderer[] panelNotes = transform0.GetComponentsInChildren<SpriteRenderer>();
			foreach (var item in panelNotes) {
				Destroy(item.gameObject);
			}

			_newSticky(0, transform0);
			_newSticky(1, transform0);
			_newSticky(2, transform0);

			panelNotes = transform0.GetComponentsInChildren<SpriteRenderer>();
			PrivateParts.SetPrivateField(sn, "panelNotes", panelNotes);
		}

		private static void _newSticky(int index, Transform parent) {
			StickyNoteFactory.MakeNewStickyNote(Sprites.StickyNotes_Shapes[PuzzlePanelSolver.solution[index]], parent, _getOffset(index), $"PanelNote{index}");
		}

		private static Vector3 _getOffset(int index) => index switch {
			0 => new Vector3(1.22f, 0.85f, 0),
			1 => new Vector3(2.98f, 0.67f, 0),
			2 => new Vector3(4.52f, 0.75f, 0),
			_ => throw new System.Exception("Rip PanelStickyOffsets")
		};
	}
}
