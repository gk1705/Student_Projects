#include "stdafx.h"
#pragma once;
#include "PauseState.h"
#include "SpriteComponent.h"
#include "GameStateManager.h"
#include "InputManager.h"
#include "GameObjectManager.h"

void PauseState::init(sf::RenderWindow& window) {

#pragma region GUISetup
	m_inputHandler = make_shared<GUIVerticalInputHandler>(GUIVerticalInputHandler(m_gui));

	//Create Menu-Object
	//Load Image
	Image image;
	image.loadFromFile("../assets/GUI/menu.png");
	shared_ptr<Texture> texture = make_shared<Texture>();
	texture->loadFromImage(image);
	//Create Object
	shared_ptr<GameObject> menu = make_shared<GameObject>("Menu", sf::Vector2f(window.getSize().x / 2 - image.getSize().x / 2, window.getSize().y / 2 - image.getSize().y / 2));
	menu->AddRenderComponent({ make_shared<SpriteComponent>(*menu, texture) });
	gameObjects.push_back(menu);

	//Create Header-Object
	//Load Image
	Image header_image;
	header_image.loadFromFile("../assets/GUI/header.png");
	shared_ptr<Texture> header_texture = make_shared<Texture>();
	header_texture->loadFromImage(header_image);
	//Create Object
	shared_ptr<GameObject> header = make_shared<GameObject>("Header", sf::Vector2f(window.getSize().x / 2 - header_image.getSize().x / 2, window.getSize().y / 2 - header_image.getSize().y / 2 - image.getSize().y / 2));
	header->AddRenderComponent({ make_shared<SpriteComponent>(*header, header_texture) });
	gameObjects.push_back(header);

	// initialize gui
	m_gui.setWindow(window);
	m_gui.setFont("../assets/GUI/arial.ttf");

	//// add widgets. 
	auto theme = make_shared<tgui::Theme>("../assets/GUI/GUI.txt");
	tgui::Button::Ptr buttonPlay = theme->load("Button");
	buttonPlay->setSize(363, 178);
	buttonPlay->setPosition(window.getSize().x / 2 - buttonPlay->getSize().x / 2, menu->GetPosition().y + 120);
	buttonPlay->setText("Resume");
	buttonPlay->setTextSize(70);
	buttonPlay->connect("Clicked", [](void) { GameStateManager::getInstance().setState("Game"); });
	buttonPlay->focus();
	m_inputHandler->AddButton(buttonPlay);
	m_gui.add(buttonPlay);


	tgui::Button::Ptr buttonQuit = theme->load("Button");
	buttonQuit->setSize(363, 178);
	buttonQuit->setPosition(tgui::bindPosition(buttonPlay) + Vector2f(0, buttonPlay->getSize().y + 40));
	buttonQuit->setText("Back to Menu");
	buttonQuit->setTextSize(45);
	buttonQuit->connect("Clicked", [](void) {
		GameObjectManager::getInstance().reset();
		GameStateManager::getInstance().setState("Menu");
	});
	m_inputHandler->AddButton(buttonQuit);
	m_gui.add(buttonQuit);
#pragma endregion

}


void PauseState::update(float delta, sf::RenderWindow& window) {

	m_inputHandler->Update(window);
}

void PauseState::render(sf::RenderWindow &window) {
	for (auto& obj : gameObjects) {
		obj->Render(window);
	}

	m_gui.draw();
}

void PauseState::handleEvents(sf::Event event)
{
	m_gui.handleEvent(event);
}
