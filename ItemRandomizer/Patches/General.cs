using HarmonyLib;

namespace ItemRandomizer.Patches {
	[HarmonyPatch]
	class General {
		//[HarmonyPrefix]
		//[HarmonyPatch(typeof(ChamberScreen), "Start")]
		//static void ChamberScreen_Start() {

		//}
#if DEBUG
		[HarmonyPrefix]
		[HarmonyPatch(typeof(NodeData), "eventHappen")]
		static void NodeData_eventHappen(AdventureEvent.Physical eventID) {
			if (!Vars.currentNodeData.eventHappened(eventID)) {
				Plugin.I.LogInfo($"event {eventID} => happened");
			}
		}
		[HarmonyPrefix]
		[HarmonyPatch(typeof(NodeData), "eventUndo")]
		static void NodeData_eventUndo(AdventureEvent.Physical eventID) {
			if (Vars.currentNodeData.eventHappened(eventID)) {
				Plugin.I.LogInfo($"event {eventID} => undid");
			}
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(Vars), "eventHappen")]
		static void Vars_eventHappen(AdventureEvent.Info eventID) {
			if (!Vars.eventHappened(eventID)) {
				Plugin.I.LogInfo($"Info event {eventID} => happened");
			}
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(Vars), "eventNotHappen")]
		static void Vars_eventNotHappen(AdventureEvent.Info eventID) {
			if (Vars.eventHappened(eventID)) {
				Plugin.I.LogInfo($"Info event {eventID} => notHappen");
			}
		}
#endif
	}
}
