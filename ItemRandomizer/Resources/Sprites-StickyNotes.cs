using UnityEngine;
using System.Collections.Generic;

namespace ItemRandomizer.Resource {
	public static partial class Sprites {
		public static readonly List<LazySprite> StickyNotes_Note = new List<LazySprite>() {
			new LazySprite("Puzzles.StickyNotes.png", new Rect(0, 0, 22, 22)),
			new LazySprite("Puzzles.StickyNotes.png", new Rect(22, 0, 22, 22)),
			new LazySprite("Puzzles.StickyNotes.png", new Rect(44, 0, 22, 22)),
			new LazySprite("Puzzles.StickyNotes.png", new Rect(66, 0, 22, 22)),
		};

		public static readonly List<LazySprite> StickyNotes_Shapes = new List<LazySprite>() {
			new LazySprite("Puzzles.StickyNotes.png", new Rect(88, 0, 22, 22)),						//diamond
			new LazySprite("Puzzles.StickyNotes.png", new Rect(110, 0, 22, 22)),					//square
			new LazySprite("Puzzles.StickyNotes.png", new Rect(132, 0, 22, 22)),					//circle
			new LazySprite("Puzzles.StickyNotes.png", new Rect(154, 0, 22, 22)),					//triangle
			new LazySprite("Puzzles.StickyNotes.png", new Rect(154, 22, 22, 22)),					//dazzle
			new LazySprite("Puzzles.StickyNotes.png", new Rect(154, 66, 22, 22)),					//hourglass
		};

		public static readonly List<LazySprite> StickyNotes_Numbers = new List<LazySprite>() {
			new LazySprite("Puzzles.StickyNotes.png", new Rect(0, 110, 22, 22)),					//0
			new LazySprite("Puzzles.StickyNotes.png", new Rect(88, 44, 22, 22)),					//1
			new LazySprite("Puzzles.StickyNotes.png", new Rect(110, 44, 22, 22)),					//2
			new LazySprite("Puzzles.StickyNotes.png", new Rect(132, 44, 22, 22)),					//3
			new LazySprite("Puzzles.StickyNotes.png", new Rect(154, 44, 22, 22)),					//etc
			new LazySprite("Puzzles.StickyNotes.png", new Rect(0, 66, 22, 22)),
			new LazySprite("Puzzles.StickyNotes.png", new Rect(22, 66, 22, 22)),
			new LazySprite("Puzzles.StickyNotes.png", new Rect(44, 66, 22, 22)),
			new LazySprite("Puzzles.StickyNotes.png", new Rect(66, 66, 22, 22)),
			new LazySprite("Puzzles.StickyNotes.png", new Rect(88, 66, 22, 22)),
		};

		public static readonly List<LazySprite> StickyNotes_NumberCorners = new List<LazySprite>() {
			new LazySprite("Puzzles.StickyNotes.png", new Rect(0, 44, 22, 22)),						//TL
			new LazySprite("Puzzles.StickyNotes.png", new Rect(44, 44, 22, 22)),					//TR
			new LazySprite("Puzzles.StickyNotes.png", new Rect(22, 44, 22, 22)),					//BL
			new LazySprite("Puzzles.StickyNotes.png", new Rect(66, 44, 22, 22)),					//BR
		};

		public static readonly List<LazySprite> StickyNotes_MusicNotes = new List<LazySprite>() {
			new LazySprite("Puzzles.StickyNotes.png", new Rect(0, 22, 22, 22)),						//A
			new LazySprite("Puzzles.StickyNotes.png", new Rect(22, 22, 22, 22)),					//B
			new LazySprite("Puzzles.StickyNotes.png", new Rect(44, 22, 22, 22)),					//C
			new LazySprite("Puzzles.StickyNotes.png", new Rect(66, 22, 22, 22)),					//etc
			new LazySprite("Puzzles.StickyNotes.png", new Rect(88, 22, 22, 22)),
			new LazySprite("Puzzles.StickyNotes.png", new Rect(110, 22, 22, 22)),
			new LazySprite("Puzzles.StickyNotes.png", new Rect(132, 22, 22, 22)),
		};

		public static readonly List<LazySprite> StickyNotes_FunStuff = new List<LazySprite>() {
			new LazySprite("Puzzles.StickyNotes.png", new Rect(110, 66, 22, 22)),					//E-Tank
			new LazySprite("Puzzles.StickyNotes.png", new Rect(132, 66, 22, 22)),					//Bomb
			new LazySprite("Puzzles.StickyNotes.png", new Rect(0, 88, 22, 22)),						//Gato
			new LazySprite("Puzzles.StickyNotes.png", new Rect(22, 88, 22, 22)),					//Knight
			new LazySprite("Puzzles.StickyNotes.png", new Rect(44, 88, 22, 22)),					//Maxim
			new LazySprite("Puzzles.StickyNotes.png", new Rect(66, 88, 22, 22)),					//Pokeball
			new LazySprite("Puzzles.StickyNotes.png", new Rect(88, 88, 22, 22)),					//Triforce
			new LazySprite("Puzzles.StickyNotes.png", new Rect(110, 88, 22, 22)),					//Junimo
			new LazySprite("Puzzles.StickyNotes.png", new Rect(132, 88, 22, 22)),					//Black Mage
			new LazySprite("Puzzles.StickyNotes.png", new Rect(154, 88, 22, 22)),					//Lambda
		};
	}
}
