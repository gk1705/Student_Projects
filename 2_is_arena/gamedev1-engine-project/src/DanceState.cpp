#include "stdafx.h"
#pragma once
#include "DanceState.h"
#include "AIController.h"

namespace
{
	const float  PI_F = 3.14159265358979f;
}

void DanceState::handleState(AIController& controller)
{
	// dance wildly
	float size = (rand() % 100) + 20;
	controller.getGObject().setHeight(size);
	auto pos = controller.getGObject().GetPosition();

	float x = pos.x + 15  *cos(time);
	float y = pos.y + 15 * sin(time);
	time += 0.10;
	controller.getGObject().setPosition(Vector2f(x, y));
}
