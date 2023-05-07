//#include "stdafx.h"
//#pragma once
//#include "AEnemyState.h"
//#include "InputManager.h"
//
//class CircleAroundStateEnemy : public AEnemyState
//{
//	// unused state
//public:
//	CircleAroundStateEnemy(const std::string& animName, float frameDuration, float radius, float rotation_degree)
//		: AEnemyState(animName, frameDuration),
//		  m_radius(radius),
//		  m_rotationDegree(rotation_degree)
//	{
//	}
//
//	void handleState(EnemyController& controller, const Vector2i& normDirection, const float& deltaTime) override;
//	void onEnter(EnemyController& controller, const Vector2i& normDirection) override;
//
//private:
//	Vector2i m_lastNormDir;
//	float m_radius;
//	float m_rotationDegree;
//};