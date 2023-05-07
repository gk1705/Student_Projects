#include "stdafx.h"
#include "Debug.h"
#include "GameObjectManager.h"
#include "UpdateManager.h"
#include "PhysicsManager.h"
#include "RenderManager.h"
#include "AnimationManager.h"
#include "PlayFieldManager.h"
#include "SoundManager.h"

typedef std::unordered_map<std::string, shared_ptr<GameObject>> GameObjectMap;

GameObjectManager* GameObjectManager::m_instance = nullptr;

GameObjectManager& GameObjectManager::getInstance() {
	if (m_instance == nullptr)
		m_instance = new GameObjectManager();
	return *m_instance;
}

void GameObjectManager::addGameObject(const shared_ptr<GameObject>& gameObject)
{
	FF_ASSERT_MSG(m_gameObjects.find(gameObject->getID()) == m_gameObjects.end(),
		"Game object with this id already exists " + gameObject->getID());

	m_gameObjects[gameObject->getID()] = gameObject;

	if (gameObject->getMovementComponent()) {
		UpdateManager::getInstance().AddComponent(gameObject->getMovementComponent(), gameObject->getID());
	}

	for (const auto& comp : gameObject->getComponents()) {
		UpdateManager::getInstance().AddComponent(comp, gameObject->getID());
	}

	if (gameObject->getRigidbody()) {
		PhysicsManager::getInstance().addObject(gameObject);
	}

}

void GameObjectManager::deleteGameObject(const std::string& id)
{
	//Push the id of the gameObject into a vector. Delete The GameObject at the end of the frame
	m_deleteObjects.push_back(id);
}

void GameObjectManager::deleteGameObjects() {
	for (const auto& id : m_deleteObjects) {
		m_gameObjects.erase(id);
		UpdateManager::getInstance().DeleteComponentsOfGameObject(id);
		RenderManager::getInstance().DeleteComponentsOfGameObject(id);
		PhysicsManager::getInstance().removeObject(id);
	}
	m_deleteObjects.clear();
}

void GameObjectManager::DebugRender(RenderWindow& window)
{
	for (auto map : m_gameObjects) {
		for (const auto& collComp : map.second->getCollisionComponents()) {
			collComp->Render(window);
		}
	}
}

shared_ptr<GameObject> GameObjectManager::getGameObject(const std::string& id) const
{
	const auto it = m_gameObjects.find(id);
	if (it == m_gameObjects.end())
	{
		FF_ERROR_MSG("Could not find gameobject with id " + id);
		return nullptr;
	}
	return it->second;
}

vector<shared_ptr<GameObject>> GameObjectManager::getGameObjectsByType(const std::string& type)
{
	vector<shared_ptr<GameObject>> newMap;
	
	for (const auto& object : m_gameObjects) {
		if (object.second->getType() == type) newMap.push_back(object.second);
	}

	return newMap;
}

void GameObjectManager::release()
{
	if (m_instance != nullptr)
		delete m_instance;
	m_instance = nullptr;
}

void GameObjectManager::reset()
{
	m_gameObjects.clear();
	RenderManager::getInstance().reset();
	UpdateManager::getInstance().reset();
	AnimationManager::getInstance().reset();
	PhysicsManager::getInstance().reset();
	PlayFieldManager::getInstance().reset();
	SoundManager::getInstance().reset();
}
