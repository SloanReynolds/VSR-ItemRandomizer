using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using ItemRandomizer.Coordinator;

namespace ItemRandomizer {
	public static class LogicHelper {
		public static readonly Operator AND = new string[] { "&", "+" };
		public static readonly Operator OR = "|";

		public static string InfixToPostfix(string infix) {
			int i = 0;
			Stack<string> stack = new Stack<string>();
			List<string> postfix = new List<string>();

			List<string> uniqueTokens = new List<string>();

			while (i < infix.Length) {
				string sym = _GetNextToken(infix, ref i);

				// Easiest way to deal with whitespace between operators
				if (sym.Trim(' ') == string.Empty) {
					continue;
				}

				// Order of Operations:  "&" > "|"
				if (sym == AND || sym == OR) {
					while (stack.Count != 0 && (sym == OR || (sym == AND && stack.Peek() != OR)) && stack.Peek() != "(") {
						postfix.Add(stack.Pop());
					}

					stack.Push(sym);
				} else if (sym == "(") {
					stack.Push(sym);
				} else if (sym == ")") {
					while (stack.Peek() != "(") {
						postfix.Add(stack.Pop());
					}

					stack.Pop();
				} else {
					postfix.Add(sym);
				}

			}

			while (stack.Count != 0) {
				postfix.Add(stack.Pop());
			}

			return string.Join(" ", postfix.ToArray());
		}

		public static string ReplaceMacros(string postfix) {
			bool keepGoing = true;
			string postfixL = postfix;
			while (keepGoing) {
				keepGoing = false;
				foreach (string k in RandoState.Macros.Select(m => m.Name)) {
					if (postfixL.Contains(k)) {
						keepGoing = true;

						postfixL = postfixL.Replace(k, RandoState.Macros.Where(m=>m.Name == k).First().Postfix);
					}
				}
			}
			return postfixL;
		}

		public static string Simplify(string postfix) {
			string ret = PostfixSimplificator.Simplify(postfix);
			return ret;
		}




































		private static string _GetNextToken(string infix, ref int i) {
			int start = i;

			if (infix[i] == '(' || infix[i] == ')' || infix[i] == AND || infix[i] == OR) {
				i++;
				return infix[i - 1].ToString();
			}

			while (i < infix.Length && infix[i] != '(' && infix[i] != ')' && infix[i] != AND && infix[i] != OR) {
				i++;
			}

			return infix.Substring(start, i - start).Trim(' ');
		}
	}

	public readonly struct Operator {
		private readonly string _opStr;
		private readonly string _opStr2;

		public Operator(string str, string str2="") {
			_opStr = str;
			_opStr2 = str2;
		}

		public override string ToString() {
			return _opStr;
		}

		public override bool Equals(object obj) {
			return obj is Operator @operator &&
				   this._opStr == @operator._opStr &&
				   this._opStr2 == @operator._opStr2;
		}

		public override int GetHashCode() {
			int hashCode = -751921306;
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this._opStr);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this._opStr2);
			return hashCode;
		}

		public static implicit operator Operator(string c) => new Operator(c);
		public static implicit operator Operator(string[] c) => new Operator(c[0], c[1]);
		public static implicit operator char(Operator c) => c._opStr[0];
		public static implicit operator string(Operator c) => c._opStr;

		public static bool operator ==(Operator left, string right) {
			if (left._opStr == right || left._opStr2 == right)
				return true;
			return false;
		}

		public static bool operator !=(Operator left, string right) {
			return !(left == right);
		}
	}
}
