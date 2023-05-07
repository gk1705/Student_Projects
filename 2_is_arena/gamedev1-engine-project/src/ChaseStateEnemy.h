#include "stdafx.h"
#pragma once
#include "AEnemyState.h"

class ChaseStateEnemy : public AEnemyState
{
public:
	ChaseStateEnemy(const std::string& animName, const float frameDuration, const Vector2f& deactivationRange)
		: AEnemyState(animName, frameDuration),
		  m_deactivationRange(deactivationRange),
		  m_lastNormDir(Vector2i(1, 0))
	{
	}

	void handleState(EnemyController& controller, const Vector2i& normDirection, const float& deltaTime) override;
	void onEnter(EnemyController& controller, const Vector2i& normDirection) override;

private:
	bool isDead(EnemyController& controller);
	bool isOutOfRange(EnemyController& controller, Vector2<float>& enemyToPlayerVec);
	void checkDirectionChange(EnemyController& controller, const Vector2i& normDirection);
	void move(EnemyController& controller, Vector2<float> enemyToPlayerVec);

	Vector2f m_deactivationRange;
	Vector2i m_lastNormDir;
};