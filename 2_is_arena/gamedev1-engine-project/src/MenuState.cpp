#include "stdafx.h"
#pragma once;
#include "MenuState.h"
#include "SpriteComponent.h"
#include "GameStateManager.h"
#include "InputManager.h"

void MenuState::init(sf::RenderWindow& window) {

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
	menu->AddRenderComponent( { make_shared<SpriteComponent>(*menu, texture) } );
	m_gameObjects.push_back(menu);

	//Create Header-Object
	//Load Image
	Image header_image;
	header_image.loadFromFile("../assets/GUI/header.png");
	shared_ptr<Texture> header_texture = make_shared<Texture>();
	header_texture->loadFromImage(header_image);
	//Create Object
	shared_ptr<GameObject> header = make_shared<GameObject>("Header", sf::Vector2f(window.getSize().x / 2 - header_image.getSize().x / 2, window.getSize().y / 2 - header_image.getSize().y / 2 - image.getSize().y/2));
	header->AddRenderComponent({ make_shared<SpriteComponent>(*header, header_texture) });
	m_gameObjects.push_back(header);

	//// initialize gui
	m_gui.setWindow(window);
	m_gui.setFont("../assets/GUI/arial.ttf");

	// add widgets. 
	auto theme = make_shared<tgui::Theme>("../assets/GUI/GUI.txt");
	tgui::Button::Ptr buttonPlay = theme->load("Button");
	buttonPlay->setSize(363, 178);
	buttonPlay->setPosition(window.getSize().x / 2 - buttonPlay->getSize().x / 2, menu->GetPosition().y + 120);
	buttonPlay->setText("Play");
	buttonPlay->setTextSize(80);
	buttonPlay->connect("Clicked", [](void) { GameStateManager::getInstance().setState("HeroSelection"); });
	buttonPlay->focus();
	m_inputHandler->AddButton(buttonPlay);
	m_gui.add(buttonPlay);

	//Quit Button
	tgui::Button::Ptr buttonQuit = theme->load("Button");
	buttonQuit->setSize(363, 178);
	buttonQuit->setPosition(tgui::bindPosition(buttonPlay) +  Vector2f(0, buttonPlay->getSize().y + 40));
	buttonQuit->setText("Quit");
	buttonQuit->setTextSize(80);
	buttonQuit->connect("Clicked", [](void) { exit(0); });
	m_inputHandler->AddButton(buttonQuit);
	m_gui.add(buttonQuit);

	//FH-Logo
	shared_ptr<GameObject> fhlogo = make_shared<GameObject>("FHLogo", sf::Vector2f(0,0));
	shared_ptr<SpriteComponent> spritecomp = make_shared<SpriteComponent>(*fhlogo, "../assets/Images/fhlogo.png");
	spritecomp->SetScale(Vector2f(0.5, 0.5));
	header->AddRenderComponent({ spritecomp});
	m_gameObjects.push_back(fhlogo);
#pragma endregion

}

void MenuState::update(float delta, sf::RenderWindow& window) {

	m_inputHandler->Update(window);
}

void MenuState::render(sf::RenderWindow &window) {
	for (auto& obj : m_gameObjects) {
		obj->Render(window);
	}

	m_gui.draw();
}

void MenuState::handleEvents(sf::Event event)
{
	m_gui.handleEvent(event);
}
