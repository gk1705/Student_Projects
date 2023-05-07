#pragma once
#include "stdafx.h"
#include "GameObject.h"
#include <SFML/Graphics.hpp>
#include "IComponent.h"

class IGraphicsComponent : public IComponent
{
public:
	IGraphicsComponent(GameObject& gameObject)
		: IComponent(gameObject)
	{

	}

	virtual void Draw(sf::RenderWindow &window) = 0;

	virtual float getWidth() const;
	virtual float getHeight() const;
	void setHeight(float height);

	GameObject& DebugGetGameObject() const
	{
		return m_gameObject;
	}

protected:
	sf::RectangleShape rectangle;
};
