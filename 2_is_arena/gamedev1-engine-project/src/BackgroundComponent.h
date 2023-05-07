#pragma once
#include "IGraphicsComponent.h"

class BackgroundComponent : public IGraphicsComponent {
public:
	BackgroundComponent(GameObject &gameObject, sf::String path, sf::RenderWindow &window);

	virtual void Draw(sf::RenderWindow &window) override;
	virtual void Update(float fDeltaTime) override {}

private:
	sf::Sprite sprite;
	sf::Texture texture;
};