using System;
using System.Collections.Generic;
using ItemRandomizer.Resource;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace ItemRandomizer {
	public class Manumator : MonoBehaviour {
		public Manumation Manumation { get; set; }

		private int _currentIndex = -1;

		private SpriteRenderer sr = null;

		void Start() {
			if (Manumation == null)
				return;

			sr = GetComponent<SpriteRenderer>();
			sr.sprite = Manumation.Sprite;
			sr.sortingOrder = Manumation.SortingOrder;
		}

		void Update() {
			if (Manumation == null)
				return;

			int newIndex = (int)(Time.time / Manumation.StepTime) % Manumation.Frames.Count;
			if (newIndex != _currentIndex) {
				_currentIndex = newIndex;
				sr.sprite = Manumation.Frames[_currentIndex];
			}
		}
	}

	public class Manumation {
		public LazySprite Sprite { get; }
		public List<LazySprite> Frames { get; } = new List<LazySprite>();
		public float StepTime { get; }
		public int SortingOrder = 0;

		public Manumation(LazySprite initialLazySprite, List<LazySprite> lazySprites, float stepTime, int sortingOrder) {
			this.Sprite = initialLazySprite;
			this.Frames = lazySprites;
			this.StepTime = stepTime;
			this.SortingOrder = sortingOrder;
		}
	}
}