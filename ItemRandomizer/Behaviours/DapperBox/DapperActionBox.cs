using System;
using ItemRandomizer.Coordinator;
using Mono.Cecil;
using UnityEngine;

namespace ItemRandomizer {
	public class DapperActionBox : DapperBox {
		private bool _canBypassEnabled = false;
		protected KeyCode _actionKey = KeyCode.None;
		protected Action<DapperActionBox> _action = null;
		protected void _InvokeExternal() {
			_action?.Invoke(this);
		}

		public DapperActionBox(string text, KeyCode actionKey, string inString, Action<DapperActionBox> action = null) : base(text) {
			_action = action;
			_actionKey = actionKey;

			UpdateText(inString);
		}

		public DapperActionBox CanBypassEnabled() {
			_canBypassEnabled = true;
			return this;
		}

		public override void DapperBoxUpdate() {
			if (Configs.Enabled || _canBypassEnabled) {
				if (_actionKey != KeyCode.None && Input.GetKeyDown(_actionKey)) {
					_InvokeExternal();
				}
			}
		}
	}
}
