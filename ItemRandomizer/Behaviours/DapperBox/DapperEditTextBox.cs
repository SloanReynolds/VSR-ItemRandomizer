using System;
using ItemRandomizer.Patches;
using UnityEngine;

namespace ItemRandomizer {
	public class DapperEditTextBox : DapperActionBox {
		private string _current = "";
		private bool _editMode = false;
		private readonly Action<string> _actionValueUpdated = null;
		private Action<string> _actionEditModeTurnedOff = null;

		public string Current {
			get => _current;
			set => _ReportIfNewText(value);
		}

		public bool EditMode {
			get => _editMode;
			set {
				if (value) _TurnOnEditMode();
				else _TurnOffEditMode();
			}
		}


		private readonly int _charLimit;
		private readonly bool _numbersOnly;
		public DapperEditTextBox(string text, KeyCode actionKey, string inString, Action<string> actionValueUpdated = null, int charLimit = 50, bool numbersOnly = false) : base(text, actionKey, inString) {
			this._action = _ToggleEdit;
			this._current = inString;
			this._actionValueUpdated = actionValueUpdated;
			this._charLimit = charLimit;
			this._numbersOnly = numbersOnly;
		}

		private void _ToggleEdit(DapperActionBox obj) {
			if (_editMode) _TurnOffEditMode();
			else _TurnOnEditMode();
		}

		private void _TurnOnEditMode() {
			_editMode = true;
			PermaPatch_UI.BlockInputOnTitleScene = true;
		}

		private void _TurnOffEditMode() {
			bool prev = _editMode;
			_editMode = false;
			PermaPatch_UI.BlockInputOnTitleScene = false;
			_ForceReport(_current);

			//Don't want to invoke if it wasn't actually turned off...
			if (prev) _actionEditModeTurnedOff?.Invoke(_current);
		}

		public override void DapperBoxUpdate() {
			base.DapperBoxUpdate(); //Toggle Edit Mode First


			if (_editMode) {
				string newString = _current;

				string flash = "_";
				if (Mathf.Round(Time.unscaledTime) != Mathf.Round(Time.unscaledTime + 0.5f)) {
					flash = " ";
				}

				//We are editing; let's listen for keyboard input.
				foreach (char c in Input.inputString) {
					if (c == '\b') { // has backspace/delete been pressed?
						if (newString.Length != 0) {
							newString = newString.Substring(0, newString.Length - 1);
						}
					} else if ((c == '\n') || (c == '\r')) { // enter/return
						_TurnOffEditMode();
						break;
					} else {
						if ((!_numbersOnly || char.IsDigit(c)) && newString.Length < _charLimit) {
							newString += c;
						}
					}
				}

				if (_editMode) {
					_ReportIfNewText(newString, flash);
					return;
				}
			}
		}

		internal DapperBox WithEditDone(Action<string> actionEditModeTurnedOff) {
			_actionEditModeTurnedOff = actionEditModeTurnedOff;
			return this;
		}

		private string _currentFlash = "";
		private void _ReportIfNewText(string outString, string flash = "") {
			if (flash == _currentFlash && outString == _current) {
				return;
			}
			_ForceReport(outString, flash);
		}

		private void _ForceReport(string outString, string flash = "") {
			if (this._actionValueUpdated != null)
				this._actionValueUpdated.Invoke(outString + flash);
			else
				UpdateText(outString + flash);

			_current = outString;
			_currentFlash = flash;
		}
	}
}
