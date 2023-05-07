#include "stdafx.h"
#include "GameObject.h"
#include "ProjectileCollisionObserver.h"
#include <utility>
#include "GameObjectManager.h"
#include "PlayerStatsComponent.h"


ProjectileCollisionObserver::ProjectileCollisionObserver(std::string thisID, std::string ownerID)
	: m_thisID(std::move(thisID))
	, m_ownerID(std::move(ownerID))
{
	
}

void ProjectileCollisionObserver::notify(GameObject& gameObject)
{
	if (gameObject.getID() != m_ownerID && GameObjectManager::getInstance().getGameObject(m_thisID)->getType() != gameObject.getType()) {
		if (gameObject.getType() == "PlayerObject") {
			shared_ptr<PlayerStatsComponent> playerstats = gameObject.getComponent<PlayerStatsComponent>();
			playerstats->getDamage(20);
			GameObjectManager::getInstance().getGameObject(m_ownerID)->getComponent<PlayerStatsComponent>()->getUltimatePoints(70);
		}
		else if (gameObject.getType() == "Enemy") {
			shared_ptr<PlayerStatsComponent> playerstats = gameObject.getComponent<PlayerStatsComponent>();
			playerstats->getDamage(20);
			GameObjectManager::getInstance().getGameObject(m_ownerID)->getComponent<PlayerStatsComponent>()->getUltimatePoints(20);
		}

		if (gameObject.getType() != "Healarea")
			GameObjectManager::getInstance().deleteGameObject(m_thisID);
	}
}