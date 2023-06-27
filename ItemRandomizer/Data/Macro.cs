using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ItemRandomizer {
	[JsonObject(MemberSerialization.OptIn)]
	public class Macro {
		[JsonProperty] public string Name { get; }
		[JsonProperty] public string LogicString { get; }

		private string _postfix = "";
		public string Postfix {
			get {
				if (LogicString == "") return "";
				if (_postfix == "") {
					_postfix = LogicHelper.ReplaceMacros(LogicHelper.InfixToPostfix(LogicString));
				}
				return _postfix;
			}
		}

		public Macro(string name, string logicString) {
			Name = name;
			LogicString = logicString;
		}

		public override string ToString() {
			return $"{Name} - '{LogicString}'";
		}
	}

	[JsonArray]
	public class Macros : List<Macro> { }
}
