#include "stdafx.h"
#include "IdleStateEnemy.h"
#include "AnimationManager.h"
#include "EnemyController.h"
#include "GameObjectManager.h"
#include "PlayFieldManager.h"

bool IdleStateEnemy::isDead(EnemyController& controller)
{
	// enemy is dead
	if (controller.isHpZero())
	{
		controller.ChangeState("dead");
		return true;
	}
	return false;
}

bool IdleStateEnemy::switchToChaseState(EnemyController& controller, Vector2<float> enemyToPlayerVec)
{
	// switch state to chase
	// where is enemy in relation to character? velocity in that direction
	normalizeVec(enemyToPlayerVec);
	controller.getGObject().getRigidbody()->velocity = enemyToPlayerVec * controller.getSpeed();
	controller.ChangeState("chase");
	return true;
}

bool IdleStateEnemy::isTargetInRange(EnemyController& controller)
{
	// we forgo the length (involving sqrt(...) to save on resources
	// is the player in range?
	const auto activateChaseState = [](const Vector2f& distance, const Vector2f& activationRange) -> bool
	{
		return ((distance.x * distance.x + distance.y * distance.y) < (activationRange.x * activationRange.x + activationRange.y * activationRange.y));
	};

	// we query the player who is currently in the same playfield as the enemy
	// suggestion: maybe only query player once -> set in controller, then later change if field is switched
	auto target = PlayFieldManager::getInstance().getPlayerCurrentlyAt(PlayFieldManager::getInstance().objectCurrentlyAt(controller.getGObject().getID()));
	const auto enemyToPlayerVec = target->GetPosition() - controller.getGObject().GetPosition();
	
	if (activateChaseState(enemyToPlayerVec, m_activationRange))
	{
		return switchToChaseState(controller, enemyToPlayerVec);
	}
	return false;
}

void IdleStateEnemy::handleState(EnemyController& controller, const Vector2i& normDirection, const float& deltaTime)
{
	if (isDead(controller)) return;
	if (isTargetInRange(controller)) return;
}

void IdleStateEnemy::onEnter(EnemyController & controller, const Vector2i& normDirection)
{
	playDirectionalAnimation(controller, normDirection);

	// reset velocity to zero on entry
	// when we're spawning (switch playfields), we set the velocity of the enemy to Vector2f(0, 1) to face downwards
	// therefor we reset
	controller.getGObject().getRigidbody()->velocity = Vector2f(0.f, 0.f);
}