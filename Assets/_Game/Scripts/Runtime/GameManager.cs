namespace StuckInALoop
{
	using System;
	using System.Collections;
	using PrimitiveEngine.Unity;
	using Spineless;
	using TMPro;
	using UnityEngine;
	using UnityEngine.SceneManagement;
	using UnityEngine.UI;


	public class GameManager: Game
	{
		private static GameObject _player;
		private Image fader;

		private TextMeshProUGUI levelCodeText;


		public static TextMeshProUGUI LevelCodeText
		{
			get
			{
				GameManager instance = (GameManager)Instance;
				if (instance.levelCodeText == null)
				{
					GameObject levelCodeObject = GameObject.Find("Text-LevelCode");
					if (levelCodeObject != null)
						instance.levelCodeText = levelCodeObject.GetComponent<TextMeshProUGUI>();
				}
					
				return ((GameManager)Instance).levelCodeText;
			}
		}


		public static Image Fader
		{
			get
			{
				GameManager instance = (GameManager)Instance;
				if (instance.fader == null)
					instance.fader = GameObject.Find("FaderImage").GetComponent<Image>();
				return ((GameManager)Instance).fader;
			}
		}


		public static GameObject Player
		{
			get
			{
				if (_player == null)
					_player = FindObjectOfType<PlayerInput>().gameObject;
				return _player;
			}
		}


		public override void Awake()
		{
			SceneManager.LoadScene("UI", LoadSceneMode.Additive);
			base.Awake();
		}


		public static void FadeIn(Color startColor, float time, Action callback)
		{
			Fader.color = startColor;
			Coroutines.EaseImageColor(Fader, Color.clear, time, callback);
		}


		public static void FadeOut(Color endColor, float time, Action callback)
		{
			Fader.color = Color.clear;
			Coroutines.EaseImageColor(Fader, endColor, time, callback);
		}
	}
}