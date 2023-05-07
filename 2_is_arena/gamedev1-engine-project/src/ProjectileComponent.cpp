#include "stdafx.h"
#include "ProjectileComponent.h"
#include "GameObject.h"
#include "GameObjectManager.h"

void ProjectileComponent::Update(float fDeltaTime)
{
	m_gameObject.setPosition(m_gameObject.GetPosition() + m_velocity);

	//Move
	if (m_gameObject.GetPosition().x > 3000 || m_gameObject.GetPosition().x < -500 ||
		m_gameObject.GetPosition().y < -500 || m_gameObject.GetPosition().y > 2000)
	{
		GameObjectManager::getInstance().deleteGameObject(m_gameObject.getID());
	}

}

std::string ProjectileComponent::getName() const
{
	return m_ownerName;
}
