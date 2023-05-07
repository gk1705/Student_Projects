#include "stdafx.h"
#pragma once
#include "InputManager.h"
#include "AEnemyState.h"

class PatrolingStateEnemy : public AEnemyState
{
public:
	PatrolingStateEnemy(const std::string& animName, const float frameDuration, const float speed_modifier)
		: AEnemyState(animName, frameDuration)
		, m_lastNormDir(Vector2i(1, 0))
		, m_speedModifier(speed_modifier)
		, m_enemyInSight(false)
	{
	}

	void handleState(EnemyController& controller, const Vector2i& normDirection, const float& deltaTime) override;
	void onEnter(EnemyController& controller, const Vector2i& normDirection) override;

private:
	bool isDead(EnemyController& controller);
	bool isCollidingBorder(EnemyController& controller);
	void checkSightOverlap(EnemyController& controller, shared_ptr<GameObject>& target);
	bool isInSight(EnemyController& controller, shared_ptr<GameObject> target);
	void checkDirectionChange(EnemyController& controller, const Vector2i& normDirection);
	void move(EnemyController& controller);

	Vector2i m_lastNormDir;
	float m_speedModifier;
	bool m_enemyInSight;
};