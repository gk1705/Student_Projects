#pragma once
#include "stdafx.h"
#include "IObserverComponent.h"

class GameObject;

class HealAreaCollisionObserver : public IObserverComponent
{
public:
	void notify(GameObject& gameObject) override;
};