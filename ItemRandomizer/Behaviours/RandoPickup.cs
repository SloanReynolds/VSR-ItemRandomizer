using System;
using System.Reflection;
using ItemRandomizer.Coordinator;
using UnityEngine;

namespace ItemRandomizer {
	class RandoPickup : MonoBehaviour {
		private TimeUser _timeUser;
		private TUFloat _startPosX;
		private TUFloat _startPosY;
		public Vector2 startPos {
			get {
				return new Vector2(_startPosX.v, _startPosY.v);
			}
			set {
				_startPosX.v = value.x;
				_startPosY.v = value.y;
			}
		}
		private Vector3 startPos3 => new Vector3(startPos.x, startPos.y, 0);

		public void Awake() {
			_timeUser = GetComponent<TimeUser>();
			_startPosX = TUFloat.create(_timeUser);
			_startPosY = TUFloat.create(_timeUser);
			startPos = GetComponent<Rigidbody2D>().position;
		}

		public void changePosition(Vector2 newPos) {
			Vector3 b = newPos - startPos;
			startPos = newPos;
			PrivateParts.SetPrivateField(GetComponent<CreatureCardPickup>(), "startPos", startPos3);
			PrivateParts.SetPrivateField(GetComponent<HealthUpgradePickup>(), "centerPos", startPos);
			PrivateParts.SetPrivateField(GetComponent<Orb>(), "startPos", startPos);
			transform.position = transform.position + b;
		}
	}
}
