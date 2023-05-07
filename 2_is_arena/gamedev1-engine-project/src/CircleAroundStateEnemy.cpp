#include "stdafx.h"
//#include "CircleAroundStateEnemy.h"
//#include "AnimationManager.h"
//#include "EnemyController.h"
//#include "PlayFieldManager.h"
//
//namespace
//{
//	const float  PI_F = 3.14159265358979f;
//}
//
//void CircleAroundStateEnemy::handleState(EnemyController& controller, const Vector2i& normDirection, const float& deltaTime)
//{
//
//	// currently unused
//
//	const auto getDistBetweenVec = [](const Vector2f& vec1, const Vector2f& vec2) -> float
//	{
//		return sqrt((vec1.x - vec2.x) * (vec1.x - vec2.x) + (vec1.y - vec2.y) * (vec1.y - vec2.y));
//	};
//
//	if (controller.isHpZero())
//	{
//		controller.ChangeState("dead");
//		return;
//	}
//
//	// get the player that is in the same play field as this object (the enemy)
//	auto target = PlayFieldManager::getInstance().getPlayerCurrentlyAt(PlayFieldManager::getInstance().objectCurrentlyAt(controller.getGObject().getID()));
//	auto enemyToPlayerVec = target->GetPosition() - controller.getGObject().GetPosition();
//
//	if (m_lastNormDir != normDirection)
//	{
//		onEnter(controller, normDirection);
//	}
//
//	// enemy circles around player's position
//	if (getDistBetweenVec(controller.getGObject().GetPosition(), target->GetPosition()) < m_radius)
//	{
//		const auto rotateEnemyAroundTarget = [](Vector2f enemyPosition, Vector2f targetPosition, float degree) -> Vector2f 
//		{
//			float angleRadians = degree * (PI_F / 180);
//
//			float s = sin(angleRadians);
//			float c = cos(angleRadians);
//
//			//translate back to origin;
//			enemyPosition.x -= targetPosition.x;
//			enemyPosition.y -= targetPosition.y;
//
//			//rotate point
//			float xNew = enemyPosition.x * c - enemyPosition.y * s;
//			float yNew = enemyPosition.x * s + enemyPosition.y * c;
//
//			//translate point back
//			enemyPosition.x = xNew + targetPosition.x;
//			enemyPosition.y = yNew + targetPosition.y;
//
//			return enemyPosition;
//		};
//
//		auto steeringDir = controller.getGObject().GetPosition() - rotateEnemyAroundTarget(controller.getGObject().GetPosition(), target->GetPosition(), 1);
//		normalizeVec(steeringDir);
//		// move enemy to newly calculated position
//		controller.getGObject().getRigidbody()->velocity = steeringDir * controller.getSpeed() * 20.f;
//	}
//}
//
//void CircleAroundStateEnemy::onEnter(EnemyController & controller, const Vector2i& normDirection)
//{
//	if (normDirection == Vector2i(1, 0))
//	{
//		AnimationManager::getInstance().GetComponent(controller.getGObject().getID())->playAnimation("move_right", 0.2);
//	}
//	else if (normDirection == Vector2i(-1, 0))
//	{
//		AnimationManager::getInstance().GetComponent(controller.getGObject().getID())->playAnimation("move_left", 0.2);
//	}
//	else if (normDirection == Vector2i(0, 1))
//	{
//		AnimationManager::getInstance().GetComponent(controller.getGObject().getID())->playAnimation("move_front", 0.2);
//	}
//	else if (normDirection == Vector2i(0, -1))
//	{
//		AnimationManager::getInstance().GetComponent(controller.getGObject().getID())->playAnimation("move_back", 0.2);
//	}
//	else
//		throw std::invalid_argument("received wrong or no direction.");
//
//	m_lastNormDir = normDirection;
//}