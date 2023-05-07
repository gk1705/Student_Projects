#include "stdafx.h"
#include "AttackState.h"
#include "AnimationManager.h"
#include "HumanController.h"

bool AttackState::isDead(HumanController& humanController)
{
	// is player dead?
	if (humanController.isHpZero())
	{
		humanController.ChangeState("death");
		return true;
	}
	return false;
}

bool AttackState::isTimePassed(HumanController& humanController)
{
	// attack animation is coupled with this state, while attacking itself is handled separately for the player characters
	// we use this state to determine how long the attack animation should be played
	if (timePassed > switchTime)
	{
		humanController.ChangeState("idle");
		timePassed = 0;
		return true;
	}
	return false;
}

void AttackState::handleState(HumanController& humanController, Vector2f tilt, Vector2i dir, float deltaTime)
{
	if (isDead(humanController)) return;
	if (isTimePassed(humanController)) return;

	timePassed += deltaTime;
}

void AttackState::onEnter(HumanController& humanController, const Vector2i& dirLeft, const Vector2i& dirRight)
{
	playDirectionalAnimation(humanController, dirRight);
}
