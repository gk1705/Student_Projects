#pragma once
#include "stdafx.h"
#include "IComponent.h"

class ProjectileComponent : public IComponent
{
public:
	ProjectileComponent(GameObject& gameObject, std::string ownerName, sf::Vector2f velocity)
		:IComponent(gameObject),
		m_velocity(velocity),
		m_ownerName(ownerName)
	{

	}

	void Update(float fDeltaTime) override;
	std::string getName() const;

private:
	sf::Vector2f m_velocity;
	std::string m_ownerName; //Who shot this projectile
};