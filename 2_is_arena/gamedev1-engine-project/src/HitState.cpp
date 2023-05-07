#include "stdafx.h"
#include "HitState.h"
#include "AnimationManager.h"
#include "HumanController.h"
#include "SoundManager.h"

bool HitState::isDead(HumanController& humanController)
{
	// player dies while getting hit
	if (humanController.isHpZero())
	{
		humanController.ChangeState("death");
		return true;
	}
	return false;
}

bool HitState::isAttacking(HumanController& humanController)
{
	// hitting should not prevent attacking
	// if we press attack and out attack is available due to attack speed threshold -> we switch into attack state
	// attack state is used to play the animation -> actual attack is handeled seperately in the controller (only for player)
	if (InputManager::getInstance().IsButtonPressed("RB", humanController.getID()) 
		&& humanController.getAttackSpeed() >= humanController.getAttackSpeedMAX())
	{
		humanController.ChangeState("attack");
		return true;
	}
	return false;
}

bool HitState::isTimePassed(HumanController& humanController)
{
	// we return to idle after a set duration has passed
	// maybe switch into movement based on current velocity...
	if (m_timePassed >= m_stateDuration)
	{
		humanController.ChangeState("idle");
		m_timePassed = 0;
		return true;
	}
	return false;
}

void HitState::handleState(HumanController& humanController, Vector2f tilt, Vector2i dir, float deltaTime)
{
	if (isDead(humanController)) return;
	if (isAttacking(humanController)) return;
	if (isTimePassed(humanController)) return;

	m_timePassed += deltaTime;
}

void HitState::onEnter(HumanController& humanController, const Vector2i& dirLeft, const Vector2i& dirRight)
{
	playDirectionAnimation(humanController, dirLeft);
}

void HitState::playDirectionAnimation(const HumanController& humanController, const Vector2i& dirLeft)
{
	if (dirLeft == Vector2i(1, 0))
	{
		AnimationManager::getInstance().GetComponent(humanController.getGObject().getID())->playAnimation("hit_front", 0.5);
	}
	else if (dirLeft == Vector2i(-1, 0))
	{
		AnimationManager::getInstance().GetComponent(humanController.getGObject().getID())->playAnimation("hit_front", 0.5);
	}
	else if (dirLeft == Vector2i(0, 1))
	{
		AnimationManager::getInstance().GetComponent(humanController.getGObject().getID())->playAnimation("hit_front", 0.5);
	}
	else if (dirLeft == Vector2i(0, -1))
	{
		AnimationManager::getInstance().GetComponent(humanController.getGObject().getID())->playAnimation("hit_front", 0.5);
	}
	else
		throw std::invalid_argument("received wrong or no direction.");
}
