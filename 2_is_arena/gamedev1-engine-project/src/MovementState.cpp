#include "stdafx.h"
#include "MovementState.h"
#include "InputManager.h"
#include "AnimationManager.h"
#include "HumanController.h"
#include "SoundManager.h"

bool MovementState::isDead(HumanController& humanController)
{
	// player dies
	if (humanController.isHpZero())
	{
		humanController.ChangeState("death");
		return true;
	}
	return false;
}

bool MovementState::isAttacking(HumanController& humanController)
{
	// player attacks
	if (InputManager::getInstance().IsButtonPressed("RB", humanController.getID()) && humanController.getAttackSpeed() >= humanController.getAttackSpeedMAX())
	{
		humanController.ChangeState("attack");
		return true;
	}
	return false;
}

bool MovementState::isNotMoving(HumanController& humanController, Vector2f tilt)
{
	// player seizes to move
	if (tilt.x == 0 && tilt.y == 0)
	{
		humanController.ChangeState("idle");
		return true;
	}
	return false;
}

void MovementState::playFootstepSound()
{
	//Play sound if none is playing!
	if (!SoundManager::getInstance().SoundIsCurrentlyPlaying())
		SoundManager::getInstance().PlaySound("Player_movement" + std::to_string(generateIntFromTo(1, 10)), "first");
}

void MovementState::checkDirectionChange(HumanController& humanController, Vector2i dir)
{
	// if the player changes the direction -> update animation to fit that direction
	if (m_lastNormDir != dir)
	{
		onEnter(humanController, dir, Vector2i(0, 0));
	}
}

// movement is decoupled from this state -> actually only for the move animation
void MovementState::handleState(HumanController& humanController, Vector2f tilt, Vector2i dir, float deltaTime)
{
	playFootstepSound();
	if (isDead(humanController)) return;
	if (isAttacking(humanController)) return;
	if (isNotMoving(humanController, tilt)) return;
	checkDirectionChange(humanController, dir);
}

void MovementState::onEnter(HumanController& humanController, const Vector2i& dirLeft, const Vector2i& dirRight)
{
	playDirectionalAnimation(humanController, dirLeft);
	m_lastNormDir = dirLeft;
}
