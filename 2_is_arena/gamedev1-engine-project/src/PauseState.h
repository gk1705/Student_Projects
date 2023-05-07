#pragma once
#include "IGameState.h"
#include "GameObject.h"
#include "TGUI\TGUI.hpp"
#include "GUIVerticalInputHandler.h"

class PauseState : public IGameState
{
public:
	void init(sf::RenderWindow &window) override;
	void update(float delta, sf::RenderWindow& window) override;
	void render(sf::RenderWindow &window) override;
	void handleEvents(sf::Event event) override;

private:
	tgui::Gui m_gui;
	vector<shared_ptr<GameObject>> gameObjects;
	shared_ptr<GUIVerticalInputHandler> m_inputHandler;
};
