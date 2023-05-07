#pragma once
#include "InputManager.h"
#include "AEnemyState.h"

class StunnedStateEnemy : public AEnemyState
{
public:
	StunnedStateEnemy(const std::string& animName, const float frameDuration, const float state_duration)
		: AEnemyState(animName, frameDuration)
		, m_stateDuration(state_duration)
		, m_timePassed(0)
		, m_normDir(Vector2i(1, 0))
	{
	}

	bool isDead(EnemyController& controller);
	bool isTimePassed(EnemyController& controller);
	void handleState(EnemyController& controller, const Vector2i& normDirection, const float& deltaTime) override;
	void onEnter(EnemyController& controller, const Vector2i& normDirection) override;

private:
	float m_stateDuration;
	float m_timePassed;

	Vector2i m_normDir;
};