#include "stdafx.h"
#include "LayerComponent.h"
#include "GameObject.h"

void LayerComponent::Draw(sf::RenderWindow &window)
{
	//render each tile
	for (auto& tile : m_tiles) {
		window.draw(*tile);
	}
}

LayerComponent::LayerComponent(GameObject& gameObject)
	: IGraphicsComponent(gameObject)
{
	
}

void LayerComponent::Insert(shared_ptr<Sprite> sprite)
{
	m_tiles.push_back(sprite);
}
