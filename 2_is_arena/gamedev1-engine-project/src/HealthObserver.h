#pragma once
#include "IObserverComponent.h"
#include "HealthBarComponent.h"

class GameObject;

class HealthObserver : public IObserverComponent
{
public:
	HealthObserver(shared_ptr<HealthBarComponent> healtbarcomponent);
	void notify(GameObject& gameObject) override;

private:
	shared_ptr<HealthBarComponent> m_healthbarcomp;
};