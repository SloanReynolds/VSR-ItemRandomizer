using UnityEngine;

namespace ItemRandomizer.Resource {
	public static partial class Sprites {
		public static Sprite RailsSprite = null;

		public static SpriteRenderer MakeNewSpriteRendererObject(int sortOrder, Sprite sprite = null, int sortLayer = 1772338085, int sortGroup = 1048575, string goName = "IR_NewSpriteRenderer") {
			//1772338085 == 'Platforms'
			GameObject newGo = new GameObject(goName);
			SpriteRenderer sr = newGo.AddComponent<SpriteRenderer>();
			if (sprite != null) sr.sprite = sprite;
			sr.sortingGroupID = sortGroup;
			sr.sortingLayerID = sortLayer;
			sr.sortingOrder = sortOrder;

			return sr;
		}

		public static SpriteRenderer_FactoryObj NewSpriteRenderer(string goName, Sprite sprite, int sortOrder, int sortLayer = 1772338085, int sortGroup = 1048575) {
			return new SpriteRenderer_FactoryObj(goName, sprite, sortOrder, sortLayer, sortGroup);
		}

		public class SpriteRenderer_FactoryObj {
			private string _goName;
			private Sprite _sprite;
			private int _sortOrder;
			private int _sortLayer;
			private int _sortGroup;
			private Transform _parent;
			private Vector3 _offset;

			public SpriteRenderer_FactoryObj(string goName, Sprite sprite, int sortOrder, int sortLayer, int sortGroup) {
				//Known sortLayers:
				//1772338085 == 'Platforms'
				this._goName = goName;
				this._sprite = sprite;
				this._sortOrder = sortOrder;
				this._sortLayer = sortLayer;
				this._sortGroup = sortGroup;
			}
			public SpriteRenderer_FactoryObj WithParent(Transform newParent) => WithParent(newParent, Vector3.zero);

			public SpriteRenderer_FactoryObj WithParent(Transform newParent, Vector3 offset) {
				this._parent = newParent;
				this._offset = offset;

				return this;
			}


			public SpriteRenderer Make() {
				GameObject newGo = new GameObject(_goName);
				SpriteRenderer sr = newGo.AddComponent<SpriteRenderer>();
				if (_sprite != null) sr.sprite = _sprite;
				sr.sortingGroupID = _sortGroup;
				sr.sortingLayerID = _sortLayer;
				sr.sortingOrder = _sortOrder;

				if (_parent != null) {
					sr.transform.SetParent(_parent, false);
					if (_offset != Vector3.zero) {
						sr.transform.localPosition += _offset;
					}
				}

				return sr;
			}
		}
	}
}
