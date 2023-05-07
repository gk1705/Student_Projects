#pragma once

#include <SFML/Graphics.hpp>
#include <string>
#include "MenuState.h"

class Game
{
public:
	void Run();

private:
	bool init();
	void update();
	void draw();
	void shutdown();

	void configurePlayerInputs() const;
	void initGameStates();

	const std::string m_resourcePath = "assets/";
	sf::RenderWindow m_window;
};