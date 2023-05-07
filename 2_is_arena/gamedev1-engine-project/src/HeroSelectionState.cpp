#include "stdafx.h"
#pragma once;
#include "HeroSelectionState.h"
#include "SpriteComponent.h"
#include "GameStateManager.h"
#include "InputManager.h"
#include "MainState.h"

void HeroSelectionState::init(sf::RenderWindow& window) {

#pragma region GUISetup
	m_inputHandler = make_shared<GUIHorizontalInputHandler>(GUIHorizontalInputHandler(m_gui));
	m_inputHandler->init();

	//Create Menu-Object
	shared_ptr<GameObject> menu = make_shared<GameObject>("Menu", sf::Vector2f(0, 0));
	shared_ptr<SpriteComponent> spritecomp = make_shared<SpriteComponent>(*menu, "../assets/GUI/heroselection.png");
	menu->AddRenderComponent(spritecomp);
	gameObjects.push_back(menu);

	//Create Player1 Character-Field
	shared_ptr<GameObject> p1charfield = make_shared<GameObject>("P1CharacterField", sf::Vector2f(200, 250));
	p1charfield->AddRenderComponent(make_shared<SpriteComponent>(*p1charfield, "../assets/GUI/menu.png"));
	gameObjects.push_back(p1charfield);

	//Create Player2 Character-Field
	shared_ptr<GameObject> p2charfield = make_shared<GameObject>("P2CharacterField", sf::Vector2f(p1charfield->GetPosition().x + p1charfield->getWidth() - 60, 250));
	p1charfield->AddRenderComponent(make_shared<SpriteComponent>(*p2charfield, "../assets/GUI/menu.png"));
	gameObjects.push_back(p2charfield);

	// initialize gui
	m_gui.setWindow(window);
	m_gui.setFont("../assets/GUI/arial.ttf");

	//add Buttons for Player 1
	auto theme = make_shared<tgui::Theme>("../assets/GUI/GUI_2.txt");
	tgui::Button::Ptr p1Hero1 = theme->load("Button");
	p1Hero1->setSize(275, 275);
	p1Hero1->setPosition(p1charfield->GetPosition().x + 90 ,p1charfield->GetPosition().y + 220);//50
	p1Hero1->setText("");
	p1Hero1->setTextSize(80);
	m_gui.add(p1Hero1);
	p1Hero1->connect("Clicked", &HeroSelectionState::SetP1Ready, this, "Archer");
	m_inputHandler->AddButton(0, p1Hero1);

	tgui::Button::Ptr p1Hero2 = theme->load("Button");
	p1Hero2->setSize(275, 275);
	p1Hero2->setPosition(tgui::bindPosition(p1Hero1) + Vector2f(p1Hero1->getSize().x + 70, 0));
	p1Hero2->setText("");
	p1Hero2->setTextSize(80);
	m_gui.add(p1Hero2);
	p1Hero2->connect("Clicked", &HeroSelectionState::SetP1Ready, this, "Witch");
	m_inputHandler->AddButton(0, p1Hero2);

	//add Buttons for Player 2
	tgui::Button::Ptr p2Hero1 = theme->load("Button");
	p2Hero1->setSize(275, 275);
	p2Hero1->setPosition(p2charfield->GetPosition().x + 90, p2charfield->GetPosition().y + 220);//50
	p2Hero1->setText("");
	p2Hero1->setTextSize(80);
	m_gui.add(p2Hero1);
	p2Hero1->connect("Clicked", &HeroSelectionState::SetP2Ready, this, "Archer");
	m_inputHandler->AddButton(1, p2Hero1);

	tgui::Button::Ptr p2Hero2 = theme->load("Button");
	p2Hero2->setSize(275, 275);
	p2Hero2->setPosition(tgui::bindPosition(p2Hero1) + Vector2f(p1Hero1->getSize().x + 70, 0));
	p2Hero2->setText("");
	p2Hero2->setTextSize(80);
	m_gui.add(p2Hero2);
	p2Hero2->connect("Clicked", &HeroSelectionState::SetP2Ready, this, "Witch");
	m_inputHandler->AddButton(1, p2Hero2);

	//Add Image over Button
	//Player 1
	shared_ptr<GameObject> p1archer = make_shared<GameObject>("P1Archer", sf::Vector2f(p1Hero1->getPosition().x+80, p1Hero1->getPosition().y+45));
	auto p1archersprite = (make_shared<SpriteComponent>(*p1archer, "../assets/GUI/archer.png"));
	p1archersprite->SetScale(Vector2f(0.65f, 0.65f));
	p1archer->AddRenderComponent(p1archersprite);
	m_images.push_back(p1archer);

	shared_ptr<GameObject> p1witch = make_shared<GameObject>("P1Witch", sf::Vector2f(p1Hero2->getPosition().x + 80, p1Hero2->getPosition().y + 45));
	auto p1witchsprite = (make_shared<SpriteComponent>(*p1witch, "../assets/GUI/witch.png"));
	p1witchsprite->SetScale(Vector2f(0.65f, 0.65f));
	p1witch->AddRenderComponent(p1witchsprite);
	m_images.push_back(p1witch);

	//Player2
	shared_ptr<GameObject> p2archer = make_shared<GameObject>("P2Archer", sf::Vector2f(p2Hero1->getPosition().x + 80, p2Hero1->getPosition().y + 45));
	auto p2archersprite = (make_shared<SpriteComponent>(*p2archer, "../assets/GUI/archer.png"));
	p2archersprite->SetScale(Vector2f(0.65f, 0.65f));
	p2archer->AddRenderComponent(p2archersprite);
	m_images.push_back(p2archer);

	shared_ptr<GameObject> p2witch = make_shared<GameObject>("P1Witch", sf::Vector2f(p2Hero2->getPosition().x + 80, p2Hero2->getPosition().y + 45));
	auto p2witchsprite = (make_shared<SpriteComponent>(*p2witch, "../assets/GUI/witch.png"));
	p2witchsprite->SetScale(Vector2f(0.65f, 0.65f));
	p2witch->AddRenderComponent(p2witchsprite);
	m_images.push_back(p2witch);
#pragma endregion

}

