#include "stdafx.h"
#include "RenderManager.h"

RenderManager* RenderManager::m_instance = nullptr;

RenderManager& RenderManager::getInstance() {
	if (m_instance == nullptr)
		m_instance = new RenderManager();
	return *m_instance;
}

void RenderManager::release()
{
	if (m_instance != nullptr)
		delete m_instance;
	m_instance = nullptr;
}

void RenderManager::AddComponent(const shared_ptr<IGraphicsComponent>& renderComponent, std::string ownerID, int layerIdx)
{
	if (m_layer.empty()) {
		m_layer.push_back(map <string, vector<shared_ptr<IGraphicsComponent>>>());
		m_layer.push_back(map <string, vector<shared_ptr<IGraphicsComponent>>>());
		m_layer.push_back(map <string, vector<shared_ptr<IGraphicsComponent>>>());

	}

	const auto keyExists2 = m_layer[layerIdx].find(ownerID);
	if (keyExists2 == m_layer[layerIdx].end()) {
		m_layer[layerIdx].insert({ ownerID, vector<shared_ptr<IGraphicsComponent>>() });
	}

	//add to vector
	m_layer[layerIdx].find(ownerID)->second.push_back(renderComponent);
}

void RenderManager::DeleteComponentsOfGameObject(const string& ownerID)
{
	for (auto& i : m_layer)
	{
		i.erase(ownerID);
	}
}

void RenderManager::Render(sf::RenderWindow& window)
{
	for (auto layer : m_layer) {
		for (auto map : layer) {
			for (const auto& comp : map.second) {
				comp->Draw(window);
			}
		}
	}
}

void RenderManager::reset()
{
	m_layer.clear();
}