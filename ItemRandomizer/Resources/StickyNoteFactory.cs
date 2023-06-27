using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ItemRandomizer.Resource {
	public static class StickyNoteFactory {
		public static GameObject MakeNewStickyNote(LazySprite sprite, Transform parent, Vector3 offset, string noteName = "StickyNote", int sortOrder = 7, params LazySprite[] additionalWritingLayers) {
			GameObject go = new GameObject(noteName);

			//SpriteRenderer bgSR = 
			Sprites.NewSpriteRenderer("sticky_note_bg", Sprites.StickyNotes_Note[RandoBrain.RndNext("sticky_notes", 4)], sortOrder)
				.WithParent(go.transform, offset)
				.Make();

			string writingName = "sticky_note_writing" + (additionalWritingLayers.Length > 0 ? "_0" : "");

			SpriteRenderer writingBase = Sprites.NewSpriteRenderer(writingName, sprite, sortOrder + 1)
				.WithParent(go.transform, offset)
				.Make();

			if (additionalWritingLayers != null && additionalWritingLayers.Length > 0) {
				MakeCompositeWriting(writingBase, additionalWritingLayers);
			}

			go.transform.SetParent(parent, false);

			return go;
		}

		public static GameObject MakeCompositeWriting(SpriteRenderer firstLayer, params LazySprite[] spriteLayers) {
			if (spriteLayers.Length < 1) {
				throw new System.Exception("Can't make composite sticky writing with no layers assigned!!");
			}
			GameObject go = firstLayer.gameObject;

			int sortOrder = firstLayer.sortingOrder;
			for (int i = 0; i < spriteLayers.Length; i++) {
				LazySprite layer = spriteLayers[i];
				Sprites.NewSpriteRenderer($"sticky_note_writing_{i+1}", layer, ++sortOrder)
					.WithParent(go.transform)
					.Make();
			}

			go.transform.SetParent(firstLayer.transform, false);
			return go;
		}

		public static GameObject SeedHashCollection() {
			RandoBrain.RndReset("seed_hash");

			GameObject go = new GameObject("SeedHashStickies");

			Vector2 cellSize = new Vector2(1.6f, 1.35f);
			int maxX = 4;
			int maxY = 3;

			for (int i = 0; i < maxX; i++) {
				for (int j = 0; j < maxY; j++) {
					float x = i - (maxX / 2f);
					float y = j - (maxY / 2f);
					int ind = j * i + j;
					GameObject newGo = _createSeedHash(ind);
					newGo.transform.SetParent(go.transform);
					newGo.transform.localPosition = new Vector2(
						(x + RandoBrain.RndNext("seed_hash", 0.5f)) * cellSize.x,
						(y + RandoBrain.RndNext("seed_hash", 0.5f)) * cellSize.y);
				}
			}

			return go;
		}

		private static GameObject _createSeedHash(int i) {
			GameObject go = new GameObject("StickyNote");

			//SpriteRenderer bgSR = 
			Sprites.NewSpriteRenderer("sticky_note_bg", Sprites.StickyNotes_Note[RandoBrain.RndNext("seed_hash", 4)], 6 + i * 2)
				.WithParent(go.transform)
				.Make();

			//SpriteRenderer writingSR = 
			Sprites.NewSpriteRenderer("sticky_note_writing", Sprites.StickyNotes_FunStuff[RandoBrain.RndNext("seed_hash", Sprites.StickyNotes_FunStuff.Count)], 7 + i * 2)
				.WithParent(go.transform)
				.Make();

			return go;
		}
	}
}
