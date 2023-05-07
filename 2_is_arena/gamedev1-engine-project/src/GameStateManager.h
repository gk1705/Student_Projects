#pragma once

#include <map>
#include <string>
class IGameState;

class GameStateManager
{
public:
	static GameStateManager& getInstance();

	void update(float delta, sf::RenderWindow& window) const;
	void render(sf::RenderWindow &window) const;
	void handleEvents(sf::Event event) const;
	void setState(const std::string& stateName);
	void registerState(std::string stateName, IGameState* state);
	void exit();
	IGameState* getState() const;

private:
	GameStateManager(void) = default;
	~GameStateManager(void) = default;
	GameStateManager(const GameStateManager& p) = delete;
	GameStateManager& operator=(GameStateManager const&) = delete;
	static GameStateManager *m_instance;

	std::map<std::string, IGameState*> states;
	IGameState* currentState;
	std::string currentStateName;
};
