﻿using UnityEngine;
using System.Collections;

namespace BoogieDownGames {

	public enum NoteStates { UnReady, LowScore, MidScore, HighScore, Die }

	public class NotesControl : MonoBehaviour {

		[SerializeField]
		private Vector3 m_vect;

		[SerializeField]
		private float m_speed;

		[SerializeField]
		private int m_maxTimeTillChange;

		[SerializeField]
		private TimeKeeper m_timer;

		[SerializeField]
		private bool m_canDie = false;

		[SerializeField]
		private SkinnedMeshRenderer m_mat;

		[SerializeField]
		private NoteStates m_myState;

		[SerializeField]
		private float m_firstBounds;

		[SerializeField]
		private float m_secondBounds;

		[SerializeField]
		private float m_thirdBounds;

		[SerializeField]
		private float m_dieBounds;

		[SerializeField]
		private int m_score;

		[SerializeField]
		private int m_scoreMod;

		[SerializeField]
		private Color m_unReadyColor;

		[SerializeField]
		private Color m_lowScoreColor;

		[SerializeField]
		private Color m_midScoreColor;

		[SerializeField]
		private Color m_highScoreColor;

		private float m_rotation;
		private Vector3 m_direction;
		private GameObject m_mainCamera;
		private float m_zLimit;
		private float zDistanceFromCamera;

		#region PROPERTIES

		public int Score
		{
			get { return m_score *  m_scoreMod; }
			set { m_score = value; }
		}

		public int ScoreMod
		{
			get { return m_scoreMod; }
			set { m_scoreMod = value; }
		}

		#endregion

		void Start () 
		{
			m_myState = NoteStates.UnReady;
			m_mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
			m_zLimit = m_mainCamera.transform.position.z;
			NotificationCenter.DefaultCenter.AddObserver(this,"OnStateRunFixedUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this,"OnStateRunUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this,"OnStateLostSongEnter");

			// to provide some variety, flip half the notes
			if (Random.value > 0.5) {
				transform.localRotation = transform.localRotation * Quaternion.Euler(0, 180, 0);
				m_direction = Vector3.forward;
			} else {
				m_direction = Vector3.forward * -1.0f;
			}
			// Send notes off in slightly random directions but toward the camera
			float x = Random.Range(-0.125f, 0.125f);
			float y = Random.Range(-0.125f, 0.125f);
			m_direction = new Vector3(m_direction.x + x, m_direction.y + y, m_direction.z);
		}

		public void OnStateRunFixedUpdate()
		{
			transform.Translate(m_direction * m_speed);
		}

		public void OnStateRunUpdate()
		{
			CheckState();
		}

		public void death()
		{
			Debug.LogFormat ("Note tapped at z={0}", transform.position.z);
			if (m_myState != NoteStates.UnReady) {
				PostMessage("PlayEvent", m_myState.ToString());
				NotificationCenter.DefaultCenter.PostNotification(this, "PlayAnime");
				DanceGameController.Instance.HitNotes ++;
				PostMessage("ReadEvent", "event", "Notes");
				gameObject.SetActive(false);
			}
			PostMessage("PlayEvent", m_myState.ToString());
		}

		public void OnStateLostSongEnter()
		{
			gameObject.SetActive(false);
		}

		public void CheckState()
		{
			zDistanceFromCamera = transform.position.z - m_zLimit;
			if (zDistanceFromCamera > m_firstBounds) {
				m_myState = NoteStates.UnReady;
				m_mat.material.SetColor("_Color", m_unReadyColor);
				m_mat.material.SetColor("_Emission", m_unReadyColor);
				m_mat.material.SetColor("_SpecColor", m_unReadyColor);
			} else if (zDistanceFromCamera <= m_firstBounds && zDistanceFromCamera > m_secondBounds) {
				m_mat.material.SetColor("_Color", m_lowScoreColor);
				m_mat.material.SetColor("_Emission", m_lowScoreColor);
				m_mat.material.SetColor("_SpecColor", m_lowScoreColor);
				m_myState = NoteStates.LowScore;
			} else if (zDistanceFromCamera <= m_secondBounds && zDistanceFromCamera > m_thirdBounds) {
				m_mat.material.SetColor("_Color", m_midScoreColor);
				m_mat.material.SetColor("_Emission", m_midScoreColor);
				m_mat.material.SetColor("_SpecColor", m_midScoreColor);
				m_myState = NoteStates.MidScore;
			} else if (zDistanceFromCamera > m_dieBounds && zDistanceFromCamera <= m_thirdBounds) {
				m_mat.material.SetColor("_Color", m_highScoreColor);
				m_mat.material.SetColor("_Emission", m_highScoreColor);
				m_mat.material.SetColor("_SpecColor", m_highScoreColor);
				m_myState = NoteStates.HighScore;
			} else {
				// if passes the camera this is a missed note
				gameObject.SetActive(false);
				DanceGameController.Instance.MissNotes ++;
			}
		}

		public void PostMessage(string p_func, string p_message)
		{
			Hashtable dat = new Hashtable();
			dat.Add("eventName", p_message);
			NotificationCenter.DefaultCenter.PostNotification(this, p_func, dat);
		}

		public void PostMessage(string p_func, string p_key, string p_message)
		{
			Hashtable dat = new Hashtable();
			dat.Add(p_key,p_message);
			NotificationCenter.DefaultCenter.PostNotification(this, p_func, dat);
		}
	}
}