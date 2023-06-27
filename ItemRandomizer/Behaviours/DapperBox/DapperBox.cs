using System;
using UnityEngine;

namespace ItemRandomizer {
	public abstract class DapperBox {
		public static GlyphBox Reference = null;
		private static int _order = 0;
		private const float _SCALE = 0.65f;
		private GlyphBox _glyphBox = null;
		protected readonly string _originalText = "";
		private string _currentGlyphText = "";
		private bool _enabled = true;

		public static void Reset() {
			_order = 0;
		}

		public bool Enabled {
			get => _enabled;
			set => _SetEnabled(value);
		}

		private void _SetEnabled(bool value) {
			_enabled = value;
			if (value) {
				_glyphBox.setColor(PauseScreen.DEFAULT_COLOR);

				return;
			}

			//Turn off
			_glyphBox.setColor(Color.gray);
		}

		public DapperBox(string text) {
			_originalText = _currentGlyphText = text;
			_glyphBox = _NewTempOptionBox(Reference, text, _SCALE, _order);
			_order++;

			//this.Show();
		}

		public static implicit operator GlyphBox(DapperBox box) => box._glyphBox;

		public void hide() {
			_glyphBox.makeAllCharsInvisible();
		}

		public virtual void Show() {
			SetText(_currentGlyphText, true);
		}

		public void SetText(string text, bool force = false) {
			if (force || _currentGlyphText != text && _glyphBox != null) {
				_currentGlyphText = text;
				_glyphBox.setText(text);
			}
		}

		public void UpdateText(params string[] strings) {
			string newStr = _originalText;
			for (int i = 0; i < strings.Length; i++) {
				newStr = newStr.Replace($"{{{i}}}", strings[i]);
			}
			SetText(newStr);
		}

		public abstract void DapperBoxUpdate();

		private static GlyphBox _NewTempOptionBox(GlyphBox reference, string text, float scale = 1.0f, int order = 0) {
			GlyphBox newGB = GameObject.Instantiate(reference, reference.transform.parent).GetComponent<GlyphBox>();
			newGB.rectTransform.localScale = newGB.rectTransform.localScale * scale;

			newGB.width = 100;

			newGB.gameObject.SetActive(true);
			newGB.setText(text);

			int pixelScale = 3;

			newGB.alignment = GlyphBox.Alignment.RIGHT;
			newGB.rectTransform.pivot = new Vector2(1f, 1f);
			float posx = Screen.width - 28f; //Right Edge
			float posy = Screen.height - 5f - (order > 0 ? (newGB.getLineHeight() + 1) * order * pixelScale * scale : 0);
			newGB.transform.position = new Vector3(posx, posy, newGB.transform.position.z);

			return newGB;
		}
	}
}
