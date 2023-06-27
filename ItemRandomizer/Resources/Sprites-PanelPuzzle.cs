using System.Collections.Generic;
using UnityEngine;

namespace ItemRandomizer.Resource {
	public static partial class Sprites {
		public static readonly LazySprite PanelSmallHourglass = new LazySprite("Puzzles.Panel-Small-Hourglass.png");
		public static readonly LazySprite PanelBackground0 = new LazySprite("Puzzles.Panel-Three-Amigos.png", new Rect(0, 0, 74, 74));
		public static readonly LazySprite PanelBackground1 = new LazySprite("Puzzles.Panel-Three-Amigos.png", new Rect(96, 0, 74, 74));
		public static readonly LazySprite PanelBackground2 = new LazySprite("Puzzles.Panel-Three-Amigos.png", new Rect(192, 0, 74, 74));

		public static readonly LazySprite PanelBigDiamond0 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(000, 0, 52, 52));
		public static readonly LazySprite PanelBigDiamond1 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(052, 0, 52, 52));
		public static readonly LazySprite PanelBigDiamond2 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(104, 0, 52, 52));
		public static readonly LazySprite PanelBigDiamond3 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(156, 0, 52, 52));
		public static readonly LazySprite PanelBigDiamond4 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(208, 0, 52, 52));

		public static readonly LazySprite PanelBigSquare0 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(000, 52, 52, 52));
		public static readonly LazySprite PanelBigSquare1 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(052, 52, 52, 52));
		public static readonly LazySprite PanelBigSquare2 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(104, 52, 52, 52));
		public static readonly LazySprite PanelBigSquare3 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(156, 52, 52, 52));
		public static readonly LazySprite PanelBigSquare4 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(208, 52, 52, 52));

		public static readonly LazySprite PanelBigCircle0 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(000, 104, 52, 52));
		public static readonly LazySprite PanelBigCircle1 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(052, 104, 52, 52));
		public static readonly LazySprite PanelBigCircle2 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(104, 104, 52, 52));
		public static readonly LazySprite PanelBigCircle3 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(156, 104, 52, 52));
		public static readonly LazySprite PanelBigCircle4 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(208, 104, 52, 52));

		public static readonly LazySprite PanelBigTriangle0 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(000, 156, 52, 52));
		public static readonly LazySprite PanelBigTriangle1 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(052, 156, 52, 52));
		public static readonly LazySprite PanelBigTriangle2 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(104, 156, 52, 52));
		public static readonly LazySprite PanelBigTriangle3 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(156, 156, 52, 52));
		public static readonly LazySprite PanelBigTriangle4 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(208, 156, 52, 52));

		public static readonly LazySprite PanelBigDazzle0 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(000, 208, 52, 52));
		public static readonly LazySprite PanelBigDazzle1 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(052, 208, 52, 52));
		public static readonly LazySprite PanelBigDazzle2 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(104, 208, 52, 52));
		public static readonly LazySprite PanelBigDazzle3 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(156, 208, 52, 52));
		public static readonly LazySprite PanelBigDazzle4 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(208, 208, 52, 52));

		public static readonly LazySprite PanelBigHourglass0 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(000, 260, 52, 52));
		public static readonly LazySprite PanelBigHourglass1 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(052, 260, 52, 52));
		public static readonly LazySprite PanelBigHourglass2 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(104, 260, 52, 52));
		public static readonly LazySprite PanelBigHourglass3 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(156, 260, 52, 52));
		public static readonly LazySprite PanelBigHourglass4 = new LazySprite("Puzzles.Panel-Big-Shapes.png", new Rect(208, 260, 52, 52));

		public static readonly Manumation PanelDiamond = new Manumation(PanelBigDiamond0,
			new() {
				PanelBigDiamond1,
				PanelBigDiamond2,
				PanelBigDiamond3,
				PanelBigDiamond4,
				PanelBigDiamond3,
				PanelBigDiamond2,
			},
			0.1f,
			3
		);

		public static readonly Manumation PanelSquare = new Manumation(PanelBigSquare0,
			new() {
				PanelBigSquare1,
				PanelBigSquare2,
				PanelBigSquare3,
				PanelBigSquare4,
				PanelBigSquare3,
				PanelBigSquare2,
			},
			0.1f,
			3
		);

		public static readonly Manumation PanelCircle = new Manumation(PanelBigCircle0,
			new() {
				PanelBigCircle1,
				PanelBigCircle2,
				PanelBigCircle3,
				PanelBigCircle4,
				PanelBigCircle3,
				PanelBigCircle2,
			},
			0.1f,
			3
		);

		public static readonly Manumation PanelTriangle = new Manumation(PanelBigTriangle0,
			new() {
				PanelBigTriangle1,
				PanelBigTriangle2,
				PanelBigTriangle3,
				PanelBigTriangle4,
				PanelBigTriangle3,
				PanelBigTriangle2,
			},
			0.1f,
			3
		);

		public static readonly Manumation PanelDazzle = new Manumation(PanelBigDazzle0,
			new() {
				PanelBigDazzle1,
				PanelBigDazzle2,
				PanelBigDazzle3,
				PanelBigDazzle4,
				PanelBigDazzle3,
				PanelBigDazzle2,
			},
			0.1f,
			3
		);

		public static readonly Manumation PanelHourglass = new Manumation(PanelBigHourglass0,
			new() {
				PanelBigHourglass1,
				PanelBigHourglass2,
				PanelBigHourglass3,
				PanelBigHourglass4,
				PanelBigHourglass3,
				PanelBigHourglass2,
			},
			0.1f,
			3
		);

		public static readonly List<Manumation> PanelPuzzle = new List<Manumation> { PanelDiamond, PanelSquare, PanelCircle, PanelTriangle, PanelDazzle, PanelHourglass };
	}
}
