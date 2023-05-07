#pragma once
#include "IGameState.h"
#include "GameObject.h"
#include "TGUI\TGUI.hpp"
#include "GUIHorizontalInputHandler.h"

class HeroSelectionState : public IGameState
{
public:
	void init(sf::RenderWindow &window) override;
	void update(float delta, sf::RenderWindow& window) override;
	void render(sf::RenderWindow &window) override;
	void handleEvents(sf::Event event) override;

private:
	void transitionState(sf::RenderWindow& window);

	vector<shared_ptr<GameObject>> gameObjects;
	vector<shared_ptr<GameObject>> m_images;
	tgui::Gui m_gui;

	shared_ptr<GUIHorizontalInputHandler> m_inputHandler;

	bool m_p1Ready = false;
	bool m_p2Ready = false;
	string m_hero1;
	string m_hero2;

	void SetP1Ready(string hero);
	void SetP2Ready(string hero);
	void clearState(sf::RenderWindow& window);
	void switchState(sf::RenderWindow& window) const;
};
