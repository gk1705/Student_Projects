#pragma once
#include "IGameState.h"
#include "GameObject.h"
#include "TGUI\TGUI.hpp"
#include "GUIVerticalInputHandler.h"

class GameOverState : public IGameState
{
public:
	void init(sf::RenderWindow &window) override;
	void update(float delta, sf::RenderWindow& window) override;
	void render(sf::RenderWindow &window) override;
	void handleEvents(sf::Event event) override;

private:
	vector<shared_ptr<GameObject>> gameObjects;
	tgui::Gui m_gui;
	shared_ptr<tgui::Label> m_scoretext;
	shared_ptr<GUIVerticalInputHandler> m_inputHandler;
};
