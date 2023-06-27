using System;
using ItemRandomizer.Coordinator;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ItemRandomizer {
	public partial class Definitions : MonoBehaviour {
		void Awake() {
			SceneManager.sceneLoaded += this._SceneManager_sceneLoaded;
		}

		private void _SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode) {
			if (!PrefetchData.FullyLoaded && !Coordinator.PrefetchData.BusyLoading && scene.name == "title_scene") {
				//We only need to fetch one time, so we can unhook ourselves now
				SceneManager.sceneLoaded -= this._SceneManager_sceneLoaded;

				//Prefetch!
				StartCoroutine(PrefetchData.PrefetchAll(this));
			}
		}
	}
}
