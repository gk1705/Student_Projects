#include "stdafx.h"
#pragma once
#include "GameObject.h"
#include "CenterObserver.h"

void CenterObserver::notify(GameObject& gameObject)
{
	if (gameObject.getType() == "BallObject") {
		printf("Passed the center counter: %i\n", ++m_counter);
	}
}