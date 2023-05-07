#include "stdafx.h"
#include <utility>
#include "GameObject.h"
#include "MagicbulletCollisionObserver.h"
#include "GameObjectManager.h"
#include "PlayerStatsComponent.h"
#include "PlayFieldManager.h"

MagicBulletCollisionObserver::MagicBulletCollisionObserver(std::string thisID, std::string ownerID)
	: m_thisID(std::move(thisID))
	, m_ownerID(std::move(ownerID))
{
}

void MagicBulletCollisionObserver::notify(GameObject& gameObject)
{
	if (gameObject.getID() != m_ownerID && GameObjectManager::getInstance().getGameObject(m_thisID)->getType() != gameObject.getType()) {
		if (gameObject.getType() == "PlayerObject") {
			auto stats = gameObject.getComponent<PlayerStatsComponent>();
			stats->getSlowed(0.7f);
			stats->getDamage(5);
			GameObjectManager::getInstance().getGameObject(m_ownerID)->getComponent<PlayerStatsComponent>()->getUltimatePoints(50);
		}

		if (gameObject.getType() == "Enemy") {
			auto stats = gameObject.getComponent<PlayerStatsComponent>();

			if (PlayFieldManager::getInstance().objectCurrentlyAt(gameObject.getID()) == PlayFieldManager::getInstance().objectCurrentlyAt(m_ownerID)) {
				stats->getSlowed(0.7f);
				stats->getDamage(15);
			}
			else {
				stats->getSlowed(1.35f);
				
			}
			GameObjectManager::getInstance().getGameObject(m_ownerID)->getComponent<PlayerStatsComponent>()->getUltimatePoints(5);
		}

		if (gameObject.getType() != "Healarea")
			GameObjectManager::getInstance().deleteGameObject(m_thisID);
	}

}