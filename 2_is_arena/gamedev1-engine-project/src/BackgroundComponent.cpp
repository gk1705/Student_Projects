#include "stdafx.h"
#pragma once
#include "BackgroundComponent.h"


BackgroundComponent::BackgroundComponent(GameObject& gameObject, sf::String path, sf::RenderWindow& window)
	: IGraphicsComponent(gameObject)
{
	texture.loadFromFile(path);
	sprite.setTexture(texture);

	float screenwidth = window.getSize().x;
	float screenheight = window.getSize().y;
	float texturewidth = texture.getSize().x;
	float textureheight = texture.getSize().y;

	sprite.setScale(sf::Vector2f(screenwidth/texturewidth, screenheight/textureheight));
}

void BackgroundComponent::Draw(sf::RenderWindow &window) {
	sprite.setPosition(m_gameObject.GetPosition());
	window.draw(sprite);
}
