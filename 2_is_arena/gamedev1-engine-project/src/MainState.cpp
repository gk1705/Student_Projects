#include "stdafx.h"
#pragma once;
#include "MainState.h"
#include "TileMap.h"
#include "RenderManager.h"
#include "PhysicsManager.h"
#include "GameObjectManager.h"
#include "UpdateManager.h"
#include "InputManager.h"
#include "AIController.h"
#include "EnemyLoader.h"
#include "PlayFieldManager.h"
#include "UltimateObserver.h"
#include "SoundManager.h"

using namespace sf;
using namespace std;

//#define DEBUG

void MainState::init(sf::RenderWindow& window) {
	//Reset spawntimer
	spawnTimer = 0;
	// init the array where we will query for our game objects based on the playfield
	PlayFieldManager::getInstance().initFieldArrays();

	initTileMap(window);
	initEnemies();
	initMonsterSpawnTime();
	initGUI(window);
	initSounds("../assets/Sounds/");
}

void MainState::initGUI(sf::RenderWindow& window)
{
	m_gui.removeAllWidgets();
	m_gui.setWindow(window);
	m_gui.setFont("../assets/GUI/arial.ttf");
	createGUI();
}

void MainState::initTileMap(sf::RenderWindow& window)
{
	tilemap = TileMap();
	tilemap.LoadMap("../assets/Levels/Level1.tmx", Vector2f(), window);
	
	HeroLoader::LoadHeros(m_hero1, m_hero2); 
	PlayFieldManager::getInstance().addObjectToPlayfield("Field1",
	                                                     GameObjectManager::getInstance().getGameObject("Player1"));
	PlayFieldManager::getInstance().addObjectToPlayfield("Field2",
	                                                     GameObjectManager::getInstance().getGameObject("Player2"));
}

void MainState::initEnemies() const
{
	// we create the enemies on enter and spawn them outside of the map
	// there they'll be frozen -> their state will not be updated
	// periodically we teleport them inside the playfields, which in turn activates them
	const Vector2f spawnposition(-1000, -1000);
	
	// creating and adding them
	PlayFieldManager::getInstance().addObjectToPlayfield("Field1", EnemyLoader::LoadEnemy("bat", "bat1", spawnposition));
	PlayFieldManager::getInstance().addObjectToPlayfield("Field2", EnemyLoader::LoadEnemy("bat", "bat2", spawnposition));
	PlayFieldManager::getInstance().addObjectToPlayfield("Field1", EnemyLoader::LoadEnemy("bat", "bat3", spawnposition));
	PlayFieldManager::getInstance().addObjectToPlayfield("Field2", EnemyLoader::LoadEnemy("bat", "bat4", spawnposition));
	PlayFieldManager::getInstance().addObjectToPlayfield("Field1", EnemyLoader::LoadEnemy("boar", "boar1", spawnposition));
	PlayFieldManager::getInstance().addObjectToPlayfield("Field2", EnemyLoader::LoadEnemy("boar", "boar2", spawnposition));
	PlayFieldManager::getInstance().addObjectToPlayfield("Field1", EnemyLoader::LoadEnemy("boar", "boar3", spawnposition));
	PlayFieldManager::getInstance().addObjectToPlayfield("Field2", EnemyLoader::LoadEnemy("boar", "boar4", spawnposition));
}

void MainState::initMonsterSpawnTime()
{
	//Define Spawntimers of Monsters
	m_SpawnTimer.push_back(SpawnData(60, "Field1", "Player1", GameObjectManager::getInstance().getGameObject("boar3")));
	m_SpawnTimer.push_back(SpawnData(60, "Field2", "Player2", GameObjectManager::getInstance().getGameObject("boar4")));
	m_SpawnTimer.push_back(SpawnData(35, "Field1", "Player1", GameObjectManager::getInstance().getGameObject("bat3")));
	m_SpawnTimer.push_back(SpawnData(35, "Field2", "Player2", GameObjectManager::getInstance().getGameObject("bat4")));
	m_SpawnTimer.push_back(SpawnData(15, "Field1", "Player1", GameObjectManager::getInstance().getGameObject("boar1")));
	m_SpawnTimer.push_back(SpawnData(15, "Field2", "Player2", GameObjectManager::getInstance().getGameObject("boar2")));
	m_SpawnTimer.push_back(SpawnData(4, "Field1", "Player1", GameObjectManager::getInstance().getGameObject("bat1")));
	m_SpawnTimer.push_back(SpawnData(4, "Field2", "Player2", GameObjectManager::getInstance().getGameObject("bat2")));
}

