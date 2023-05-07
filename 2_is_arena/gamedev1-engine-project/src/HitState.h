#include "stdafx.h"
#pragma once
#include "APlayerState.h"
#include "InputManager.h"

class HitState : public APlayerState
{
public:
	HitState(const std::string& animName, const float frameDuration, const float state_duration)
		: APlayerState(animName, frameDuration)
		, m_stateDuration(state_duration)
		, m_timePassed(0)
	{
	}

	void handleState(HumanController& humanController, Vector2f tilt, Vector2i dir, float deltaTime) override;
	void onEnter(HumanController& humanController, const Vector2i& dirLeft, const Vector2i& dirRight) override;

private:
	bool isDead(HumanController& humanController);
	bool isAttacking(HumanController& humanController);
	bool isTimePassed(HumanController& humanController);
	void playDirectionAnimation(const HumanController& humanController, const Vector2i& dirLeft);

	float m_stateDuration;
	float m_timePassed;
};