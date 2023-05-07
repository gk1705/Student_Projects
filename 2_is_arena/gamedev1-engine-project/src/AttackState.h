#include "stdafx.h"
#pragma once
#include "APlayerState.h"
#include "InputManager.h"

class AttackState : public APlayerState
{
public:
	AttackState(const std::string& animName, const float frameDuration)
		: APlayerState(animName, frameDuration)
	{
	}

	void handleState(HumanController& humanController, Vector2f tilt, Vector2i dir, float deltaTime) override;
	void onEnter(HumanController& humanController, const Vector2i& dirLeft, const Vector2i& dirRight) override;

private:
	bool isDead(HumanController& humanController);
	bool isTimePassed(HumanController& humanController);

	float switchTime = 0.3;
	float timePassed = 0;
};