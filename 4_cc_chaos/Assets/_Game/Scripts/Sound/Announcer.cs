//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CaravanCrashChaos;
using Exploder;
using UnityEngine;

namespace CaravanCrashChaos
{
	/// <summary>
	/// Singleton with three main functions:
	/// - queue voice lines
	/// - play voice lines concurrently
	/// - stop all active voice lines to play another one
	/// </summary>
	public class Announcer : Singleton<Announcer>
	{
		private Dictionary<string, AnnouncerVoiceLines> announcerVoiceLines;
		private Queue<VoiceLine> queuedVoiceLines;
		private List<VoiceLine> activeVoiceLines;

		/// <summary>
		/// Add voice line to a queue.
		/// Voice line is played after the ones added beforehand have finished playing.
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="delay"></param>
		public void EnqueueVoiceLine(string expression, float delay = 0)
		{
			var soundClip = GetAnnouncerSoundClip(expression);
			queuedVoiceLines.Enqueue(new VoiceLine(soundClip, delay));
		}

		/// <summary>
		/// Voice line is played while other voice lines are playing.
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="delay"></param>
		public void PlayVoiceLineConcurrently(string expression, float delay = 0)
		{
			var soundClip = GetAnnouncerSoundClip(expression);
			soundClip?.Play();
		}

		/// <summary>
		/// All active voice lines are stopping in favour of this one being played.
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="delay"></param>
		public void ForceVoiceLine(string expression, float delay = 0)
		{
			StopRemoveAllVoiceLines();
			var soundClip = GetAnnouncerSoundClip(expression);
			var voiceLine = new VoiceLine(soundClip, delay);
			activeVoiceLines.Add(voiceLine);
			voiceLine.Play();
		}

		/// <summary>
		/// Stops all voice lines from playing and flushes the queue.
		/// </summary>
		public void StopVoiceLines()
		{
			StopRemoveAllVoiceLines();
		}

		private void Awake()
		{
			announcerVoiceLines = new Dictionary<string, AnnouncerVoiceLines>();
			queuedVoiceLines = new Queue<VoiceLine>();
			activeVoiceLines = new List<VoiceLine>();

			LoadAnnouncerVoiceLines();
		}

		private void Update()
		{
			CheckQueueVoiceLines();
			CheckRemoveFinishedVoiceLines();
		}

		/// <summary>
		/// Are voice lines queued and are no active voice lines playing?
		/// In that case we dequeue a voice line and play it.
		/// </summary>
		private void CheckQueueVoiceLines()
		{
			if (queuedVoiceLines.Count > 0 && !IsActiveVoiceLinePlaying())
			{
				var voiceLine = queuedVoiceLines.Dequeue();
				activeVoiceLines.Add(voiceLine);
				voiceLine.Play();
			}
		}

		/// <summary>
		/// Active voice lines that have finished playing are removed.
		/// </summary>
		private void CheckRemoveFinishedVoiceLines()
		{
			foreach (var activeVoiceLine in activeVoiceLines.ToList())
			{
				if (!activeVoiceLine.IsPlaying())
					activeVoiceLines.Remove(activeVoiceLine);
			}
		}

		/// <summary>
		/// Checks if atleast one active voice line is currently playing.
		/// </summary>
		/// <returns></returns>
		private bool IsActiveVoiceLinePlaying()
		{
			foreach (var activeVoiceLine in activeVoiceLines)
			{
				if (activeVoiceLine.IsPlaying())
					return true;
			}

			return false;
		}

		/// <summary>
		/// All voice lines are stopped and the list of active voice lines is cleared.
		/// </summary>
		private void StopRemoveAllVoiceLines()
		{
			foreach (var activeVoiceLine in activeVoiceLines)
			{
				activeVoiceLine.Stop();
			}

			activeVoiceLines.Clear();
			queuedVoiceLines.Clear();
		}

		/// <summary>
		/// Query announcer voice lines scriptable objects
		/// to get the appropriate soundclip based on the passed expression.
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		private SoundClip GetAnnouncerSoundClip(string expression)
		{
			var announcerVoiceLine = announcerVoiceLines[expression];
			return announcerVoiceLine.GetVoiceLine(expression);
		}

		/// <summary>
		/// Loading all scriptable objects in the Resource/AnnouncerVoiceLines folder
		/// Then adding the voice line to a dictionary:
		/// KEY: Filename without "Lines", "TakedownLines" -> "Takedown"
		/// </summary>
		private void LoadAnnouncerVoiceLines()
		{
			var voiceLineResources = Resources.LoadAll("AnnouncerVoiceLines", typeof(AnnouncerVoiceLines));
			foreach (var voiceLineResource in voiceLineResources)
			{
				announcerVoiceLines.Add(voiceLineResource.name.Replace("Lines", ""), (AnnouncerVoiceLines) voiceLineResource);
				((AnnouncerVoiceLines) voiceLineResource).Setup(gameObject);
			}
		}
	}
}
