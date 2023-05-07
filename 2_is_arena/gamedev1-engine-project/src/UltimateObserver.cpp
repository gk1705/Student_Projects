#include "stdafx.h"
#include "GameObject.h"
#include "UltimateObserver.h"
#include "PlayerStatsComponent.h"

UltimateObserver::UltimateObserver(shared_ptr<tgui::Panel> ultimatebar)
	: m_ultimatebar(ultimatebar)
{

}

void UltimateObserver::notify(GameObject& gameObject)
{
	const auto MAX_BAR_SIZE = 500;
	auto scale = gameObject.getComponent<PlayerStatsComponent>()->getUltimate() / 100.0f;

	//Dont allow bar to get bigger/smaller than background_bar
	if (scale < 0) scale = 0;
	if (scale > 1) scale = 1;

	m_ultimatebar->setSize(scale * MAX_BAR_SIZE, 50);
}