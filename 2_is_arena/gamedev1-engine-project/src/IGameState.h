#pragma once
#include "stdafx.h"
#include "GameStateManager.h"
class IGameState
{
public:
	virtual void init(sf::RenderWindow& window) = 0;
	virtual void update(float delta, sf::RenderWindow& window) = 0;
	virtual void render(sf::RenderWindow &window) = 0;
	virtual void handleEvents(sf::Event event) = 0;

	virtual ~IGameState()
	{

	}
};