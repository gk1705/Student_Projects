#include "stdafx.h"
#include "MonsterComponent.h"
#include "GameObject.h"
#include "GameObjectManager.h"

void MonsterComponent::Update(float fDeltaTime)
{
	// utility lambda
	auto Normalize = [](Vector2f vec) -> Vector2f
	{
		float length = sqrt(vec.x * vec.x + vec.y * vec.y);

		if (length != 0) {
			vec.x /= length;
			vec.y /= length;
		}
		else {
			vec.x = 1;
			vec.y = 0;
		}
		return vec;
	};

	if (GameObjectManager::getInstance().getGameObject(m_targetObject) != nullptr) {
		Vector2f targetPos = GameObjectManager::getInstance().getGameObject(m_targetObject)->GetPosition();
		Vector2f velocity = Normalize(targetPos - m_gameObject.GetPosition());
		velocity *= 1.5f;
		m_gameObject.setPosition(m_gameObject.GetPosition() + velocity);
	}
}