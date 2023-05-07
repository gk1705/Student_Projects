#include "stdafx.h"
#pragma once
#include "APlayerState.h"

class MovementState : public APlayerState
{
public:
	MovementState(const std::string& animName, const float frameDuration)
		: APlayerState(animName, frameDuration)
	{
	}

	void handleState(HumanController& humanController, Vector2f tilt, Vector2i dir, float deltaTime) override;
	void onEnter(HumanController& humanController, const Vector2i& dirLeft, const Vector2i& dirRight) override;

private:
	bool isDead(HumanController& humanController);
	bool isAttacking(HumanController& humanController);
	bool isNotMoving(HumanController& humanController, Vector2f tilt);
	void playFootstepSound();
	void checkDirectionChange(HumanController& humanController, Vector2i dir);

	Vector2i m_lastNormDir;
};