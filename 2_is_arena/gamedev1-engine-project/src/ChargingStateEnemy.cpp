#include "stdafx.h"
#include "ChargingStateEnemy.h"
#include "AnimationManager.h"
#include "EnemyController.h"
#include "PlayFieldManager.h"

bool ChargingStateEnemy::isDead(EnemyController& controller)
{
	if (controller.isHpZero())
	{
		controller.ChangeState("dead");
		return true;
	}
	return false;
}

bool ChargingStateEnemy::isCollidingBorder(EnemyController& controller)
{
	if (controller.isCollidingBorder())
	{
		controller.ChangeState("stunned");
		return true;
	}
	return false;
}

void ChargingStateEnemy::move(EnemyController& controller, const Vector2i& normDirection)
{
	controller.getGObject().getRigidbody()->velocity = Vector2f(normDirection) * controller.getSpeed() * m_speedModifier;
}

void ChargingStateEnemy::handleState(EnemyController& controller, const Vector2i& normDirection, const float& deltaTime)
{
	if (isDead(controller)) return;
	if (isCollidingBorder(controller)) return;

	move(controller, normDirection);
}

void ChargingStateEnemy::onEnter(EnemyController & controller, const Vector2i& normDirection)
{
	playDirectionalAnimation(controller, normDirection);
}