void MainState::createGUI()
{

#pragma region GUISetup
	const sf::Color darkblue(0, 0, 150);

	//Player 1 GUI
	auto ultimatebar_background = make_shared<tgui::Panel>();
	ultimatebar_background->setSize(500, 50);
	ultimatebar_background->setPosition(50, 990);

	ultimatebar_background->setBackgroundColor(darkblue);
	m_gui.add(ultimatebar_background);

	auto ultimatebar = make_shared<tgui::Panel>();
	ultimatebar->setSize(0, 50);
	ultimatebar->setPosition(tgui::bindPosition(ultimatebar_background));
	ultimatebar->setBackgroundColor(sf::Color::Blue);
	m_gui.add(ultimatebar);
	GameObjectManager::getInstance().getGameObject("Player1")->getComponent<PlayerStatsComponent>()->registerObserver(make_shared<UltimateObserver>(ultimatebar));

	auto text = make_shared<tgui::Label>();
	text->setPosition(800, 990);
	text->setText(to_string(PlayFieldManager::getInstance().getScore(0)) + "    SCORE    " + to_string(PlayFieldManager::getInstance().getScore(1)));
	text->setTextColor(Color::White);
	text->setTextSize(50);
	m_gui.add(text);

	//Player2 GUI
	auto ultimatebar2_background = make_shared<tgui::Panel>();
	ultimatebar2_background->setSize(500, 50);
	ultimatebar2_background->setPosition(1920.0 - ultimatebar2_background->getSize().x - 50, 990);
	ultimatebar2_background->setBackgroundColor(darkblue);
	m_gui.add(ultimatebar2_background);

	auto ultimatebar2 = make_shared<tgui::Panel>();
	ultimatebar2->setSize(0, 50);
	ultimatebar2->setPosition(tgui::bindPosition(ultimatebar2_background));
	ultimatebar2->setBackgroundColor(sf::Color::Blue);
	m_gui.add(ultimatebar2);
	GameObjectManager::getInstance().getGameObject("Player2")->getComponent<PlayerStatsComponent>()->registerObserver(make_shared<UltimateObserver>(ultimatebar2));

#pragma endregion

}

void MainState::update(float delta, sf::RenderWindow& window) {
	UpdateManager::getInstance().Update(delta);

	if (InputManager::getInstance().IsButtonPressed("Start", 1) || InputManager::getInstance().IsButtonPressed("Start", 2)) {
		GameStateManager::getInstance().setState("PauseMenu");
	}

	spawnTimer += delta;

	if (spawnTimer >= m_SpawnTimer.back().spawnTime) {
		m_SpawnTimer.back().gameObject->setPosition(Vector2f(PlayFieldManager::getInstance()
			.returnSpawnPosition(m_SpawnTimer.back().playfield, GameObjectManager::getInstance()
				.getGameObject(m_SpawnTimer.back().player)->GetPosition())));
		m_SpawnTimer.pop_back();
	}
}

void MainState::render(sf::RenderWindow &window) {
	
#ifdef DEBUG
	GameObjectManager::getInstance().DebugRender(window);
#else
	RenderManager::getInstance().Render(window);
#endif

	m_gui.draw();
}

void MainState::handleEvents(sf::Event event)
{
	m_gui.handleEvent(event);
}

void MainState::SetHeros(string hero1, string hero2)
{
	m_hero1 = hero1;
	m_hero2 = hero2;
}

void MainState::initSounds(const std::string& resourcePath) const
{
	SoundManager::getInstance().AddSound("Archer_shooting", resourcePath + "Archer_shooting.wav");
	SoundManager::getInstance().AddSound("Archer_ultimate", resourcePath + "Archer_ultimate.wav");
	SoundManager::getInstance().AddSound("Witch_shooting", resourcePath + "Witch_shooting.wav");
	SoundManager::getInstance().AddSound("Player_hit1", resourcePath + "Player_hit1.wav");
	SoundManager::getInstance().AddSound("Player_hit2", resourcePath + "Player_hit2.wav");
	SoundManager::getInstance().AddSound("Player_hit3", resourcePath + "Player_hit3.wav");
	SoundManager::getInstance().AddSound("Player_hit4", resourcePath + "Player_hit4.wav");
	SoundManager::getInstance().AddSound("Player_hit5", resourcePath + "Player_hit5.wav");
	SoundManager::getInstance().AddSound("Player_hit6", resourcePath + "Player_hit6.wav");
	SoundManager::getInstance().AddSound("Player_hit7", resourcePath + "Player_hit7.wav");
	SoundManager::getInstance().AddSound("Player_hit8", resourcePath + "Player_hit8.wav");
	SoundManager::getInstance().AddSound("Player_movement1", resourcePath + "Player_movement1.wav");
	SoundManager::getInstance().AddSound("Player_movement2", resourcePath + "Player_movement2.wav");
	SoundManager::getInstance().AddSound("Player_movement3", resourcePath + "Player_movement3.wav");
	SoundManager::getInstance().AddSound("Player_movement4", resourcePath + "Player_movement4.wav");
	SoundManager::getInstance().AddSound("Player_movement5", resourcePath + "Player_movement5.wav");
	SoundManager::getInstance().AddSound("Player_movement6", resourcePath + "Player_movement6.wav");
	SoundManager::getInstance().AddSound("Player_movement7", resourcePath + "Player_movement7.wav");
	SoundManager::getInstance().AddSound("Player_movement8", resourcePath + "Player_movement8.wav");
	SoundManager::getInstance().AddSound("Player_movement9", resourcePath + "Player_movement9.wav");
	SoundManager::getInstance().AddSound("Player_movement10", resourcePath + "Player_movement10.wav");
	SoundManager::getInstance().AddSound("Enemy_hit1", resourcePath + "Enemy_hit1.wav");
	SoundManager::getInstance().AddSound("Enemy_hit2", resourcePath + "Enemy_hit2.wav");
	SoundManager::getInstance().AddSound("bat_death", resourcePath + "bat_death.wav");
	SoundManager::getInstance().AddSound("boar_death", resourcePath + "boar_death.wav");
}