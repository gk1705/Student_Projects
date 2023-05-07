// Game.cpp : Defines the entry point for the console application.
//
//
#include "stdafx.h"
#pragma once
#include <SFML/Graphics.hpp>
#include "Game.h"
#include "GameObject.h"
#include "MainState.h"
#include "InputManager.h"
#include "RenderManager.h"
#include "PhysicsManager.h"
#include "GameObjectManager.h"
#include "UpdateManager.h"
#include "AnimationManager.h"
#include "HeroSelectionState.h"
#include "PauseState.h"
#include "GameOverState.h"
#include "PlayFieldManager.h"
#include "SoundManager.h"

using namespace sf;
using namespace std;

bool pressed = false;

void Game::Run()
{
	if (!init())
		return;

	while (m_window.isOpen())
	{

		sf::Event event;
		while (m_window.pollEvent(event))
		{
			if (event.type == sf::Event::Closed)
				m_window.close();
			GameStateManager::getInstance().handleEvents(event);

		}

		update();
		draw();
		GameObjectManager::getInstance().deleteGameObjects();
	}
	
	shutdown();
}

bool Game::init()
{
	VideoMode vm(1920, 1080);
	m_window.create(vm, "SFML");
	m_window.setFramerateLimit(30);
	//m_window.setSize(Vector2u(1, 720));

	configurePlayerInputs();
	initGameStates();
	
	return true;
}

void Game::update()
{
	static Clock clock; //< starts the clock
	Time deltaTime = clock.restart();
	const float fDeltaTimeSeconds = deltaTime.asSeconds();

	GameStateManager::getInstance().update(fDeltaTimeSeconds, m_window);
}

void Game::draw()
{
	m_window.clear();

	// move camera pixel precisely to avoid render artifacts
	View view = m_window.getView();
	view.move(0.0f, 0.0f);
	m_window.setView(view);

	GameStateManager::getInstance().render(m_window);

	m_window.display();
}

void Game::shutdown()
{
	// release manager resources
	InputManager::getInstance().release();
	RenderManager::getInstance().release();
	PhysicsManager::getInstance().release();
	GameObjectManager::getInstance().release();
	UpdateManager::getInstance().release();
	AnimationManager::getInstance().release();
	GameStateManager::getInstance().exit();
	PlayFieldManager::getInstance().release();
	SoundManager::getInstance().release();
}

void Game::configurePlayerInputs() const
{
	// ***************** Create Inputs *****************
	//Player 1
	InputManager::getInstance().bind("moveUp", sf::Keyboard::W, 1);
	InputManager::getInstance().bind("moveDown", sf::Keyboard::S, 1);
	InputManager::getInstance().bind("moveLeft", sf::Keyboard::A, 1);
	InputManager::getInstance().bind("moveRight", sf::Keyboard::D, 1);

	/* Controller Buttons:
	0: A
	1: B
	2: X
	3: Y
	4: LB
	5: RB
	6: select
	7: start
	8: L axis b
	9: R axis b
	*/
	InputManager::getInstance().bindButton("A", 0, 1);
	InputManager::getInstance().bindButton("B", 1, 1);
	InputManager::getInstance().bindButton("X", 2, 1);
	InputManager::getInstance().bindButton("Y", 3, 1);
	InputManager::getInstance().bindButton("Start", 7, 1);
	InputManager::getInstance().bindButton("LB", 4, 1);
	InputManager::getInstance().bindButton("RB", 5, 1);

	//Player 2
	InputManager::getInstance().bind("moveUp", sf::Keyboard::Up, 2);
	InputManager::getInstance().bind("moveDown", sf::Keyboard::Down, 2);
	InputManager::getInstance().bind("moveLeft", sf::Keyboard::Left, 2);
	InputManager::getInstance().bind("moveRight", sf::Keyboard::Right, 2);

	InputManager::getInstance().bindButton("A", 0, 2);
	InputManager::getInstance().bindButton("B", 1, 2);
	InputManager::getInstance().bindButton("X", 2, 2);
	InputManager::getInstance().bindButton("Y", 3, 2);
	InputManager::getInstance().bindButton("Start", 7, 2);
	InputManager::getInstance().bindButton("LB", 4, 2);
	InputManager::getInstance().bindButton("RB", 5, 2);
}

void Game::initGameStates()
{
	// ***************** Create Gamestates *****************
	IGameState* mainstate = new MainState();
	IGameState* heroSelec = new HeroSelectionState();
	IGameState* menustate = new MenuState();
	IGameState* pausestate = new PauseState();
	IGameState* gameOverState = new GameOverState();
	menustate->init(m_window);
	pausestate->init(m_window);
	heroSelec->init(m_window);
	gameOverState->init(m_window);
	GameStateManager::getInstance().registerState("Game", mainstate);
	GameStateManager::getInstance().registerState("Menu", menustate);
	GameStateManager::getInstance().registerState("HeroSelection", heroSelec);
	GameStateManager::getInstance().registerState("PauseMenu", pausestate);
	GameStateManager::getInstance().registerState("Game", mainstate);
	GameStateManager::getInstance().registerState("GameOver", gameOverState);
	GameStateManager::getInstance().setState("Menu");
}