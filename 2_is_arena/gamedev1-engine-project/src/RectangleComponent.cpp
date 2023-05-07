#include "stdafx.h"
#pragma once
#include "RectangleComponent.h"


RectangleComponent::RectangleComponent(GameObject& gameObject, int width, int height, sf::Color color)
	: IGraphicsComponent(gameObject)
{
	rectangle = sf::RectangleShape(sf::Vector2f(width, height));
	rectangle.setFillColor(color);
}

void RectangleComponent::Draw(sf::RenderWindow &window) {
	rectangle.setPosition(m_gameObject.GetPosition());
	window.draw(rectangle);
}