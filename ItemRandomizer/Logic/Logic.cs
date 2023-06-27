using System;
using System.Collections.Generic;
using System.Linq;
using ItemRandomizer.Coordinator;

namespace ItemRandomizer {
	public class Logic {
		public string PostfixAfterMacros => postfixAfterMacros;

		private const string AND = "&";
		private const string OR = "|";

		private string _infixRaw = null;
		private string _postfixRaw = null;
		private string _postfixAfterMacros = null;
		private string _postfix = null;

		private string postfixRaw {
			get {
				if (_postfixRaw == null) {
					_postfixRaw = _InfixToPostfix(_infixRaw);
				}
				return _postfixRaw;
			}
		}

		private string postfixAfterMacros {
			get {
				if (_postfixAfterMacros == null) {
					_postfixAfterMacros = _ReplaceMacros(postfixRaw);
				}
				return _postfixAfterMacros;
			}
		}

		private string postfix {
			get {
				if (_postfix == null) {
					_postfix = _Simplify(postfixAfterMacros);
				}
				return _postfix;
			}
		}

		public Logic(string logic) {
			_infixRaw = logic;
		}

		public bool Evaluate(List<Item> items) {
			if (postfix == "") return true;
			if (items.Count == 0) return false;

			return _Calculate(calc);

			bool calc(string token) {
				//'?' is just for notes of unsureness... or something
				bool negated = false;
				if (token.StartsWith("!")) {
					negated = true;
					token = token.Replace("!", "");
				}
				token = token.Replace("?", "");

				//Comparators
				if (token.Contains(">")) {
					string[] split = token.Split('>');
					string itemName = split[0].ToLower();
					int count = int.Parse(split[1]);
					switch (itemName) {
						case "card":
						case "cards":
							return items.Where(it => it is Item.Card).Count() > count;
						case "decrypt":
						case "decrypts":
						case "decryptor":
						case "decryptors":
						default:
							return items.Where(it => it is Item.Decrypt).Count() > count;
						case "orb":
						case "orbs":
							return items.Where(it => it is Item.Orb).Count() > count;
						case "health":
						case "heart":
						case "hearts":
							return items.Where(it => it is Item.Heart).Count() > count;
						case "phase":
						case "phases":
							return items.Where(it => it is Item.Phase).Count() > count;
					}
				}

				//Items
				if (items.Select(it => it.IDStr).Contains(token)) {
					return negated ? false : true;
				}

				//Options
				if (token.StartsWith("opt_")) {
					if (Options.TryGetOption(token, out bool optionVal)) {
						return negated ? !optionVal : optionVal;
					}
				} else if (!Definitions.ValidTokens.Contains(token)) {
					Plugin.I.LogError($"Logic Token `{token}` unrecognized!");
				}

				return negated ? true : false;
			}
		}

		private bool _Calculate(Predicate<string> calc) {
			Stack<bool> stack = new Stack<bool>();
			string[] tokens = postfix.Split(' ');

			for (int i = 0; i < tokens.Length; i++) {
				switch (tokens[i]) {
					case AND:
						stack.Push(stack.Pop() & stack.Pop());
						break;
					case OR:
						stack.Push(stack.Pop() | stack.Pop());
						break;
					default:
						stack.Push(calc(tokens[i]));
						break;
				}
			}

			if (stack.Count != 1) {
				Plugin.I.LogError("Error while calculating Logic: Too many tokens in the stack!");
			}

			return stack.Pop();
		}

		private string _InfixToPostfix(string infixRaw) {
			if (infixRaw == "") return "";

			return LogicHelper.InfixToPostfix(infixRaw);
		}

		private string _ReplaceMacros(string postfixRaw) {
			if (postfixRaw == "") return "";

			return LogicHelper.ReplaceMacros(postfixRaw);
		}

		private string _Simplify(string postfixAfterMacros) {
			if (postfixAfterMacros == "") return "";

			return LogicHelper.Simplify(postfixAfterMacros);
		}

		public override string ToString() {
			return _infixRaw;
		}
	}
}
