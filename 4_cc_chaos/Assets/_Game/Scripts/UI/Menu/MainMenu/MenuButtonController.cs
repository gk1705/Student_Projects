using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonController : MonoBehaviour
{
	[SerializeField] private int maxIndex;
	[SerializeField] private int maxIndexStart = 2;
	[SerializeField] private int maxIndexGamemode = 4;
	[SerializeField] private int maxIndexCredits = 0;
	[SerializeField] private int maxIndexTournament = 3;
	[SerializeField] private bool horizontal = false;

	[SerializeField] private AudioClip menuNavigation;

	[SerializeField] private GameObject StartMenu;
	[SerializeField] private GameObject GameModeMenu;
	[SerializeField] private GameObject CreditsMenu;
	[SerializeField] private GameObject TournamentMenu;

	[SerializeField] private GameObject DescriptionBox;
	[SerializeField] private Text DescriptionText;

	public AudioSource audioSource;
	public int index;
	private bool keyDown;
	private List<Rewired.Player> players;
	private List<GameObject> menus;
	private bool[] playerKeyDown;


	void Start()
    {
	    audioSource = GetComponent<AudioSource>();
	    players = ReInput.players.GetPlayers().ToList();
		playerKeyDown = new bool[players.Count];
		StartMenu.SetActive(true);
		if (GameModeMenu != null) 
			GameModeMenu.SetActive(false);
		if (CreditsMenu != null)
			CreditsMenu.SetActive(false);

		SetupMenuList();
	}

	private void SetupMenuList()
	{
		menus = new List<GameObject>();
		menus.Add(StartMenu);
		menus.Add(GameModeMenu);
		menus.Add(CreditsMenu);
		menus.Add(TournamentMenu);
	}

    void Update()
    {
		HandleMenu();	   
	}

	private void HandleMenu()
	{
		for (int i = 0; i < players.Count; i++)
		{
			float axisValue = horizontal ? players[i].GetAxisRaw("UIHorizontal") : players[i].GetAxisRaw("UIVertical");

			if (Math.Abs(axisValue) > 0)
			{
				if (!playerKeyDown[i])
				{
					if (axisValue < 0)
					{
						if (index < maxIndex)
						{
							index++;
						}
						else
						{
							index = 0;
						}
					}
					else if (axisValue > 0)
					{
						if (index > 0)
						{
							index--;
						}
						else
						{
							index = maxIndex;
						}
					}

					audioSource.PlayOneShot(menuNavigation);
					playerKeyDown[i] = true;
				}
			}
			else
			{
				playerKeyDown[i] = false;
			}
		}		
	}

	public void SetDescription(string text)
	{
		DescriptionBox.SetActive(true);
		DescriptionText.text = text;
	}

	public void DisableDescription()
	{
		if(DescriptionBox != null)
			DescriptionBox.SetActive(false);
	}

	private void ActivateMenu(GameObject menu)
	{
		index = 0;

		// deactivate all menus
		foreach (GameObject item in menus)
		{
			item.SetActive(false);
		}

		menu.SetActive(true);
	}

	public void SwitchToStartMenu()
	{
		maxIndex = maxIndexStart;
		ActivateMenu(StartMenu);
	}

	public void SwitchToGamemodeMenu()
	{
		maxIndex = maxIndexGamemode;
		ActivateMenu(GameModeMenu);
	}

	public void SwitchToCreditsMenu()
	{
		maxIndex = maxIndexCredits;
		ActivateMenu(CreditsMenu);
	}

	public void SwitchToTournamentMenu()
	{
		maxIndex = maxIndexTournament;
		ActivateMenu(TournamentMenu);
	}
}
