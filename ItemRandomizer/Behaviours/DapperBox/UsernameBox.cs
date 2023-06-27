using ItemRandomizer.Patches;
using UnityEngine;

namespace ItemRandomizer {
	public class UsernameBox : DapperBox {
		private string _username;
		private bool _editMode = false;
		private KeyCode _actionKey = KeyCode.None;

		public UsernameBox(string username, string text, KeyCode actionKey) : base(text) {
			this._username = username;
			this._actionKey = actionKey;
		}

		private string _Text => _originalText.Replace("{0}", _username);

		private void _TurnOnEditMode() {
			_editMode = true;
			PermaPatch_UI.BlockInputOnTitleScene = true;
		}

		private void _TurnOffEditMode() {
			_editMode = false;
			PermaPatch_UI.BlockInputOnTitleScene = false;
			SetText($"{_Text}");
		}

		public override void DapperBoxUpdate() {
			if (Input.GetKeyDown(_actionKey)) {
				if (_editMode) _TurnOffEditMode();
				else _TurnOnEditMode();
			}

			if (_editMode) {
				string flash = "_";
				if (Mathf.Round(Time.unscaledTime) != Mathf.Round(Time.unscaledTime + 0.5f)) {
					flash = " ";
				}

				//We are editing; let's listen for keyboard input.
				foreach (char c in Input.inputString) {
					if (c == '\b') { // has backspace/delete been pressed?
						if (_username.Length != 0) {
							_username = _username.Substring(0, _username.Length - 1);
						}
					} else if ((c == '\n') || (c == '\r')) { // enter/return
						_TurnOffEditMode();
						break;
					} else {
						if (_username.Length < 50) {
							_username += c;
						}
					}
				}

				if (_editMode) SetText($"{_Text}{flash}");
			}
		}
	}
}
