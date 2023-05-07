#include "stdafx.h"
#pragma once
#include "AEnemyState.h"

class IdleStateEnemy : public AEnemyState
{
public:
	IdleStateEnemy(const std::string& animName, const float frameDuration, const Vector2f& activationRange)
		: AEnemyState(animName, frameDuration),
		  m_activationRange(activationRange)
	{
	}

	void handleState(EnemyController& controller, const Vector2i& normDirection, const float& deltaTime) override;
	void onEnter(EnemyController& controller, const Vector2i& normDirection) override;

private:
	bool isDead(EnemyController& controller);
	bool switchToChaseState(EnemyController& controller, Vector2<float> enemyToPlayerVec);
	bool isTargetInRange(EnemyController& controller);

	Vector2f m_activationRange;
};