void HeroSelectionState::SetP1Ready(string hero)
{
	m_hero1 = hero;
	m_p1Ready = true;
}

void HeroSelectionState::SetP2Ready(string hero)
{
	m_hero2 = hero;
	m_p2Ready = true;
}

void HeroSelectionState::clearState(sf::RenderWindow& window)
{
	gameObjects.clear();
	m_images.clear();
	m_gui.removeAllWidgets();
	m_inputHandler->terminate();
	m_p1Ready = false;
	m_p2Ready = false;
	init(window);
}

void HeroSelectionState::switchState(sf::RenderWindow& window) const
{
	GameStateManager::getInstance().setState("Game");
	IGameState* gameState = GameStateManager::getInstance().getState();
	dynamic_cast<MainState*>(gameState)->SetHeros(m_hero1, m_hero2);		
	gameState->init(window);
}

void HeroSelectionState::transitionState(sf::RenderWindow& window)
{
	clearState(window);
	switchState(window);
}

void HeroSelectionState::update(float delta, sf::RenderWindow& window) {
	if (m_p1Ready && m_p2Ready) {
		transitionState(window);
	}
	else {
		const int playerIdx = m_p1Ready ? 1 : 0;
		m_inputHandler->Update(window, playerIdx);
	}
}

void HeroSelectionState::render(sf::RenderWindow &window) {
	for (auto& obj : gameObjects) {
		obj->Render(window);
	}

	m_gui.draw();

	for (auto& obj2 : m_images) {
		obj2->Render(window);
	}
}

void HeroSelectionState::handleEvents(sf::Event event)
{
	m_gui.handleEvent(event);
}
