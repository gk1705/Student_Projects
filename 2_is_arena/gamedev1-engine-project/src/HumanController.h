#pragma once
#include <utility>
#include "AController.h"
#include "APlayerState.h"
#include "IdleState.h"
#include "MovementState.h"
#include "AttackState.h"
#include "DeathState.h"
#include "HitState.h"
#include <unordered_map>

class HumanController : public AController
{
public:
	HumanController(GameObject& gameObject, string hero, shared_ptr<PlayerStatsComponent> stats)
		:AController(gameObject, stats)
		, m_hero(std::move(hero))
		, m_currentState(nullptr)
	{
		m_lastDirNormLeft = Vector2i(0, 1);
		m_lastDirNormRight = Vector2i(0, 1);

		m_states.insert({ "idle", make_shared<IdleState>(IdleState("idle", 0.2f)) });
		m_states.insert({ "movement", make_shared<MovementState>(MovementState("walk", 0.2f)) });
		m_states.insert({ "attack", make_shared<AttackState>(AttackState("attack", 0.09f)) });
		m_states.insert({ "death", make_shared<DeathState>(DeathState("die", 0.4f)) });
		m_states.insert({ "hit", make_shared<HitState>("hit", 0.5f, 0.5f) });

		m_currentState = m_states["idle"];
	}

	void Update(const float& fDeltaTime) override;
	void ChangeState(const string& stateName) override;

	float getAttackSpeed() const;
	float getAttackSpeedMAX() const;

	void LockPlayer();

private:
	void unlockedUpdate(const float& fDeltaTime);
	void lockedUpdate(const float& fDeltaTime);

	void SpawnProjectile(Vector2f velocity);
	bool checkShooting();
	bool checkUltimate();
	void UseUltimate();
	void shooting();
	void handleMovement();
	void getRightJoystickDir();

	float m_attackspeed = 0;
	float m_attackspeedMAX = 0.6f;
	int m_count = 0;
	bool lock = false;
	string m_hero;

	unordered_map<string, shared_ptr<APlayerState>> m_states;
	shared_ptr<APlayerState> m_currentState;

	Vector2i m_lastDirNormLeft;
	Vector2i m_lastDirNormRight;
	Vector2f m_lastDirLeft;
	Vector2f m_lastDirRight;
};
