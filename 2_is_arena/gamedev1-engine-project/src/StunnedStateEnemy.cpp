#include "stdafx.h"
#include "StunnedStateEnemy.h"
#include "AnimationManager.h"
#include "EnemyController.h"
#include "PlayFieldManager.h"

bool StunnedStateEnemy::isDead(EnemyController& controller)
{
	// enemy dies
	if (controller.isHpZero())
	{
		controller.ChangeState("dead");
		return true;
	}
	return false;
}

bool StunnedStateEnemy::isTimePassed(EnemyController& controller)
{
	// duration enemy is stunned
	if (m_timePassed >= m_stateDuration)
	{
		// boar's direction is inverted -> patrolling in the opposite direction the collision happened
		// else: idle direction is direction when collision happened
		if (controller.getGObject().getID().substr(0, 4) == "boar")
		{
			controller.getGObject().getRigidbody()->velocity = Vector2f(m_normDir * -1);
		}
		else
		{
			controller.getGObject().getRigidbody()->velocity = Vector2f(m_normDir);
		}
		
		controller.ChangeState("idle");
		m_timePassed = 0;
		return true;
	}
	return false;
}

void StunnedStateEnemy::handleState(EnemyController& controller, const Vector2i& normDirection, const float& deltaTime)
{
	if (isDead(controller)) return;
	if (isTimePassed(controller)) return;

	m_timePassed += deltaTime;
}

void StunnedStateEnemy::onEnter(EnemyController & controller, const Vector2i& normDirection)
{
	playDirectionalAnimation(controller, normDirection);
	m_normDir = normDirection;
	controller.getGObject().getRigidbody()->velocity = Vector2f(0, 0);
}
