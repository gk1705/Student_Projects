#include "stdafx.h"
#include "UpdateManager.h"
#include "GameObjectManager.h"
#include "PhysicsManager.h"

UpdateManager* UpdateManager::m_instance = nullptr;

UpdateManager& UpdateManager::getInstance() {
	if (m_instance == nullptr)
		m_instance = new UpdateManager();
	return *m_instance;
}

void UpdateManager::release()
{
	if (m_instance != nullptr)
		delete m_instance;
	m_instance = nullptr;
}

void UpdateManager::AddComponent(const shared_ptr<MovementComponent>& component, const std::string& ownerID) {
	const auto keyExists = m_movementComponents.find(ownerID);
	if (keyExists == m_movementComponents.end()) { //does not exist
		m_movementComponents.insert({ ownerID, component });
	}
}

void UpdateManager::AddComponent(const shared_ptr<IComponent>& component, const std::string& ownerID)
{
	//Check if key exists
	const auto keyExists = m_components.find(ownerID);
	if (keyExists == m_components.end()) { //does not exist
		m_components.insert({ ownerID, vector<shared_ptr<IComponent>>() });
	}

	//add to Component-Vector
	m_components.find(ownerID)->second.push_back(component);
}

void UpdateManager::AddComponent(const shared_ptr<IGraphicsComponent>& component, const std::string& ownerID)
{
	const auto keyExists = m_graphicscomponents.find(ownerID);
	if (keyExists == m_graphicscomponents.end()) { //does not exist
		m_graphicscomponents.insert({ ownerID, vector<shared_ptr<IGraphicsComponent>>() });
	}

	//add to vector
	m_graphicscomponents.find(ownerID)->second.push_back(component);
}

void UpdateManager::DeleteComponentsOfGameObject(const string& ownerID)
{
	m_components.erase(ownerID);
	m_graphicscomponents.erase(ownerID);
	m_movementComponents.erase(ownerID);
}

void UpdateManager::Update(float fDeltaTime)
{
	for (auto map : m_movementComponents) {
		map.second->Update(fDeltaTime);
	}

	PhysicsManager::getInstance().handleCollisions();

	for (auto map : m_components) {
		for (const auto& comp : map.second) {
			comp->Update(fDeltaTime);
		}
	}

	for (auto map : m_graphicscomponents)
	{
		for (const auto& comp : map.second)
		{
			comp->Update(fDeltaTime);
		}
	}
}

void UpdateManager::reset()
{
	m_components.clear();
	m_graphicscomponents.clear();
	m_movementComponents.clear();
}