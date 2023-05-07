#pragma once
#include "IGraphicsComponent.h"

class GameObject;

class LayerComponent : public IGraphicsComponent
{
public:
	//typedef std::vector<shared_ptr<Sprite>> Tiles;

	LayerComponent(GameObject& gameObject);

	void Draw(sf::RenderWindow &window) override;	
	void Update(float fDeltaTime) override {}
	void Insert(shared_ptr<Sprite> sprite);
	
private:
	std::vector<shared_ptr<Sprite>> m_tiles;
};