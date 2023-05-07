#include "stdafx.h"
#include "CollisionResolverObserver.h"
#include "GameObject.h"
#include "GameObjectManager.h"

void CollisionResolverObserver::notify(GameObject& gameObject) {
	//GameObjectManager::getInstance().deleteGameObject(m_playerName);
	/*shared_ptr<ProjectileComponent> projcomp = std::dynamic_pointer_cast<ProjectileComponent>(gameObject.getComponents()[0]);


	if (gameObject.getType() == "Arrow" && projcomp->getName() != m_playerName) {
		std::cout << projcomp->getName() << " - " << m_playerName << std::endl;

		float x = rand() % 5000;
		if (m_playerName == "Player2") {
			x *= -1;
		}

		float y = rand() % 5000;
		float negative = rand() % 2;
		if (negative == 0) y *= -1;

		gameObject.getRigidbody()->m_impulses.push_back(sf::Vector2f(x, y));
	}*/
}

CollisionResolverObserver::CollisionResolverObserver(std::string playerName)
{
	m_playerName = playerName;
}