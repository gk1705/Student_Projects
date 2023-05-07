#include "stdafx.h"
#pragma once
#include "BoxColliderComponent.h"
#include "GameObject.h"

BoxColliderComponent::BoxColliderComponent(GameObject& gameObject, sf::FloatRect shape)
	: IComponent(gameObject)
	, ownerObject(gameObject)
	, m_offsetFromPosition(Vector2f(0,0))
	, m_shape(shape)
{
	debugGeometry.setFillColor(sf::Color::Transparent);
	debugGeometry.setOutlineColor(sf::Color::Red);
	debugGeometry.setOutlineThickness(1);
}

// offset is handy if we want to have a particularly placed boxcollider; also for multiple boxcolliders for one object
BoxColliderComponent::BoxColliderComponent(GameObject& gameObject, sf::FloatRect shape, sf::Vector2f offsetFromPosition)
	: IComponent(gameObject)
	, ownerObject(gameObject)
	, m_offsetFromPosition(offsetFromPosition)
	, m_shape(shape)
{
	debugGeometry.setFillColor(sf::Color::Transparent);
	debugGeometry.setOutlineColor(sf::Color::Red);
	debugGeometry.setOutlineThickness(1);
}

void BoxColliderComponent::SetTrigger(bool trigger) {
	m_trigger = trigger;
}

bool BoxColliderComponent::GetTrigger() const
{
	return m_trigger;
}

void BoxColliderComponent::SetDisabled(const bool disable)
{
	m_disabled = disable;
}

bool BoxColliderComponent::GetDisabled() const
{
	return m_disabled;
}

sf::FloatRect& BoxColliderComponent::GetShape()
{
	return m_shape;
}

sf::Vector2f BoxColliderComponent::GetOffset() const
{
	return m_offsetFromPosition;
}

void BoxColliderComponent::Update(float fDeltatime) {
	m_shape.top = ownerObject.GetPosition().y + m_offsetFromPosition.y;
	m_shape.left = ownerObject.GetPosition().x + m_offsetFromPosition.x;
}

void BoxColliderComponent::Render(sf::RenderWindow &window) {
	debugGeometry.setPosition(sf::Vector2f(m_shape.left, m_shape.top));
	debugGeometry.setSize(sf::Vector2f(m_shape.width, m_shape.height));
	window.draw(debugGeometry);
}