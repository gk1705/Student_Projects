#include "stdafx.h"
#pragma once

class GatherItemsState;
class AIController;

class IAiState
{
public:
	virtual void handleState(AIController& controller) = 0;
	virtual ~IAiState() 
	{

	}
};