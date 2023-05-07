#pragma once
#include "AController.h"
#include "IAiState.h"
#include "MoveToPlayerState.h"
#include "PlayBallState.h"
#include "DanceState.h"

class AIController : public AController
{
public:
	AIController(GameObject& gameObject, shared_ptr<PlayerStatsComponent> stats)
		:AController(gameObject, stats) 
	{
		m_currentState = make_unique<MoveToPlayerState>(state_moveToPlayer);
	}

	void Update(const float& fDeltaTime) override;
	void ChangeState(unique_ptr<IAiState> state);

public:
	static MoveToPlayerState state_moveToPlayer;
	static PlayBallState state_playBall;
	static DanceState state_Dance;

private:
	unique_ptr<IAiState> m_currentState;
};
