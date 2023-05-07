#include "stdafx.h"
#include "GameObjectManager.h"

void AIController::Update(float fDeltaTime)
{
	m_currentState->handleState(*this);
}

void AIController::ChangeState(unique_ptr<IAiState> state)
{
	m_currentState = std::move(state);
}

MoveToPlayerState AIController::state_moveToPlayer;
PlayBallState AIController::state_playBall;
DanceState AIController::state_Dance;