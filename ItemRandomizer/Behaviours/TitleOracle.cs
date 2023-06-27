using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemRandomizer.Coordinator;
using ItemRandomizer.Resource;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ItemRandomizer {
	class TitleOracle : MonoBehaviour {
		private GameObject _spotLight;
		private TitleAnimation _titleAnimation;
		private TitleAnimation2 _titleAnimation2;
		private Animator _animator;


		void Awake() {
			_spotLight = transform.parent.Find("title_animation_cover").gameObject;
			_spotLight.transform.SetParent(this.transform);
			_spotLight.transform.localPosition += new Vector3(0f, -0.8f);

			Sprite blackSquare = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 4f, 4f), new Vector2(0.5f, 0.5f), 1f, 0, SpriteMeshType.FullRect);

			for (int i = -1; i < 2; i++) {
				for (int j = -1; j < 2; j++) {
					if (i != 0 || j != 0) {
						//Skip middle
						_MakeEdgeSprite(blackSquare, i, j);
					}
				}
			}

			_titleAnimation = Resources.FindObjectsOfTypeAll<TitleAnimation>().First();
			_titleAnimation2 = Resources.FindObjectsOfTypeAll<TitleAnimation2>().First();

			_titleAnimation2.gameObject.SetActive(false);
			//_spotLight.gameObject.SetActive(false);

			this.transform.localPosition = new Vector3(-33f, 1.94f);
			_TurnAround();
		}

		void Start() {
			_titleAnimation.minDisplayWidth = 55f;
			_animator = GetComponent<Animator>();
			_animator.speed = _speed;

			Sprites.RailsSprite = _titleAnimation.railsGameObject.GetComponent<SpriteRenderer>().sprite;
		}

		void _MakeEdgeSprite(Sprite sprite, int x, int y) {
			SpriteRenderer sr = Instantiate(_spotLight, this.transform).GetComponent<SpriteRenderer>();
			sr.sprite = sprite;
			sr.color = Color.black;
			sr.drawMode = SpriteDrawMode.Tiled;
			sr.transform.localPosition += new Vector3(x * 45f, y * 25f);
			sr.transform.localScale = new Vector3(45f, 25f);
		}

		private int _direction = 1;
		private float _speed = 3 / 4f;
		private float _animSin => Mathf.Sin(-_animator.GetCurrentAnimatorStateInfo(0).normalizedTime * (2 * Mathf.PI));
		private float _animCos => -Mathf.Cos(-(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime + 0.15f) * (2 * Mathf.PI));
		private float _wormSpeed => _direction * (_speed * (_animSin + 2f) / 2f) * Time.deltaTime * 4f;
		private float _platformSpeed => _direction * Time.deltaTime * 4f;

		void Update() {
			this.transform.localPosition += new Vector3(_wormSpeed, 0f);

			if (this.transform.localPosition.x < -33f || this.transform.localPosition.x > 33f) {
				_TurnAround(this.transform.localPosition.x < -33f);
			}

		}

		private float dist = 0;
		public void UpdatePlatforms_Post(List<GameObject> platforms, List<GameObject> rails) {
			float num = _titleAnimation.platformGameObject.GetComponent<SpriteRenderer>().sprite.rect.width / _titleAnimation.platformGameObject.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
			float num2 = _titleAnimation.railsGameObject.GetComponent<SpriteRenderer>().sprite.rect.width / _titleAnimation.railsGameObject.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;

			dist += -_platformSpeed / 5f;

			for (int i = 0; i < platforms.Count; i++) {
				GameObject gameObject3 = platforms[i];
				int num5 = ((i % 3 >= 2) ? 1 : 0);
				gameObject3.GetComponent<SpriteRenderer>().sprite = _titleAnimation.platformSprites[num5];

				float x = dist + (float)i * num;
				Vector2 v = new Vector2(x, _titleAnimation.platformY);
				v.x = Utilities.fmod(y: Mathf.Ceil(_titleAnimation.minDisplayWidth / num) * num, x: v.x) - _titleAnimation.minDisplayWidth / 2f;
				gameObject3.transform.localPosition = v;
			}
			for (int j = 0; j < rails.Count; j++) {
				GameObject gameObject4 = rails[j];

				float x2 = dist * _titleAnimation.railsParallax + (float)j * num2;
				Vector2 v2 = new Vector2(x2, _titleAnimation.platformY + _titleAnimation.railsDistFromPlatforms);
				v2.x = Utilities.fmod(y: Mathf.Ceil(_titleAnimation.minDisplayWidth / num2) * num2, x: v2.x) - _titleAnimation.minDisplayWidth / 2f;
				gameObject4.transform.localPosition = v2;
			}
		}

		void _TurnAround(bool onLeft = true) {
			if (onLeft) {
				_direction = 1;
				this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
			} else {
				_direction = -1;
				this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
			}
			//_titleAnimation.oracleMovementSpeed *= -1;

			float verticalPos = Random.Range(-9f, 10f);
			_titleAnimation.transform.localPosition = new Vector3(_titleAnimation.transform.localPosition.x, verticalPos);

			transform.localPosition = new Vector3(transform.localPosition.x, 1.94f);
		}
	}
}
