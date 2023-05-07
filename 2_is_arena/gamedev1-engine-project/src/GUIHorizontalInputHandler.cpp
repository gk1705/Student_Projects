#include "stdafx.h"
#pragma once
#include "GUIHorizontalInputHandler.h"
#include "InputManager.h"

void GUIHorizontalInputHandler::init()
{
	m_buttons.resize(2);
	m_currentfocus.push_back(0);
	m_currentfocus.push_back(0);
	m_buttondown.push_back(false);
	m_buttondown.push_back(false);
}

void GUIHorizontalInputHandler::terminate()
{
	m_buttons.clear();
	m_currentfocus.clear();
	m_buttondown.clear();
}

void GUIHorizontalInputHandler::trackHorizontalInput()
{
	const int i = m_playerIdx;

	//Move Right
	sf::Vector2f vec = InputManager::getInstance().getLeftJoystickWithReset(i + 1);
	if (vec.x > 0) {

		if (m_currentfocus[i] + 1 < m_buttons[i].size()) {
			m_currentfocus[i]++;
		}
	}

	//Move Left
	if (vec.x < 0) {
		if (m_currentfocus[i] != 0) {
			m_currentfocus[i]--;
		}
	}
}

void GUIHorizontalInputHandler::pollFocusEvents(const sf::Vector2f& scale)
{
	const int i = m_playerIdx;

	sf::Event event;
	event.type = sf::Event::MouseMoved;
	event.mouseMove.x = (m_buttons[i][m_currentfocus[i]]->getPosition().x + 10) * scale.x;
	event.mouseMove.y = (m_buttons[i][m_currentfocus[i]]->getPosition().y + 10) * scale.y;

	m_gui.handleEvent(event);
}

void GUIHorizontalInputHandler::handleClickEvent(const sf::Vector2f& scale)
{
	const int i = m_playerIdx;

	if (InputManager::getInstance().IsButtonPressed("A", i + 1)) {
		sf::Event event2;
		event2.mouseButton.x = (m_buttons[i][m_currentfocus[i]]->getPosition().x + 10) * scale.x;
		event2.mouseButton.y = (m_buttons[i][m_currentfocus[i]]->getPosition().y + 10) * scale.y;
		event2.type = Event::MouseButtonPressed;
		event2.mouseButton.button = sf::Mouse::Left;
		m_gui.handleEvent(event2);
		m_buttondown[i] = true;
	}

	if (m_buttondown[i] == true && !InputManager::getInstance().IsButtonPressed("A", 1)) {
		sf::Event event3;
		event3.mouseButton.x = (m_buttons[i][m_currentfocus[i]]->getPosition().x + 10) * scale.x;
		event3.mouseButton.y = (m_buttons[i][m_currentfocus[i]]->getPosition().y + 10) * scale.y;
		event3.type = Event::MouseButtonReleased;
		event3.mouseButton.button = sf::Mouse::Left;
		m_gui.handleEvent(event3);
		m_buttondown[i] = false;
	}
}
