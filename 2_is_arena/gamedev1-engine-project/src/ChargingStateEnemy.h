#include "stdafx.h"
#pragma once
#include "InputManager.h"
#include "AEnemyState.h"

class ChargingStateEnemy : public AEnemyState
{
public:

	ChargingStateEnemy(const std::string& animName, const float frameDuration, const float speed_modifier)
		: AEnemyState(animName, frameDuration),
		  m_speedModifier(speed_modifier)
	{
	}

	void handleState(EnemyController& controller, const Vector2i& normDirection, const float& deltaTime) override;
	void onEnter(EnemyController& controller, const Vector2i& normDirection) override;

private:
	bool isDead(EnemyController& controller);
	bool isCollidingBorder(EnemyController& controller);
	void move(EnemyController& controller, const Vector2i& normDirection);

	float m_speedModifier;
};