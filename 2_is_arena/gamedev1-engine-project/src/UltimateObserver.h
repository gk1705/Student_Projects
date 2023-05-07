#pragma once
#include "IObserverComponent.h"
#include "HealthBarComponent.h"
#include "TGUI\TGUI.hpp"

class GameObject;

class UltimateObserver : public IObserverComponent
{
public:
	UltimateObserver(shared_ptr<tgui::Panel> ultimatebar);
	void notify(GameObject& gameObject) override;

private:
	shared_ptr<tgui::Panel> m_ultimatebar;
};