#include "stdafx.h"
#pragma once
#include "PlayBallState.h"
#include "GameObjectManager.h"
#include "AIController.h"

void PlayBallState::handleState(AIController& controller)
{
	auto ball = GameObjectManager::getInstance().getGameObjectsByType("BallObject")[0];
	Vector2f dir2 = controller.getGObject().getRigidbody()->getPosition();

	/// <summary>	
	/// ball is facing the opposite direction -> change state
	/// </summary>
	if (dotProduct(ball->getRigidbody()->velocity, dir2) < 0)
	{
		std::cout << "Changed state to: gather items" << std::endl;
		controller.ChangeState(make_unique<MoveToPlayerState>(AIController::state_moveToPlayer));
		return;
	}

	/// <summary>
	/// steer in right direction
	/// </summary>
	Vector2f impulsePlayerDirection = controller.getGObject().GetPosition() - ball->GetPosition();
	normalizeVec(impulsePlayerDirection);
	ball->getRigidbody()->m_impulses.push_back(impulsePlayerDirection);
}
