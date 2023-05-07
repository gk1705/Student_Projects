//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	/// <summary>
	/// A generic ringbuffer that reserves its indices
	/// based on a given occupier index.
	/// When iterating - both foreward- and backward-iteration are possible -
	/// indices are also freed based on the given index.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class OccupiableRingBuffer<T>
	{
		private const int free = -1;

		private List<T> collection;
		private List<int> possession;

		public OccupiableRingBuffer()
		{
			collection = new List<T>();
			possession = new List<int>();
		}

		public void Add(T element)
		{
			collection.Add(element);
			possession.Add(free);
		}

		public T GetNextUp(int occupierId)
		{
			int startIndex = GetOcccupierPossessionIndex(occupierId);

			int currentNumLoops = 0;
			int maximumNumLoops = possession.Count;
			for (int currentIndex = startIndex; currentNumLoops != maximumNumLoops; currentNumLoops++, IterateIndexUp(ref currentIndex))
			{
				if (IsFree(currentIndex))
				{
					if (HasPossession(occupierId))
					{
						possession[startIndex] = free;
					}

					possession[currentIndex] = occupierId;
					return collection[currentIndex];
				}
			}

			throw new InvalidOperationException("Collection is fully occupied, and therefore there is no way to get another element.");
		}

		public T GetNextDown(int occupierId)
		{
			var startIndex = GetOcccupierPossessionIndex(occupierId);

			int currentNumLoops = 0;
			int maximumNumLoops = possession.Count;
			for (int currentIndex = startIndex; currentNumLoops != maximumNumLoops; currentNumLoops++, IterateIndexDown(ref currentIndex))
			{
				if (IsFree(currentIndex))
				{
					if (HasPossession(occupierId))
					{
						possession[startIndex] = free;
					}

					possession[currentIndex] = occupierId;
					return collection[currentIndex];
				}
			}

			throw new InvalidOperationException("Collection is fully occupied, and therefore there is no way to get another element.");
		}

		public void FreePossession(int occupierId)
		{
			if (HasPossession(occupierId))
			{
				var index = possession.FindIndex(i => i == occupierId);
				possession[index] = free;
			}

			Debug.Log("Couldn't free possession due to not having one.");
		}

		private bool IsFree(int idx)
		{
			return possession[idx] == free;
		}

		private bool HasPossession(int occupierId)
		{
			return possession.Contains(occupierId);
		}

		private int GetOcccupierPossessionIndex(int occupierId)
		{
			if (HasPossession(occupierId))
			{
				return possession.FindIndex(i => i == occupierId);
			}

			return 0; // not found
		}

		private void IterateIndexUp(ref int index)
		{
			index = (index + 1) % possession.Count;
		}

		private void IterateIndexDown(ref int index)
		{
			index = (index + possession.Count - 1) % possession.Count;
		}
	}
}
