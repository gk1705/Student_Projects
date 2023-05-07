#pragma once
#include "GameObject.h"
#include "PlayerStatsComponent.h"

using namespace std;
using namespace sf;

class ProjectileSpawner
{
public:
	static void spawnProjectile(GameObject& gameObject, const string& heroType, int& projectileCounter, const int&
	                            controllerID, const PlayerStatsComponent& playerStats, Vector2f velocity);

	static void spawnUltimate(GameObject& gameObject, const string& heroType, int& projectileCounter, const int&
		m_id, const PlayerStatsComponent& playerStats);
};