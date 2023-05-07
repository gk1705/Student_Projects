#include "stdafx.h"
#include "DespawnComponent.h"
#include "GameObject.h"
#include "GameObjectManager.h"
#include "SoundManager.h"

void DespawnComponent::Update(float fDeltaTime)
{
	m_timer += fDeltaTime;

	if (m_timer >= m_despawnTime) {
		GameObjectManager::getInstance().deleteGameObject(m_gameObject.getID());
		SoundManager::getInstance().StopMusic();
	}
}

DespawnComponent::DespawnComponent(GameObject& gameObject, float despawnTime)
	: IComponent(gameObject)
	, m_despawnTime(despawnTime)
{
}