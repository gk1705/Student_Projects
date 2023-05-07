#include "stdafx.h"
#include "IObserverComponent.h"
#pragma once

class GameObject;

class CenterObserver : public IObserverComponent
{
public:
	void notify(GameObject& gameObject) override;

private:
	int m_counter = 0;
};