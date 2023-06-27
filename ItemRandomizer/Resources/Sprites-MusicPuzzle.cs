using UnityEngine;

namespace ItemRandomizer.Resource {
	public static partial class Sprites {
		public static readonly LazySprite MusicBars = new LazySprite("Puzzles.Totems-Music.png", new Rect(0, 0, 64, 33));

		public static readonly LazySprite MusicNoteA = new LazySprite("Puzzles.Totems-Music.png", new Rect(0, 33, 9, 9));
		public static readonly LazySprite MusicNoteB = new LazySprite("Puzzles.Totems-Music.png", new Rect(9, 33, 9, 9));
		public static readonly LazySprite MusicNoteC = new LazySprite("Puzzles.Totems-Music.png", new Rect(18, 33, 9, 9));
		public static readonly LazySprite MusicNoteD = new LazySprite("Puzzles.Totems-Music.png", new Rect(27, 33, 9, 9));
		public static readonly LazySprite MusicNoteE = new LazySprite("Puzzles.Totems-Music.png", new Rect(36, 33, 9, 9));
		public static readonly LazySprite MusicNoteF = new LazySprite("Puzzles.Totems-Music.png", new Rect(45, 33, 9, 9));
		public static readonly LazySprite MusicNoteG = new LazySprite("Puzzles.Totems-Music.png", new Rect(54, 33, 9, 9));

		public static readonly LazySprite[] MusicNotes = new[] { MusicNoteA, MusicNoteB, MusicNoteC, MusicNoteD, MusicNoteE, MusicNoteF, MusicNoteG };
	}
}
