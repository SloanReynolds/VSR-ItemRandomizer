using UnityEngine;

namespace ItemRandomizer.Resource {
	public static partial class Sprites {
		public static readonly LazySprite LabCornerBL = new LazySprite("Puzzles.Lab-Clues.png", new Rect(0, 0, 24, 24));
		public static readonly LazySprite LabCornerBR = new LazySprite("Puzzles.Lab-Clues.png", new Rect(24, 0, 24, 24));
		public static readonly LazySprite LabCornerTL = new LazySprite("Puzzles.Lab-Clues.png", new Rect(48, 0, 24, 24));
		public static readonly LazySprite LabCornerTR = new LazySprite("Puzzles.Lab-Clues.png", new Rect(72, 0, 24, 24));

		public static readonly LazySprite LabNumber1 = new LazySprite("Puzzles.Lab-Clues.png", new Rect(0, 24, 24, 24));
		public static readonly LazySprite LabNumber2 = new LazySprite("Puzzles.Lab-Clues.png", new Rect(24, 24, 24, 24));
		public static readonly LazySprite LabNumber3 = new LazySprite("Puzzles.Lab-Clues.png", new Rect(48, 24, 24, 24));
		public static readonly LazySprite LabNumber4 = new LazySprite("Puzzles.Lab-Clues.png", new Rect(72, 24, 24, 24));

		public static readonly LazySprite LabNumber5 = new LazySprite("Puzzles.Lab-Clues.png", new Rect(0, 48, 24, 24));
		public static readonly LazySprite LabNumber6 = new LazySprite("Puzzles.Lab-Clues.png", new Rect(24, 48, 24, 24));
		public static readonly LazySprite LabNumber7 = new LazySprite("Puzzles.Lab-Clues.png", new Rect(48, 48, 24, 24));
		public static readonly LazySprite LabNumber8 = new LazySprite("Puzzles.Lab-Clues.png", new Rect(72, 48, 24, 24));

		public static readonly LazySprite LabNumber9 = new LazySprite("Puzzles.Lab-Clues.png", new Rect(0, 72, 24, 24));
		public static readonly LazySprite LabNumber0 = new LazySprite("Puzzles.Lab-Clues.png", new Rect(24, 72, 24, 24));

		public static readonly LazySprite[] LabCorners = new[] { LabCornerBL, LabCornerBR, LabCornerTL, LabCornerTR };
		public static readonly LazySprite[] LabNumbers = new[] { LabNumber0, LabNumber1, LabNumber2, LabNumber3, LabNumber4, LabNumber5, LabNumber6, LabNumber7, LabNumber8, LabNumber9 };
	}
}


