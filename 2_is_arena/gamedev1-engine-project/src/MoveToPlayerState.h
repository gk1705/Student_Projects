#include "stdafx.h"
#pragma once
#include "IAiState.h"

class MoveToPlayerState : public IAiState
{
public:
	void handleState(AIController& controller) override;
};