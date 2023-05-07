#pragma once
#include "stdafx.h"
#include "IComponent.h"

class GameObject;

class BoxColliderComponent : public IComponent
{
public:
	BoxColliderComponent(GameObject& gameObject, sf::FloatRect shape);
	BoxColliderComponent(GameObject& gameObject, sf::FloatRect shape, sf::Vector2f offsetFromPosition);
	void Update(float fDeltaTime) override;
	void Render(sf::RenderWindow &window);

	void SetTrigger(bool trigger);
	bool GetTrigger() const;
	void SetDisabled(const bool disable);
	bool GetDisabled() const;

	sf::FloatRect& GetShape();
	sf::Vector2f GetOffset() const;

private:
	GameObject& ownerObject;
	sf::RectangleShape debugGeometry;
	sf::Vector2f m_offsetFromPosition;

	sf::FloatRect m_shape;
	bool m_trigger = false;
	bool m_disabled = false;
};
