#include "stdafx.h"
#pragma once
#include "IAiState.h"

class PlayBallState : public IAiState
{
public:
	void handleState(AIController& controller) override;
};