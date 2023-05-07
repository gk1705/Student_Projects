#include "stdafx.h"
#pragma once
#include "InputManager.h"
#include "AEnemyState.h"

class DeathStateEnemy : public AEnemyState
{
public:
	DeathStateEnemy(const std::string& animName, const float frameDuration, const float switch_time)
		: AEnemyState(animName, frameDuration)
		, m_switchTime(switch_time)
		, m_timePassed(0)
	{
	}

	void handleState(EnemyController& controller, const Vector2i& normDirection, const float& deltaTime) override;
	void onEnter(EnemyController& controller, const Vector2i& normDirection) override;

private:
	void switchPlayfield(EnemyController& controller, string& playField);
	void setSpawnPosition(EnemyController& controller, string playField);
	void resetObject(EnemyController& controller);
	void triggerDeathSound(EnemyController& controller);

	float m_switchTime;
	float m_timePassed;
};