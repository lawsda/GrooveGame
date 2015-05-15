using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoogieDownGames {

	public class CharacterController : MonoBehaviour {

		public string demoAnimation;
		public string[] basicAnimations;
		public string[] goodAnimations;
		public string[] bestAnimations;

		[SerializeField]
		private Animator m_anime;

		[SerializeField]
		private GameObject [] m_models;

		[SerializeField]
		private int m_currentIndex;

		private bool m_triggerFired;
		private string m_lastMove;
		private Stack<string> m_animationQueue;

		void Start()
		{
			m_animationQueue = new Stack<string> ();
			SetCurrentModel(GameMaster.Instance.CurrentModel);
			NotificationCenter.DefaultCenter.AddObserver(this, "PlayStart");
			NotificationCenter.DefaultCenter.AddObserver(this, "PlayGood");
			NotificationCenter.DefaultCenter.AddObserver(this, "PlayBetter");
			NotificationCenter.DefaultCenter.AddObserver(this, "PlayBest");
			NotificationCenter.DefaultCenter.AddObserver(this, "PlayLame");
			NotificationCenter.DefaultCenter.AddObserver(this, "OnStateRunExit");
			NotificationCenter.DefaultCenter.AddObserver(this, "OnStateRunEnter");
			m_triggerFired = false;
			SetAnimationTrigger ("StandIdle");
		}

		public void OnStateRunExit()
		{
			m_anime.speed = 0f;
		}

		public void OnStateRunEnter()
		{
			m_anime.speed = 1f;
		}

		public void SetCurrentModel(int p_index)
		{
			for (int index = 0; index < m_models.Length; index ++) {
				m_models[index].SetActive(false);
			}
			m_models[p_index].SetActive(true);
			m_currentIndex = p_index;
			m_anime = m_models[m_currentIndex].GetComponent<Animator>();
			m_triggerFired = false;
		}

		public void NextModel()
		{
			//turn off current model
			m_models[m_currentIndex].SetActive(false);
			m_currentIndex ++;
			if (m_currentIndex >= m_models.Length) {
				m_currentIndex = 0;
			}
			m_models[m_currentIndex].SetActive(true);
			m_anime = m_models[m_currentIndex].GetComponent<Animator>();
			m_triggerFired = false;
			GameMaster.Instance.CurrentModel = m_currentIndex;
		}

		public void PrevModel()
		{
			//Turn off the current model
			m_models[m_currentIndex].SetActive(false);
			m_currentIndex--;
			if (m_currentIndex < 0) {
				m_currentIndex = m_models.Length-1;
			}
			m_models[m_currentIndex].SetActive(true);
			m_anime = m_models[m_currentIndex].GetComponent<Animator>();
			m_triggerFired = false;
			GameMaster.Instance.CurrentModel = m_currentIndex;
		}

		private string GetRandomArrayElement (string [] animationArray, string defaultValue) {
			string result;
			if (demoAnimation != null && demoAnimation.Length > 0) {
				result = demoAnimation;
			} else {
				if (animationArray != null && animationArray.Length > 0) {
					if (animationArray.Length > 1) {
						int index = Random.Range (0, animationArray.Length);
						result = animationArray [index];
					} else {
						result = animationArray [0];
					}
				} else {
					result = defaultValue;
				}
			}
			return result;
		}

		public void PlayGood()
		{
			SetAnimationTrigger (GetRandomArrayElement(basicAnimations, demoAnimation));
		}
		
		public void PlayBetter()
		{
			SetAnimationTrigger (GetRandomArrayElement(goodAnimations, demoAnimation));
		}
		
		public void PlayBest()
		{
			SetAnimationTrigger (GetRandomArrayElement(bestAnimations, demoAnimation));
		}
		
		public void PlayLame()
		{
			SetAnimationTrigger ("CheerJump");
		}

		public bool TriggerFired ()
		{
			return m_triggerFired;
		}

		private void SetAnimationTrigger (string animationTrigger)
		{
			if (m_anime != null) {
				if (animationTrigger == "") {
					animationTrigger = "StandIdle";
				}
				if (animationTrigger != m_lastMove) {
					Debug.Log ("Setting animation to " + animationTrigger);
					m_animationQueue.Push (animationTrigger);
					m_lastMove = animationTrigger;
					TriggerNextAnimation();
				}
			}
		}

		public void TriggerNextAnimation ()
		{
			string animationTrigger = m_animationQueue.Pop ();
			m_anime.SetTrigger (animationTrigger);
			m_triggerFired = true;
		}
	}
}