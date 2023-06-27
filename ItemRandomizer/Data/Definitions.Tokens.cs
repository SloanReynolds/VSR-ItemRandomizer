using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItemRandomizer {
	public partial class Definitions {
		//ALL VALID (NON-MACRO) TOKENS
		public static readonly List<string> ValidTokens = new List<string>() {
			"CHARGE_SHOT",
			"ENERGY_CLAW",
			"CLAW_BREAKS_CHARGE_SHOT",
			"SPEED_BOOST_BREAKS_CLAW",
			"SPIN_DODGE",
			"WALL_RUN",
			"SPEED_BOOST",
			"HEAT_RESIST",
			"STRIP_SUIT",
		};
	}
}
