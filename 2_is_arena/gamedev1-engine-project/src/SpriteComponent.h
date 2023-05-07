#pragma once
#include "IGraphicsComponent.h"

class SpriteComponent: public IGraphicsComponent {
public: 
	SpriteComponent(GameObject& gameObject, shared_ptr<Texture> texture);
	SpriteComponent(GameObject& gameObject, std::string path);
	void Rotate(float degree);
	void SetScale(Vector2f scale);
	void Draw(sf::RenderWindow &window) override;
	void Update(float fDeltaTime) override {}

	Sprite& getSprite();

private:
	Sprite m_sprite;
	shared_ptr<Texture> m_texture;
};	