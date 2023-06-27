using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ItemRandomizer.Resource {
	public class LazySprite {
		private const string _RESOURCE_PATH = "ItemRandomizer.Resources";
		public string ResourceName { get; }
		public string ResourcePath => $"{_RESOURCE_PATH}.{ResourceName}";
		public Sprite Sprite => _sprite ??= _makeSprite();

		private Sprite _sprite = null;

		private Rect _rect = Rect.zero;

		public LazySprite(string resourceName) : this(resourceName, Rect.zero) { }

		public LazySprite(string resourceName, Rect rect) {
			//Console.WriteLine($"New LazySprite {resourceName}");
			this.ResourceName = resourceName;
			this._rect = rect;
		}

		private Sprite _makeSprite() {
			//Console.WriteLine($"LazySprite _makeSprite() {ResourceName}");
			Texture2D tex = _makeTexture();
																												//fucking y axis
			Rect rect = _rect == Rect.zero ? new Rect(0, 0, tex.width, tex.height) : new Rect(_rect.x, tex.height - _rect.height - _rect.y, _rect.width, _rect.height);
			return Sprite.Create(tex, rect, new Vector2(0.5f, 0.5f), 16f);
		}

		private Texture2D _makeTexture() {
			//Load up all the one sprites!
			//Plugin.I.LogInfo(ResourcePath);
			Stream img = typeof(Plugin).Assembly.GetManifestResourceStream($"{ResourcePath}");
			byte[] buff = new byte[img.Length];
			img.Read(buff, 0, buff.Length);
			img.Dispose();

			Texture2D texture = new Texture2D(1, 1);
			texture.LoadImage(buff, true);
			texture.filterMode = FilterMode.Point;

			return texture;
		}


		public static implicit operator Sprite(LazySprite ls) => ls.Sprite;
	}
}
