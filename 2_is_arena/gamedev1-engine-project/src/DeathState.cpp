#include "stdafx.h"
#pragma once
#include "DeathState.h"
#include "InputManager.h"
#include "AnimationManager.h"
#include "HumanController.h"
#include "PlayFieldManager.h"
#include "GameStateManager.h"
#include "SoundManager.h"

void DeathState::handleState(HumanController& humanController, Vector2f tilt, Vector2i dir, float deltaTime)
{
	// after the death animation has been finished, set gamestate to gameOver;
	if (timePassed >= switchTime)
	{
		GameStateManager::getInstance().setState("GameOver");
		timePassed = 0;
	}

	timePassed += deltaTime;
}

void DeathState::onEnter(HumanController& humanController, const Vector2i& dirLeft, const Vector2i& dirRight)
{
	playDirectionalAnimation(humanController, dirLeft);

	SoundManager::getInstance().PlaySound("Player_hit" + std::to_string(generateIntFromTo(1, 8)), "second");
	humanController.LockPlayer();

	incrementScore(humanController);
}

void DeathState::incrementScore(const HumanController& humanController)
{
	// score of winner is elevated
	if (humanController.getGObject().getID() == "Player1") {
		PlayFieldManager::getInstance().getScore(1)++;
	}
	if (humanController.getGObject().getID() == "Player2") {
		PlayFieldManager::getInstance().getScore(0)++;
	}
}
