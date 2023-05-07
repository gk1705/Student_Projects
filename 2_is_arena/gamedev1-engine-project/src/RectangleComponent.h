#pragma once

#include "IGraphicsComponent.h"

class RectangleComponent : public IGraphicsComponent {
public:
	RectangleComponent(GameObject& gameObject, int width, int height, sf::Color color);

	virtual void Draw(sf::RenderWindow &window) override;
	virtual void Update(float fDeltaTime) override {}
};