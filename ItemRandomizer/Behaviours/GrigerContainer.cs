using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ItemRandomizer {
	class GrigerContainer : MonoBehaviour {
		public Rigidbody2D rb2d;
		public RandoPickup RandoPickup {
			get {
				if (_pickupRef == null) {
					_pickupRef = FindObjectOfType<RandoPickup>().gameObject;
				}
				return _pickupRef.GetComponent<RandoPickup>();
			}
		}

		private GameObject _pickupRef = null;

		void Awake() {
			rb2d = GetComponent<Rigidbody2D>();
		}
	}
}
