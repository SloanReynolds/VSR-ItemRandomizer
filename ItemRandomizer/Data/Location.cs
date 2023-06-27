using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace ItemRandomizer {

	[JsonObject(MemberSerialization.OptIn)]
	public class Location {
		[JsonProperty] public byte Number { get; }
		[JsonProperty] public string Area { get; }
		[JsonProperty] public string Scene { get; }
		[JsonProperty] public int MapX { get; }
		[JsonProperty] public int MapY { get; }
		[JsonProperty] public string GameObject { get; }
		[JsonProperty] public string IDStr { get; }
		[JsonProperty] public string LogicString { get; }

		public Vector2 MapCoords => new Vector2(MapX, MapY);

		public Location(byte number, string area, string scene, int mapX, int mapY, string gameObject, string idStr, string logicString) {
			this.Number = number;
			this.Area = area;
			this.Scene = scene;
			this.MapX = mapX;
			this.MapY = mapY;
			this.GameObject = gameObject;
			this.IDStr = idStr;
			this.LogicString = logicString;
		}

		private Item _originalItem = null;
		public Item OriginalItem {
			get {
				if (_originalItem == null) {
					_originalItem = Item.FromIDStr(IDStr);
				}
				return _originalItem;
			}
		}

		private Item _currentItem = null;
		public Item CurrentItem {
			get {
				if (_currentItem == null) return OriginalItem;
				return _currentItem;
			}
		}

		private Logic _logic = null;
		public Logic Logic {
			get {
				if (_logic == null) {
					_logic = new Logic(this.LogicString);
				}
				return _logic;
			}
		}

		public void SetItem(Item item) {
			_currentItem = item;
		}

		public override string ToString() {
			return $"{Scene}:{GameObject}";
		}
	}

	[JsonArray]
	public class Locations : List<Location> {
		public static Locations _initial = null;
		public static Locations Initial => _initial ??= _InitJsonLocations();

		private static Locations _InitJsonLocations() {
			using (StreamReader sr = new StreamReader(typeof(Plugin).Assembly.GetManifestResourceStream("ItemRandomizer.Data.Logic.locations.json"))) {
				string json = sr.ReadToEnd();
				return JsonConvert.DeserializeObject<Locations>(json);
			}
		}

		public IEnumerable<Location> ForScene(string curScene) {
			return this.Where(loc => loc.Scene == curScene);
		}

		public List<string> SceneNames() {
			return this.Where(loc => loc.OriginalItem != loc.CurrentItem).Select(loc => loc.Scene).Distinct().ToList();
		}

		public Location WithCurrentItem(Item item) {
			return this.Where(loc => loc.CurrentItem == item).First();
		}
	}
}

// 24 cards
// 4 hearts
// 4 phases
// 4 orbs
// 16 decrypts