#pragma once
#include <memory>
#include <vector>
#include "IGraphicsComponent.h"

class RenderManager
{
public:
	static RenderManager& getInstance();
	static void release();

	void AddComponent(const shared_ptr<IGraphicsComponent>& renderComponent, std::string ownerID, int layerIdx);
	void DeleteComponentsOfGameObject(const string& ownerID);
	void Render(sf::RenderWindow& window);

	void reset();

	RenderManager(const RenderManager& p) = delete;
	RenderManager& operator=(RenderManager const&) = delete;

private:
	RenderManager(void) = default;
	~RenderManager(void) = default;

	static RenderManager *m_instance;

	vector<map <string, vector<shared_ptr<IGraphicsComponent>>>> m_layer;

};