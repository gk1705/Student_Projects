#pragma once
#include "stdafx.h"
#include "HealthBarComponent.h"
#include "BoxColliderComponent.h"


HealthBarComponent::HealthBarComponent(GameObject& gameObject)
	: IGraphicsComponent(gameObject)
{
	m_healthbarGreen = sf::RectangleShape(sf::Vector2f(m_gameObject.getColliderShape().width, 5));
	m_healthbarGreen.setFillColor(sf::Color::Green);

	m_healthbarRed = sf::RectangleShape(sf::Vector2f(m_gameObject.getColliderShape().width, 5));
	m_healthbarRed.setFillColor(sf::Color::Red);
}

void HealthBarComponent::Draw(sf::RenderWindow &window) {
	const shared_ptr<BoxColliderComponent> collisioncomp = m_gameObject.getCollisionComponents()[0];
	m_healthbarGreen.setPosition(m_gameObject.GetPosition().x + collisioncomp->GetOffset().x, m_gameObject.GetPosition().y);
	m_healthbarRed.setPosition(m_healthbarGreen.getPosition());
	window.draw(m_healthbarRed);
	window.draw(m_healthbarGreen);
}

void HealthBarComponent::setScale(float scale)
{
	m_healthbarGreen.setScale(Vector2f(scale, 1));
}
