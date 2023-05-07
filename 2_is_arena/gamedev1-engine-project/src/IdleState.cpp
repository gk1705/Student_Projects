#include "stdafx.h"
#include "IdleState.h"
#include "InputManager.h"
#include "AnimationManager.h"
#include "HumanController.h"

bool IdleState::isDead(HumanController& humanController)
{
	if (humanController.isHpZero())
	{
		humanController.ChangeState("death");
		return true;
	}
	return false;
}

bool IdleState::isAttacking(HumanController& humanController)
{
	// player presses attack button + enough time has passed to fulfill attackspeed threshold
	if (InputManager::getInstance().IsButtonPressed("RB", humanController.getID()) 
		&& humanController.getAttackSpeed() >= humanController.getAttackSpeedMAX())
	{
		humanController.ChangeState("attack");
		return true;
	}
	return false;
}

bool IdleState::isMoving(HumanController& humanController, Vector2f tilt)
{
	// left stick is tilted beyond dead zone (input from left stick isn't Vector2f(0, 0))
	if (tilt.x != 0 || tilt.y != 0)
	{
		humanController.ChangeState("movement");
		return true;
	}
	return false;
}

void IdleState::handleState(HumanController& humanController, Vector2f tilt, Vector2i dir, float deltaTime)
{
	if (isDead(humanController)) return;
	if (isAttacking(humanController)) return;
	if (isMoving(humanController, tilt)) return;
}

void IdleState::onEnter(HumanController& humanController, const Vector2i& dirLeft, const Vector2i& dirRight)
{
	playDirectionalAnimation(humanController, dirLeft);
}
