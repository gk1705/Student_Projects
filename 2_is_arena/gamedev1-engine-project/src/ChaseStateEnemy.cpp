#include "stdafx.h"
#include "ChaseStateEnemy.h"
#include "AnimationManager.h"
#include "EnemyController.h"
#include "PlayFieldManager.h"

bool ChaseStateEnemy::isDead(EnemyController& controller)
{
	if (controller.isHpZero())
	{
		controller.ChangeState("dead");
		return true;
	}
	return false;
}

bool ChaseStateEnemy::isOutOfRange(EnemyController& controller, Vector2<float>& enemyToPlayerVec)
{
	const auto deactivateChaseState = [](const Vector2f& distance, const Vector2f& activationRange) -> bool
	{
		return ((distance.x * distance.x + distance.y * distance.y) > (activationRange.x * activationRange.x + activationRange.y * activationRange.y));
	};

	// get the player that is in the same play field as this object (the enemy)
	auto target = PlayFieldManager::getInstance().getPlayerCurrentlyAt(PlayFieldManager::getInstance().objectCurrentlyAt(controller.getGObject().getID()));
	enemyToPlayerVec = target->GetPosition() - controller.getGObject().GetPosition();
	// if player is out of range, we switch the monster to idle state
	if (deactivateChaseState(enemyToPlayerVec, m_deactivationRange))
	{
		controller.ChangeState("idle");
		return true;
	}
	return false;
}

void ChaseStateEnemy::checkDirectionChange(EnemyController& controller, const Vector2i& normDirection)
{
	// if the enemy changes direction (normDirection: closest to -> up, down, left, right) we change the animation
	if (m_lastNormDir != normDirection)
	{
		onEnter(controller, normDirection);
	}
}

void ChaseStateEnemy::move(EnemyController& controller, Vector2<float> enemyToPlayerVec)
{
	normalizeVec(enemyToPlayerVec);
	// move enemy in player direction
	controller.getGObject().getRigidbody()->velocity = enemyToPlayerVec * controller.getSpeed();
}

void ChaseStateEnemy::handleState(EnemyController& controller, const Vector2i& normDirection, const float& deltaTime)
{
	Vector2<float> enemyToPlayerVec;

	if (isDead(controller)) return;
	if (isOutOfRange(controller, enemyToPlayerVec)) return;

	checkDirectionChange(controller, normDirection);
	move(controller, enemyToPlayerVec);
}

void ChaseStateEnemy::onEnter(EnemyController & controller, const Vector2i& normDirection)
{
	playDirectionalAnimation(controller, normDirection);
	m_lastNormDir = normDirection;
}
