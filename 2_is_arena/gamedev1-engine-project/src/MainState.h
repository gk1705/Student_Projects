#pragma once
#include "IGameState.h"
#include <vector>
#include "GameObject.h"
#include "TileMap.h"
#include "HeroLoader.h"
#include "TGUI/TGUI.hpp"

class MainState : public IGameState
{
public:
	void initGUI(sf::RenderWindow& window);
	void initTileMap(sf::RenderWindow& window);
	void initEnemies() const;
	void initMonsterSpawnTime();
	void init(sf::RenderWindow& window) override;
	void createGUI();
	void update(float delta, sf::RenderWindow& window) override;
	void render(sf::RenderWindow& window) override;
	void handleEvents(sf::Event event);

	void SetHeros(string Hero1, string Hero2);


	struct SpawnData {

		SpawnData(int spawnTime, string playfield, string player,  shared_ptr<GameObject> gameObject) {
			this->spawnTime = spawnTime;
			this->playfield = playfield;
			this->player = player;
			this->gameObject = gameObject;
		}

		int spawnTime;
		string playfield;
		string player;
		shared_ptr<GameObject> gameObject;
	};

private:
	TileMap tilemap;
	vector<SpawnData> m_SpawnTimer;
	string m_hero1;
	string m_hero2;
	tgui::Gui m_gui;
	float spawnTimer = 0.0f;
	vector<bool> spawndone;

	void initSounds(const std::string& resourcePath) const;
};