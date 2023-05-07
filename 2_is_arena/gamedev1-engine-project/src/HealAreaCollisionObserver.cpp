#include "stdafx.h"
#include "GameObject.h"
#include "HealAreaCollisionObserver.h"
#include "GameObjectManager.h"
#include "PlayerStatsComponent.h"

void HealAreaCollisionObserver::notify(GameObject& gameObject)
{
	if (gameObject.getType() == "PlayerObject") {
		gameObject.getComponent<PlayerStatsComponent>()->receiveHealing();
	}
}