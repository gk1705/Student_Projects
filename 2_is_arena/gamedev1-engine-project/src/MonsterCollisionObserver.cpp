#include "stdafx.h"
#include "GameObject.h"
#include "MonsterCollisionObserver.h"
#include <utility>
#include "GameObjectManager.h"
#include "PlayerStatsComponent.h"
#include "MovementComponent.h"


MonsterCollisionObserver::MonsterCollisionObserver(std::string thisID)
	: m_thisID(std::move(thisID))
{
}

void MonsterCollisionObserver::notify(GameObject& gameObject)
{
	// player collides with enemy -> player gets damaged
	if (gameObject.getType() == "PlayerObject") {
		gameObject.getComponent<PlayerStatsComponent>()->getDamage(25);

		// additionally, on impact a bat's state is set to stunned
		if (m_thisID.substr(0, 3) == "bat")
		{
			GameObjectManager::getInstance().getGameObject(m_thisID)->getMovementComponent()->getStrategy().ChangeState("stunned");
		}
	}
}