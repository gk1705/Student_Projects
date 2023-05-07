using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CaravanCrashChaos;
using Rewired;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{

	[SerializeField] private MenuButtonController menuButtonController;
	[SerializeField] private Animator animator;
	[SerializeField] private int thisIndex;
	[SerializeField] private UnityEvent buttonEvent;

	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip onPressSound;

	[SerializeField] private GameMode gameMode;
	[TextArea]
	[SerializeField] private string description;
	private Text text;

	private List<Rewired.Player> players;


	void Start()
	{
		players = ReInput.players.GetPlayers().ToList();
		text = GetComponentInChildren<Text>();
		if(text!=null && gameMode != null)
			text.text = gameMode.Name;
	}

	void Update()
    {
	    if (menuButtonController.index == thisIndex)
	    {
			animator.SetBool("selected", true);

		    if (!string.IsNullOrEmpty(description))
		    {
				menuButtonController.SetDescription(description);
		    }
		    else
		    {
				menuButtonController.DisableDescription();
		    }

		    for (int i = 0; i < players.Count; i++)
		    {
				if (players[i].GetButtonDown("UISubmit"))
				{
					animator.SetBool("pressed", true);
					audioSource.PlayOneShot(onPressSound);
					buttonEvent.Invoke();
				}
				else if (animator.GetBool("pressed"))
				{
					animator.SetBool("pressed", false);
				}
			}		   
	    }
	    else
	    {
			animator.SetBool("selected", false);
	    }
    }
}
