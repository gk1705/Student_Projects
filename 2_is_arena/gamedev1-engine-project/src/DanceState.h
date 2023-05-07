#include "stdafx.h"
#include "IAiState.h"
#pragma once

class DanceState : public IAiState
{
public:
	void handleState(AIController& controller) override;

	float time = 0;
};