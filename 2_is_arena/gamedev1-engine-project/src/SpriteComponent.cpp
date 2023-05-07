#include "stdafx.h"
#include "SpriteComponent.h"

SpriteComponent::SpriteComponent(GameObject& gameObject, shared_ptr<Texture> texture)
	: IGraphicsComponent(gameObject)
{
	m_texture = texture;
	m_sprite.setTexture(*texture);

	rectangle = sf::RectangleShape(sf::Vector2f(m_sprite.getTextureRect().width, m_sprite.getTextureRect().height));
}

SpriteComponent::SpriteComponent(GameObject& gameObject, std::string path)
	: IGraphicsComponent(gameObject)
{
	Image image;
	image.loadFromFile(path);
	shared_ptr<Texture> texture = make_shared<Texture>();
	texture->loadFromImage(image);
	m_texture = texture;
	m_sprite.setTexture(*texture);

	rectangle = sf::RectangleShape(sf::Vector2f(m_sprite.getTextureRect().width, m_sprite.getTextureRect().height));
}

void SpriteComponent::Rotate(float angle)
{
	m_sprite.rotate(angle);
}

void SpriteComponent::SetScale(Vector2f scale)
{
	m_sprite.setScale(scale);
}

void SpriteComponent::Draw(sf::RenderWindow &window)
{
	m_sprite.setPosition(m_gameObject.GetPosition());
	window.draw(m_sprite);
}

Sprite & SpriteComponent::getSprite()
{
	return m_sprite;
}
