#include "stdafx.h"
#include "GameStateManager.h"
#include "IGameState.h"

GameStateManager& GameStateManager::getInstance() {
	if (m_instance == nullptr)
		m_instance = new GameStateManager();
	return *m_instance;
}

GameStateManager* GameStateManager::m_instance = nullptr;

void GameStateManager::setState(const std::string& stateName)
{
	currentState = states[stateName];
	currentStateName = stateName;
}

void GameStateManager::registerState(std::string stateName, IGameState* state)
{
	states.insert({ stateName, state });
}

void GameStateManager::update(float delta, sf::RenderWindow& window)const {

	currentState->update(delta, window);
}

void GameStateManager::render(sf::RenderWindow &window) const {
	currentState->render(window);
}

void GameStateManager::handleEvents(sf::Event event) const
{
	currentState->handleEvents(event);
}

IGameState* GameStateManager::getState() const {
	return currentState;
}

void GameStateManager::exit() {
	for (auto& state : states) {
		delete state.second;
	}

	if (m_instance != nullptr)
		delete m_instance;
	m_instance = nullptr;
}