#pragma once
#include "stdafx.h"
#include "GameObject.h"
#include "AnimationComponent.h"
#include "PlayerStatsComponent.h"

using namespace std;
using namespace sf;

namespace {
	int countid = 1; // 0 is for input-types like "changeGameState"

	struct playerType {
		virtual ~playerType() = default;
		virtual void init() = 0;

		int HP = 100;
		float movementspeed = 2;
		float attackSpeed = 1;
		int projectileSpeed = 10;
	};

	struct witch : public playerType {
		void init() override {
			movementspeed = 3.5;
			projectileSpeed = 25;
			attackSpeed = 0.45;
		}
	};

	struct archer : public playerType {
		void init() override {
			movementspeed = 3.5;
			projectileSpeed = 35;
			attackSpeed = 0.7;
		}
	};
}

class HeroLoader
{
public:
	static void LoadHeros(const string& Hero1, const string& Hero2);

private:
	static void initBorder(const initializer_list<const shared_ptr<GameObject>> listOfPlayers);

	static void initSpriteSheet(const string& heroName, Image& colliderReferenceImage, shared_ptr<Texture>& texture);
	static void initAnimations(const string& heroName, const shared_ptr<GameObject>& gameObject,
		const Image& colliderReferenceImage, shared_ptr<Texture> texture, sf::FloatRect& collider);
	static void initPhysics(const string& heroName, const shared_ptr<GameObject>& gameObject, sf::FloatRect collider);
	static void initHealthbarAndStats(const shared_ptr<GameObject>& gameObject, const shared_ptr<playerType>& hero,
		shared_ptr<PlayerStatsComponent>& playerstats);
	static void createPlayerType(const string& heroName, shared_ptr<playerType>& heroType);
	static void initMovement(const shared_ptr<GameObject>& gameObject, shared_ptr<PlayerStatsComponent> playerstats, const string& heroName);
	static void addToManagers(const shared_ptr<GameObject>& gameObject);

	static shared_ptr<GameObject> CreateHero(const string& player, const string& heroName, const Vector2f& position);
	static void loadSpriteSheet(Image& spriteSheet, Image& colliderReferenceImage, const string& objectName);
	static void loadAnimationComponent(const shared_ptr<GameObject>& gameObject, const shared_ptr<Texture> texture, const string
	                                   & objectName);
	static void loadAnimations(const shared_ptr<AnimationComponent>& animationComponent, const string& objectName);
	static void correctPlayerCollider(FloatRect& collider, const string& objectName);
	static Vector2f getPlayerOffset(const string& objectName);
};