using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace BoogieDownGames {

	public class SongItem {
		public string id;        // unique id that refers to the AudioClip of the sound file
		public string title;     // Full title of the song
		public string artist;    // Song artist attribution
		public string duration;  // length of song in seconds
		public string icon;      // Image asset reference to show (album art)
		public bool paid;        // true indicates this is a locked song can be unlocked by paying for it. false for free songs.

		public SongItem (string _id, string _title, string _artist, string _duration, string _icon, bool _paid) {
			id = _id;
			title = _title;
			artist = _artist;
			duration = _duration;
			icon = _icon;
			paid = _paid;
		}
	}

	// TODO: TextMachine is completely misnamed, this class is only responsible for managing Song display information in the UI.

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
		private List<AudioClip> m_soundClips;

		private List<SongItem> m_songList;


		void Awake () {
			m_songList = new List<SongItem>();
			m_songList.Add (new SongItem ("song1", "I'm Sexy Now", "Sky Silver & the Sky Girls", "2:00", "", true));
			m_songList.Add (new SongItem ("song2", "She Smiles", "Sky Silver", "2:00", "", true));
			m_songList.Add (new SongItem ("song3", "I Got You", "Sky Girl Jo", "2:07", "", false));
			m_songList.Add (new SongItem ("song4", "Alone Tonight", "Dani", "1:30", "", false));
			m_songList.Add (new SongItem ("song5", "It's Filth", "Sky Silver", "1:44", "", false));
			m_songList.Add (new SongItem ("song6", "Fry & Sizzle", "Delsa & Sky Silver", "1:46", "", false));
			m_songList.Add (new SongItem ("song7", "Shut Up & Dance", "Delsa Feat. Ai Man", "1:31", "", true));
			m_songList.Add (new SongItem ("song8", "She Said", "Dani Feat. Sky Silver", "1:53", "", false));
			m_songList.Add (new SongItem ("song9", "In Fashion", "Sky Silver", "1:33", "", false));
			m_songList.Add (new SongItem ("song10", "Lay With Me", "Sky Silver", "1:33", "", true));
			m_songList.Add (new SongItem ("song11", "You Have It", "Sky Silver", "1:33", "", true));

			NotificationCenter.DefaultCenter.AddObserver(this, "SetSong");
		}
		
		public void SetSong (NotificationCenter.Notification notification) {
			SetSongInfo ((string)notification.data["songid"], (float)notification.data["length"]);
		}

		private void SetSongInfo (string songId, float duration) {
			if (songId != null) {
				foreach (SongItem song in m_songList) {
					if (song.id == songId) {
						m_songTitleText.text = song.title;
						m_songArtistText.text = song.artist;
						m_songDurationText.text = ConvertSecondsToMMSS(duration);
						break;
					}
				}
			}
		}

		private string ConvertSecondsToMMSS (float duration)
		{
			string result;
			int seconds = (int)Mathf.RoundToInt (duration);
			int minutes = seconds / 60;
			seconds = seconds % 60;

			result = string.Format("{0}:{1:00}", minutes, seconds);
			return result;
		}
	}
}