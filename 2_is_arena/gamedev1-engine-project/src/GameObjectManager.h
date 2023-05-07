#pragma once
#include <string>
#include <unordered_map>
#include "GameObject.h"

class GameObjectManager
{
public:
	static GameObjectManager& getInstance();

	//Methods
	void addGameObject(const shared_ptr<GameObject>& gameObject);
	void deleteGameObject(const std::string& id); //Adds the id to m_deleteObjects
	void deleteGameObjects(); //Deletes all gameObjects in m_deleteObjects at the end of the frame(otherwise game might crash)
	void DebugRender(RenderWindow& window);

	//Getter
	shared_ptr<GameObject> getGameObject(const std::string& id) const;
	std::unordered_map<std::string, shared_ptr<GameObject>>& getGameObjects() { return m_gameObjects; }
	vector<shared_ptr<GameObject>> getGameObjectsByType(const std::string& type);

	//Release
	static void release();
	void reset();

	GameObjectManager(const GameObjectManager& p) = delete;
	GameObjectManager& operator=(GameObjectManager const&) = delete;

private:
	GameObjectManager(void) = default;
	~GameObjectManager(void) = default;

	static GameObjectManager *m_instance;

	std::unordered_map<std::string, shared_ptr<GameObject>> m_gameObjects;
	vector<std::string> m_deleteObjects;
};