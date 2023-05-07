#include "stdafx.h"
#pragma once
#include "ItemPickedUpObserver.h"
#include "GameObject.h"
#include "RigidbodyComponent.h"
#include "GameObjectManager.h"
#include "MovementComponent.h"
#include "AIController.h"

void ItemPickedUpObserver::notify(GameObject& gameObject)
{
	if (gameObject.getType() == "Item") {
		printf("%s picked up %i Item(s).\n", m_playerName.c_str(), ++m_counter);
		gameObject.setPosition(Vector2f(-100,-100));

		shared_ptr<GameObject> PlayerObject = GameObjectManager::getInstance().getGameObject(m_playerName);
		PlayerObject->setHeight(PlayerObject->getHeight()+75);

		if (m_counter >= 5) {
			printf("%s won the Game!\n", m_playerName.c_str());
			for (auto item : GameObjectManager::getInstance().getGameObjectsByType("Item")) {
				item->setPosition(Vector2f(-100, -100));
			}

			GameObjectManager::getInstance().getGameObject("Ball")->setPosition(Vector2f(-100, -100));

			// ai happily dances;
			IController& controller = GameObjectManager::getInstance().getGameObjectsByType("PlayerObject")[1]->getComponent<MovementComponent>()->getStrategy();
			dynamic_cast<AIController&>(controller).ChangeState(make_unique<DanceState>(AIController::state_Dance));
		}
	}
}

ItemPickedUpObserver::ItemPickedUpObserver(std::string playerName_)
{
	m_playerName = playerName_;
}