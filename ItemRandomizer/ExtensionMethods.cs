using System;
using System.Collections.Generic;
using ItemRandomizer.Coordinator;

static class ExtensionMethods {
	public static void Shuffle<T>(this IList<T> list, Random rnd) {
		int n = list.Count;
		while (n > 1) {
			int k = rnd.Next(0, n);
			n--;
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}

	public static float getLineHeight(this GlyphBox gb) {
		GlyphSprite glyphSprite = ((List<List<GlyphSprite>>)PrivateParts.GetPrivateField(gb, "glyphs"))[0][0];
		return glyphSprite.pixelHeight;
	}

	public static Queue<T> ToQueue<T>(this IEnumerable<T> collection) {
		return new Queue<T>(collection);
	}
}