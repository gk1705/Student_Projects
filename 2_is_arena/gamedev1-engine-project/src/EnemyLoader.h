#pragma once
#include "stdafx.h"
#include "GameObject.h"
#include "AnimationComponent.h"
#include "EnemyController.h"
#include "HealthBarComponent.h"

using namespace std;
using namespace sf;

namespace {
	struct enemyBreed {
		virtual ~enemyBreed() = default;
		virtual void init() = 0;

		int HP = 100;
		float movementspeed = 2;
	};

	struct bat : public enemyBreed {
		void init() override
		{
			HP = 50;
			movementspeed = 125;
		}
	};

	struct boar : public enemyBreed {
		void init() override
		{
			int HP = 100;
			movementspeed = 75.0;
		}
	};

	struct bee : public enemyBreed {
		void init() override
		{

		}
	};

	struct slime : public enemyBreed {
		void init() override
		{

		}
	};
}

// factory;
class EnemyLoader
{

public:
	static shared_ptr<GameObject> LoadEnemy(const string& enemyType, const string& ID, Vector2f position);

private:
	static void initHealthbarAndStats(const shared_ptr<GameObject>& enemyObject, const string& enemyName);
	static void initEnemySpriteSheet(const string& enemyType, Image& colliderReferenceImage, shared_ptr<Texture>& texture);
	static void initEnemyAnimation(const string& enemyType, const shared_ptr<GameObject>& enemyObject,
		const Image& colliderReferenceImage, shared_ptr<Texture> texture, sf::FloatRect& collider);
	static void initEnemyPhysics(const shared_ptr<GameObject>& enemyObject, const Image& colliderReferenceImage, sf::FloatRect collider);
	static void initEnemyMovement(const string& enemyType, const shared_ptr<GameObject>& enemyObject);
	static void addEnemyToManagers(const shared_ptr<GameObject>& enemyObject);

	static void loadSpriteSheet(Image& spriteSheet, Image& colliderReferenceImage, const string& monsterName);
	static void loadAnimationComponent(const shared_ptr<GameObject>& enemyObject, shared_ptr<Texture> texture, const string
	                                   & enemyName);
	static void loadAnimations(const shared_ptr<AnimationComponent>& animationComponent, const string& enemyName);
	static shared_ptr<EnemyController> loadController(const shared_ptr<GameObject>& enemyObject, shared_ptr<PlayerStatsComponent> statsComponent, const string
	                                                  & enemyName);
	static void createEnemyType(const string& enemyName, shared_ptr<enemyBreed>& monster);
	static void configureEnemyStats(const shared_ptr<GameObject>& enemyObject, const shared_ptr<enemyBreed>& enemyType,
	                                shared_ptr<HealthBarComponent>& healthbarcomp,
	                                shared_ptr<PlayerStatsComponent>& enemyStats);
};