#include "stdafx.h"
#pragma once
#include "APlayerState.h"

class IdleState : public APlayerState
{
public:
	IdleState(const std::string& animName, const float frameDuration)
		: APlayerState(animName, frameDuration)
	{
	}

	void handleState(HumanController& humanController, Vector2f tilt, Vector2i dir, float deltaTime) override;
	void onEnter(HumanController& humanController, const Vector2i& dirLeft, const Vector2i& dirRight) override;

private:
	bool isDead(HumanController& humanController);
	bool isAttacking(HumanController& humanController);
	bool isMoving(HumanController& humanController, Vector2f tilt);
};