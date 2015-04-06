﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace BoogieDownGames {

	public class SongItem {
		public string id;
		public string title;
		public string artist;
		public string duration;
		public bool paid;

		public SongItem(string _id, string _title, string _artist, string _duration, bool _paid) {
			id = _id;
			title = _title;
			artist = _artist;
			duration = _duration;
			paid = _paid;
		}
	}

	public class TextMachine : MonoBehaviour {
		
		[SerializeField]
		Text m_songTitleText;

		[SerializeField]
		Text m_songArtistText;

		[SerializeField]
		Text m_songDurationText;
		
		[SerializeField]
		Image m_songIcon;
		
		[SerializeField]
		private string m_keyToListenFor;

		private List<SongItem> m_songList;


		void Start () {
			m_songList = new List<SongItem>();
			m_songList.Add (new SongItem ("song1", "I'm Sexy Now", "Sky Silver & the Sky Girls", "2:00", true));
			m_songList.Add (new SongItem ("song2", "She Smiles", "Sky Silver", "2:00", true));
			m_songList.Add (new SongItem ("song3", "I Got You", "Sky Girl Jo", "2:07", false));
			m_songList.Add (new SongItem ("song4", "Alone Tonight", "Dani", "1:30", false));
			m_songList.Add (new SongItem ("song5", "It's Filth", "Sky Silver", "1:44", false));
			m_songList.Add (new SongItem ("song6", "Fry & Sizzle", "Delsa & Sky Silver", "1:46", false));
			m_songList.Add (new SongItem ("song7", "Shut Up & Dance", "Delsa Feat. Ai Man", "1:31", true));
			m_songList.Add (new SongItem ("song8", "She Said", "Dani Feat. Sky Silver", "1:53", false));
			m_songList.Add (new SongItem ("song9", "In Fashion", "Sky Silver", "1:33", false));

			NotificationCenter.DefaultCenter.AddObserver(this,"SetText");
			SetAll ("song1");
		}
		
		public void SetText(NotificationCenter.Notification p_not) {
			SetAll ((string)p_not.data[m_keyToListenFor]);
		}

		private void SetAll(string songId) {
			if (songId != null) {
				foreach (SongItem song in m_songList) {
					if (song.id == songId) {
						m_songTitleText.text = song.title;
						m_songArtistText.text = song.artist;
						m_songDurationText.text = song.duration;
						break;
					}
				}
			}
		}
	}
}