#include "stdafx.h"
#pragma once
#include "MoveToPlayerState.h"
#include "GameObjectManager.h"
#include "AIController.h"

void MoveToPlayerState::handleState(AIController& controller)
{
	Vector2f targetPos = GameObjectManager::getInstance().getGameObject("Player1")->GetPosition();

	sf::Vector2f desiredVelocity = targetPos - controller.getGObject().GetPosition();
	normalizeVec(desiredVelocity);
	desiredVelocity.x *= controller.getSpeed();
	desiredVelocity.y *= controller.getSpeed();

	controller.getGObject().getRigidbody()->velocity = desiredVelocity;
}