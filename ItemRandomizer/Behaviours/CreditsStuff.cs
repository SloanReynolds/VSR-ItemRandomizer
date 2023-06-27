using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemRandomizer.Coordinator;
using UnityEngine;
using UnityEngine.UI;

namespace ItemRandomizer {
	class CreditsStuff : MonoBehaviour {
		private float _time = 0f;
		private bool _isShowingStats = false;
		private bool _isFadingOut = false;
		private Text _statsRandoLabelText;
		private Text _statsRandoText;

		public float _statsRandoLabelAlpha {
			get {
				return _statsRandoLabelText.color.a;
			}
			set {
				Color color = _statsRandoLabelText.color;
				color.a = value;
				_statsRandoLabelText.color = color;
			}
		}

		public float _statsRandoAlpha {
			get {
				return _statsRandoText.color.a;
			}
			set {
				Color color = _statsRandoText.color;
				color.a = value;
				_statsRandoText.color = color;
			}
		}

		public Credits Credits { get; internal set; }

		void Start() {
			Credits.statsCompleteTime = 4f;

			_statsRandoLabelText = Instantiate(Credits.statsCompletionLabelText, Credits.statsCompletionLabelText.transform.parent);
			_statsRandoText = Instantiate(Credits.statsCompletionText, Credits.statsCompletionText.transform.parent);

			_statsRandoLabelText.rectTransform.position -= new Vector3(0, 80f);
			_statsRandoText.rectTransform.position -= new Vector3(0, 80f);
			_statsRandoLabelText.text = $"<b>Item Seed:</b>";
			_statsRandoText.text = $"{RandoState.Seed} v{PluginInfo.PLUGIN_VERSION}";
		}

		void Update() {
			_time += Time.unscaledDeltaTime;

			switch (Credits.state) {
				case Credits.State.STATS:
					if (!_isShowingStats) {
						_ShowStats();
						_isShowingStats = true;
					}
					_statsRandoLabelAlpha = Utilities.easeLinearClamp(_time - (Credits.statsCompletionFadeTime + 1f), 0f, 1f, Credits.fadeTextDuration);
					_statsRandoAlpha = Utilities.easeLinearClamp(_time - (Credits.statsCompletionFadeTime + 1f), 0f, 1f, Credits.fadeTextDuration);
					break;
				case Credits.State.FADE_OUT: {
					if (!_isFadingOut) {
						_time = 0f;
						_isFadingOut = true;
					}
					_statsRandoAlpha = _statsRandoLabelAlpha = Utilities.easeLinear(_time, 1f, -1f, Credits.fadeOutDuration);
					break;
				}
			}
		}

		private void _ShowStats() {
			_time = 0f;
			_statsRandoLabelAlpha = 0f;
			_statsRandoAlpha = 0f;
		}
	}
}
