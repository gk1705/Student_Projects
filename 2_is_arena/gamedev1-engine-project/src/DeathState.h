#include "stdafx.h"
#pragma once
#include "APlayerState.h"
#include "InputManager.h"

class DeathState : public APlayerState
{
public:
	DeathState(const std::string& animName, const float frameDuration)
		: APlayerState(animName, frameDuration)
	{
	}

	void handleState(HumanController& humanController, Vector2f tilt, Vector2i dir, float deltaTime) override;
	void onEnter(HumanController& humanController, const Vector2i& dir, const Vector2i& dirRight) override;

private:
	void incrementScore(const HumanController& humanController);

	float switchTime = 1;
	float timePassed = 0;
};