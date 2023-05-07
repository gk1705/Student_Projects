#include "stdafx.h"
#pragma once
#include "GUIVerticalInputHandler.h"
#include "InputManager.h"

void GUIVerticalInputHandler::trackVerticalInput()
{
	//Move Down
	const sf::Vector2f vec = InputManager::getInstance().getLeftJoystickWithReset(1);
	if (vec.y > 0) {

		if (m_currentfocus + 1 < m_buttons.size()) {
			m_currentfocus++;
		}
	}

	//Move Up
	if (vec.y < 0) {
		if (m_currentfocus != 0) {
			m_currentfocus--;
		}
	}
}

void GUIVerticalInputHandler::pollFocusEvents(const sf::Vector2f& scale)
{
	sf::Event event;
	event.type = Event::MouseMoved;
	event.mouseMove.x = (m_buttons[m_currentfocus]->getPosition().x + 10) * scale.x;
	event.mouseMove.y = (m_buttons[m_currentfocus]->getPosition().y + 10) * scale.y;

	m_gui.handleEvent(event);
}

void GUIVerticalInputHandler::handleClickEvent(const sf::Vector2f& scale)
{
	if (InputManager::getInstance().IsButtonPressed("A", 1)) { //Only Player 1 can start
		sf::Event event2;
		event2.mouseButton.x = (m_buttons[m_currentfocus]->getPosition().x + 10) * scale.x;
		event2.mouseButton.y = (m_buttons[m_currentfocus]->getPosition().y + 10) * scale.y;
		event2.type = Event::MouseButtonPressed;
		event2.mouseButton.button = sf::Mouse::Left;
		m_gui.handleEvent(event2);
		m_buttondown = true;
	}

	if (m_buttondown == true && !InputManager::getInstance().IsButtonPressed("A", 1)) {
		sf::Event event3;
		event3.mouseButton.x = (m_buttons[m_currentfocus]->getPosition().x + 10) * scale.x;
		event3.mouseButton.y = (m_buttons[m_currentfocus]->getPosition().y + 10) * scale.y;
		event3.type = Event::MouseButtonReleased;
		event3.mouseButton.button = sf::Mouse::Left;
		m_gui.handleEvent(event3);
		m_buttondown = false;
	}
}