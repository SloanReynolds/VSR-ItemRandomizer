using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine.XR;

namespace ItemRandomizer {

	public abstract class Item : IEquatable<Item> {
		public static Item FromIDStr(string idStr) {
			if (idStr.StartsWith("Card")) return Card.FromIDStr(idStr);
			if (idStr.StartsWith("PU")) return Phase.FromIDStr(idStr);
			if (idStr.StartsWith("HU")) return Heart.FromIDStr(idStr);
			if (idStr.StartsWith("ORB")) return Orb.FromIDStr(idStr);
			/*if idStr looks as Decrypt*/ return Decrypt.FromIDStr(idStr);
		}

		public string IDStr { get; private set; }

		public Item(string logicName) {
			IDStr = logicName;
		}

		protected Item() { }

		public override string ToString() {
			return IDStr;
		}

		public bool Equals(Item other) {
			if (ReferenceEquals(this, null) && ReferenceEquals(other, null)) return true;   //null == null
			if (ReferenceEquals(this, null) || ReferenceEquals(other, null)) return false;  //null != other
			if (this.IDStr == other.IDStr) return true;
			return false;
		}

		public static bool operator ==(Item item1, Item item2) => EqualityComparer<Item>.Default.Equals(item1,item2);
		public static bool operator !=(Item item1, Item item2) => !(item1 == item2);

		public class Card : Item {
			public static new Card FromIDStr(string idStr) {
				//Card ctor takes "Shaifi"; idStr is "Card_Shaifi"
				return new Card(idStr.Substring(5));
			}
			public string ID { get; private set; }

			public Card(string id) : base() {
				ID = id;
				IDStr = $"Card_{id}".ToUpper();
			}
		}

		public class Decrypt : Item {
			public static new Decrypt FromIDStr(string idStr) {
				if (Enum.TryParse(idStr, out Decryptor.ID id)) {
					return new Decrypt(id);
				}
				throw new Exception($"No Decryptor Found for idStr '{idStr}'");
			}
			public Decryptor.ID ID { get; private set; }
			public Decrypt(Decryptor.ID id) : base() {
				ID = id;
				IDStr = id.ToString().ToUpper();
			}
		}

		public class Heart : Item {
			public static new Heart FromIDStr(string idStr) {
				if (Enum.TryParse(idStr, out PhysicalUpgrade.HealthUpgrade id)) {
					return new Heart(id);
				}
				throw new Exception($"No HealthUpgrade Found for idStr '{idStr}'");
			}
			public PhysicalUpgrade.HealthUpgrade ID { get; private set; }
			public Heart(PhysicalUpgrade.HealthUpgrade id) : base() {
				ID = id;
				IDStr = id.ToString().ToUpper();
			}
		}

		public class Orb : Item {
			public static new Orb FromIDStr(string idStr) {
				if (Enum.TryParse(idStr, out PhysicalUpgrade.Orb id)) {
					return new Orb(id);
				}
				throw new Exception($"No Orb Found for idStr '{idStr}'");
			}
			public PhysicalUpgrade.Orb ID { get; private set; }
			public Orb(PhysicalUpgrade.Orb id) : base() {
				ID = id;
				IDStr = id.ToString().ToUpper();
			}
		}

		public class Phase : Item {
			public static new Phase FromIDStr(string idStr) {
				if (Enum.TryParse(idStr, out PhysicalUpgrade.PhaseUpgrade id)) {
					return new Phase(id);
				}
				throw new Exception($"No PhaseUpgrade Found for idStr '{idStr}'");
			}
			public PhysicalUpgrade.PhaseUpgrade ID { get; private set; }
			public Phase(PhysicalUpgrade.PhaseUpgrade id) : base() {
				ID = id;
				IDStr = id.ToString().ToUpper();
			}
		}
	}
}
