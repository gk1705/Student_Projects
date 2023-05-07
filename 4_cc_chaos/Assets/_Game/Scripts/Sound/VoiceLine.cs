//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	class VoiceLine
	{
		public VoiceLine(SoundClip soundClip, float delay)
		{
			this.soundClip = soundClip;
			this.delay = delay;
		}

		public bool IsPlaying()
		{
			if (soundClip == null) return false;
			return soundClip.IsPlaying();
		}

		public void Stop()
		{
			soundClip?.Stop();
		}

		public void Play()
		{
			soundClip?.Play(delay);
		}

		public readonly SoundClip soundClip;
		public readonly float delay;
	}
}
