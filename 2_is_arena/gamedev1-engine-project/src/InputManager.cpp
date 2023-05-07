#include "stdafx.h"
#include "InputManager.h"

InputManager* InputManager::m_instance = nullptr;

InputManager& InputManager::getInstance() {
	if (m_instance == nullptr)
		m_instance = new InputManager();
	return *m_instance;
}

void InputManager::release() {
	if (m_instance != nullptr)
		delete m_instance;
	m_instance = nullptr;
}

bool InputManager::IsKeyPressed(const std::string& action, int playerIdx)
{
	const sf::Keyboard::Key key = player[playerIdx][action];
	return sf::Keyboard::isKeyPressed(key);
}

bool InputManager::IsButtonPressed(const std::string& action, int playerIdx)
{
	const auto keyExists = playerControlls[playerIdx].find(action);
	if (keyExists == playerControlls[playerIdx].end()) { //does not exist
		return false;
	}

	const int button = playerControlls[playerIdx][action];

	//-1 because Player 1 is Controller 0
	return sf::Joystick::isButtonPressed(playerIdx - 1, button);
}

void InputManager::bindButton(std::string action, int button, int playerIdx)
{
	if (playerControlls.size() < 4) {
		playerControlls.resize(4);
	}

	playerControlls[playerIdx].insert({ action, button });
}

sf::Vector2f InputManager::getLeftJoystickWithReset(int playerIdx)
{
	if (m_resetLeftJoystick.size() < 2) {
		m_resetLeftJoystick.resize(2);
		m_resetLeftJoystick[0] = false;
		m_resetLeftJoystick[1] = false;
	}

	sf::Vector2f position;
	position.x = sf::Joystick::getAxisPosition(playerIdx - 1, sf::Joystick::X); //-1 because Player 1 is Controller 0
	position.y = sf::Joystick::getAxisPosition(playerIdx - 1, sf::Joystick::Y); //-1 because Player 1 is Controller 0

	//Controller axis is never really on position 0.0
	const float length = sqrt((position.x * position.x) + (position.y * position.y));

	if (length < 40) {
		position.x = 0;
		position.y = 0;
		m_resetLeftJoystick[playerIdx - 1] = false;
	}
	else if (m_resetLeftJoystick[playerIdx - 1]) {
		position.x = 0;
		position.y = 0;
	}
	else {
		m_resetLeftJoystick[playerIdx - 1] = true;
	}

	return position;
}

sf::Vector2f InputManager::getLeftJoystick(int playerIdx) const
{
	sf::Vector2f position;
	position.x = sf::Joystick::getAxisPosition(playerIdx-1, sf::Joystick::X); //-1 because Player 1 is Controller 0
	position.y = sf::Joystick::getAxisPosition(playerIdx-1, sf::Joystick::Y); //-1 because Player 1 is Controller 0

	//Controller axis is never really on position 0.0
	const float length = sqrt((position.x * position.x) + (position.y * position.y));
	if (length < 40) {
		position.x = 0;
		position.y = 0;
	}

	return position;
}

sf::Vector2f InputManager::getRightJoystickWithReset(int playerIdx) {
	if (m_resetRightJoystick.size() < 2) {
		m_resetRightJoystick.resize(2);
		m_resetRightJoystick[0] = false;
		m_resetRightJoystick[1] = false;
	}

	sf::Vector2f position;
	position.x = sf::Joystick::getAxisPosition(playerIdx - 1, sf::Joystick::U); //-1 because Player 1 is Controller 0
	position.y = sf::Joystick::getAxisPosition(playerIdx - 1, sf::Joystick::R); //-1 because Player 1 is Controller 0

	//Controller axis is never really on position 0.0
	const float length = sqrt((position.x * position.x) + (position.y * position.y));

	if (length < 40) {
		position.x = 0;
		position.y = 0;
		m_resetRightJoystick[playerIdx - 1] = false;
	}
	else if (m_resetRightJoystick[playerIdx - 1]) {
		position.x = 0;
		position.y = 0;
	}
	else {
		m_resetRightJoystick[playerIdx - 1] = true;
	}

	return position;
}

sf::Vector2f InputManager::getRightJoystick(int playerIdx) const
{
	sf::Vector2f position;
	position.x = sf::Joystick::getAxisPosition(playerIdx - 1, sf::Joystick::U); //-1 because Player 1 is Controller 0
	position.y = sf::Joystick::getAxisPosition(playerIdx - 1, sf::Joystick::R); //-1 because Player 1 is Controller 0

	//Controller axis is never really on position 0.0
	const float length = sqrt((position.x * position.x) + (position.y * position.y));
	if (length < 40) {
		position.x = 0;
		position.y = 0;
	}

	return position;
}

void InputManager::bind(std::string action, sf::Keyboard::Key keyCode, int playerIdx)
{
	if (playerIdx >= player.size()) {
		player.resize(playerIdx*2);
	}

	player[playerIdx].insert({ action, keyCode });
}

void InputManager::unbind(const std::string& action, int playerIdx)
{
	player[playerIdx].erase(action);
}