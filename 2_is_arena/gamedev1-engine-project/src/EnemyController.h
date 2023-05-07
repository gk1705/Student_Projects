#pragma once
#include "AController.h"
#include "AEnemyState.h"
#include <unordered_map>

class EnemyController : public AController
{
public:
	EnemyController(GameObject& gameObject, shared_ptr<AEnemyState> currentState, shared_ptr<PlayerStatsComponent> stats)
		: AController(gameObject, stats)
		, m_currentState(currentState)
		, m_lastDirNorm(Vector2i(0, 1))
	{
		if (!m_currentState)
		{
			throw std::invalid_argument("Current State of entity was null.");
		}

		//adding idle by default;
		AddState(currentState, "idle");
	}

	void Update(const float& fDeltaTime) override;
	void ChangeState(const string& stateName) override;
	void AddState(const shared_ptr<AEnemyState>& state, const string& stateName);
	void setNormDir(const Vector2f& inputVector);
	bool isCollidingBorder() const;

	Vector2i getNormDir() const;

private:
	shared_ptr<AEnemyState> m_currentState;
	unordered_map<string, shared_ptr<AEnemyState>> m_states;

	bool horizontalCollisionCheck(Rect<float> borderRect, const shared_ptr<BoxColliderComponent>& collisioncomp) const;
	bool verticalCollisionCheck(Rect<float> borderRect, const shared_ptr<BoxColliderComponent>& collisioncomp) const;

	Vector2i m_lastDirNorm;
};