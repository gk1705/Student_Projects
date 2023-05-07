#include "stdafx.h"
#include "EnemyLoader.h"
#include "AnimationManager.h"
#include "EnemyController.h"
#include "MovementComponent.h"
#include "IdleStateEnemy.h"
#include "ChaseStateEnemy.h"
#include "DeathStateEnemy.h"
#include "RenderManager.h"
#include "UpdateManager.h"
#include "GameObjectManager.h"
#include "PlayerStatsComponent.h"
#include "HealthBarComponent.h"
#include "HealthObserver.h"
#include "MonsterCollisionObserver.h"
#include "PatrolingStateEnemy.h"
#include "StunnedStateEnemy.h"
#include "ChargingStateEnemy.h"
#include "CircleAroundStateEnemy.h"

shared_ptr<GameObject> EnemyLoader::LoadEnemy(const string& enemyType, const string& ID, Vector2f position)
{
	shared_ptr<GameObject> enemyObject = make_shared<GameObject>(ID, position);
	enemyObject->setType("Enemy");

	Image colliderReferenceImage;
	shared_ptr<Texture> texture;
	sf::FloatRect collider;

	initEnemySpriteSheet(enemyType, colliderReferenceImage, texture);
	initEnemyAnimation(enemyType, enemyObject, colliderReferenceImage, texture, collider);
	initEnemyPhysics(enemyObject, colliderReferenceImage, collider);
	initHealthbarAndStats(enemyObject, enemyType);
	initEnemyMovement(enemyType, enemyObject);
	addEnemyToManagers(enemyObject);

	return enemyObject;
}

void EnemyLoader::initEnemySpriteSheet(const string& enemyType, Image& colliderReferenceImage, shared_ptr<Texture>& texture)
{
	// load the sprite sheet based on enemyBreed
	Image spriteSheet;
	loadSpriteSheet(spriteSheet, colliderReferenceImage, enemyType);

	texture = make_shared<Texture>();
	texture->loadFromImage(spriteSheet);
}

void EnemyLoader::initEnemyAnimation(const string& enemyType, const shared_ptr<GameObject>& enemyObject, const Image&
                                     colliderReferenceImage, shared_ptr<Texture> texture, sf::FloatRect& collider)
{
	// load the animation component based on enemyBreed -> additionally define the animations
	loadAnimationComponent(enemyObject, texture, enemyType);

	collider = sf::FloatRect(enemyObject->GetPosition(), 
		sf::Vector2f(colliderReferenceImage.getSize().x*0.7f,
			colliderReferenceImage.getSize().y*0.7f));

	// add animation component to manager
	AnimationManager::getInstance().AddComponent(dynamic_pointer_cast<AnimationComponent>(enemyObject->getRenderComponentByIdx(0)), enemyObject->getID());
	dynamic_pointer_cast<AnimationComponent>(enemyObject->getRenderComponentByIdx(0))->playAnimation("idle_front", 0.2f);
}

void EnemyLoader::initEnemyPhysics(const shared_ptr<GameObject>& enemyObject, const Image& colliderReferenceImage, sf::FloatRect collider)
{
	// add rigidbody + boxCollider
	enemyObject->AddRigidbody(make_shared<RigidbodyComponent>(*enemyObject, 0));
	enemyObject->AddCollisionComponent(make_shared<BoxColliderComponent>(*enemyObject, collider, 
		Vector2f(colliderReferenceImage.getSize().x*0.15f, colliderReferenceImage.getSize().y*0.15f)));

	enemyObject->getRigidbody()->registerObserver(make_shared<MonsterCollisionObserver>(enemyObject->getID()));
}

void EnemyLoader::initEnemyMovement(const string& enemyType, const shared_ptr<GameObject>& enemyObject)
{
	shared_ptr<MovementComponent> movement = make_shared<MovementComponent>(*enemyObject);
	// load enemyBreed-specific controller -> statemachine
	const shared_ptr<EnemyController> enemyController = loadController(enemyObject, enemyObject->getComponent<PlayerStatsComponent>(), enemyType);

	movement->setStrategy(enemyController);
	enemyObject->AddMovementComponent(movement);
}

void EnemyLoader::addEnemyToManagers(const shared_ptr<GameObject>& enemyObject)
{
	RenderManager::getInstance().AddComponent(enemyObject->getRenderComponentByIdx(0), enemyObject->getID(), 2);// animations
	UpdateManager::getInstance().AddComponent(enemyObject->getRenderComponentByIdx(0), enemyObject->getID()); // animations
	RenderManager::getInstance().AddComponent(enemyObject->getRenderComponentByIdx(1), enemyObject->getID(), 2); // health bar

	GameObjectManager::getInstance().addGameObject(enemyObject);
}

// load sprite sheet based on enemy
// also load collider reference image (how to size the boxcollider)
void EnemyLoader::loadSpriteSheet(Image& spriteSheet, Image& colliderReferenceImage, const string& monsterName)
{
	if (monsterName == "bat")
	{
		if (!spriteSheet.loadFromFile("../assets/Images/bat_sheet.png")) {
			err() << "loadSprite: Could not load texture for sprite: " <<
				"../assets/Images/bat_sheet.png" << endl;
		}
		
		if (!colliderReferenceImage.loadFromFile("../assets/Images/bat_reference.png")) {
			err() << "loadSprite: Could not load texture for sprite: " <<
				"../assets/Images/bat_reference.png" << endl;
		}
	}
	else if (monsterName == "boar")
	{
		if (!spriteSheet.loadFromFile("../assets/Images/boar_sheet.png")) {
			err() << "loadSprite: Could not load texture for sprite: " <<
				"../assets/Images/boar_sheet.png" << endl;
		}
		
		if (!colliderReferenceImage.loadFromFile("../assets/Images/boar_reference.png")) {
			err() << "loadSprite: Could not load texture for sprite: " <<
				"../assets/Images/boar_reference.png" << endl;
		}
	}
	else if (monsterName == "bee")
	{
		if (!spriteSheet.loadFromFile("../assets/Images/bee_sheet.png")) {
			err() << "loadSprite: Could not load texture for sprite: " <<
				"../assets/Images/bee_sheet.png" << endl;
		}
		
		if (!colliderReferenceImage.loadFromFile("../assets/Images/bee_reference.png")) {
			err() << "loadSprite: Could not load texture for sprite: " <<
				"../assets/Images/bee_reference.png" << endl;
		}
	}
	else if (monsterName == "slime")
	{
		
	}
	else
	{
		throw std::invalid_argument("No existing sprite sheet for given object found.");
	}
}

// load animation component and specify the sprite sheet's row and column count via a Vector2u
void EnemyLoader::loadAnimationComponent(const shared_ptr<GameObject>& enemyObject, shared_ptr<Texture> texture,
	const string& enemyName)
{
	if (enemyName == "bat")
	{
		enemyObject->AddRenderComponent({ make_shared<AnimationComponent>(*enemyObject, texture, Vector2u(6, 5), 0.2) });
	}
	else if (enemyName == "boar")
	{
		enemyObject->AddRenderComponent({ make_shared<AnimationComponent>(*enemyObject, texture, Vector2u(5, 12), 0.2) });
	}
	else if (enemyName == "bee")
	{
		enemyObject->AddRenderComponent({ make_shared<AnimationComponent>(*enemyObject, texture, Vector2u(6, 8), 0.2) });
	}
	else
	{
		throw std::invalid_argument("Invalid object name given. Could not create animation component.");
	}

	loadAnimations(dynamic_pointer_cast<AnimationComponent>(enemyObject->getRenderComponentByIdx(0)), enemyName);
}

// define animations based on enemy type
void EnemyLoader::loadAnimations(const shared_ptr<AnimationComponent>& animationComponent, const string& enemyName)
{
	if (enemyName == "bat")
	{
		animationComponent->addAnimation("die_back", 0, 6);
		animationComponent->addAnimation("die_front", 0, 6);
		animationComponent->addAnimation("die_left", 0, 6);
		animationComponent->addAnimation("die_right", 0, 6);

		animationComponent->addAnimation("idle_back", 1, 4);
		animationComponent->addAnimation("idle_front", 2, 4);
		animationComponent->addAnimation("idle_left", 3, 4);
		animationComponent->addAnimation("idle_right", 4, 4);

		animationComponent->addAnimation("move_back", 1, 4);
		animationComponent->addAnimation("move_front", 2, 4);
		animationComponent->addAnimation("move_left", 3, 4);
		animationComponent->addAnimation("move_right", 4, 4);
	}
	else if (enemyName == "boar")
	{
		animationComponent->addAnimation("die_back", 1, 5);
		animationComponent->addAnimation("die_front", 1, 5);
		animationComponent->addAnimation("die_left", 1, 5);
		animationComponent->addAnimation("die_right", 1, 5);

		animationComponent->addAnimation("idle_back", 4, 3);
		animationComponent->addAnimation("idle_front", 5, 3);
		animationComponent->addAnimation("idle_left", 6, 3);
		animationComponent->addAnimation("idle_right", 7, 3);

		animationComponent->addAnimation("move_back", 8, 4);
		animationComponent->addAnimation("move_front", 9, 4);
		animationComponent->addAnimation("move_left", 10, 4);
		animationComponent->addAnimation("move_right", 11, 4);
	}
	else if (enemyName == "bee")
	{
		animationComponent->addAnimation("die_back", 1, 5);
		animationComponent->addAnimation("die_front", 1, 5);
		animationComponent->addAnimation("die_left", 1, 5);
		animationComponent->addAnimation("die_right", 1, 5);

		animationComponent->addAnimation("idle_back", 4, 6);
		animationComponent->addAnimation("idle_front", 5, 6);
		animationComponent->addAnimation("idle_left", 6, 6);
		animationComponent->addAnimation("idle_right", 7, 6);

		animationComponent->addAnimation("move_back", 4, 6);
		animationComponent->addAnimation("move_front", 5, 6);
		animationComponent->addAnimation("move_left", 6, 6);
		animationComponent->addAnimation("move_right", 7, 6);
	}
	else
	{
		throw std::invalid_argument("Invalid object name given. Could not load animations.");
	}
}

// load state machine based on enemy type
shared_ptr<EnemyController> EnemyLoader::loadController(const shared_ptr<GameObject>& enemyObject, 
	shared_ptr<PlayerStatsComponent> statsComponent, const string& enemyName)
{
	shared_ptr<EnemyController> enemyController = nullptr;

	if (enemyName == "bat")
	{
		enemyController = make_shared<EnemyController>(*enemyObject, make_shared<IdleStateEnemy>("idle", 0.2f, Vector2f(500, 0)), statsComponent);
		enemyController->AddState(make_shared<ChaseStateEnemy>("move", 0.2f, Vector2f(500, 0)), "chase");
		enemyController->AddState(make_shared<DeathStateEnemy>("die", 0.2f, 1), "dead");
		enemyController->AddState(make_shared<StunnedStateEnemy>("idle", 0.2f, 1), "stunned");
	}
	else if (enemyName == "boar")
	{
		enemyController = make_shared<EnemyController>(*enemyObject, make_shared<PatrolingStateEnemy>("move", 0.2f, 0.5f), statsComponent);
		enemyController->AddState(make_shared<StunnedStateEnemy>("idle", 0.2f, 2), "stunned");
		enemyController->AddState(make_shared<ChargingStateEnemy>("move", 0.1f, 8.0f), "chase");
		enemyController->AddState(make_shared<DeathStateEnemy>("die", 0.2f, 1), "dead");
	}
	else if (enemyName == "bee")
	{
		// currently not in use
		/*enemyController = make_shared<EnemyController>(*enemyObject, make_shared<IdleStateEnemy>(Vector2i(500, 0)), statsComponent);
		enemyController->AddState(make_shared<CircleAroundStateEnemy>(400, 30), "chase");
		enemyController->AddState(make_shared<DeathStateEnemy>(1), "dead");*/
	}
	else if (enemyName == "slime")
	{
		
	}
	else
	{
		throw std::invalid_argument("No existing controller states for given object found.");
	}

	if (!enemyController)
	{
		throw std::invalid_argument("Controller was empty.");
	}

	return enemyController;
}

void EnemyLoader::createEnemyType(const string& enemyName, shared_ptr<enemyBreed>& enemyType)
{
	if (enemyName == "bat") {
		enemyType = make_shared<bat>();
	}
	else if (enemyName == "boar") {
		enemyType = make_shared<boar>();
	}
	else if (enemyName == "bee") {
		enemyType = make_shared<bee>();
	}
	else if (enemyName == "slime") {
		enemyType = make_shared<slime>();
	}
	else
	{
		throw std::invalid_argument("No existing predefined templates for given object found.");
	}
	enemyType->init();
}

void EnemyLoader::configureEnemyStats(const shared_ptr<GameObject>& enemyObject, const shared_ptr<enemyBreed>& enemyType, 
	shared_ptr<HealthBarComponent>& healthbarcomp, shared_ptr<PlayerStatsComponent>& enemyStats)
{
	healthbarcomp = make_shared<HealthBarComponent>(*enemyObject);
	enemyStats = make_shared<PlayerStatsComponent>(*enemyObject, enemyType->HP);
	enemyStats->registerObserver(make_shared<HealthObserver>(healthbarcomp));
	enemyStats->setMovementSpeed(enemyType->movementspeed);

	enemyObject->AddComponent(enemyStats);
	enemyObject->AddRenderComponent(healthbarcomp);
}

void EnemyLoader::initHealthbarAndStats(const shared_ptr<GameObject>& enemyObject, const string& enemyName)
{
	shared_ptr<enemyBreed> enemyType;
	createEnemyType(enemyName, enemyType);

	shared_ptr<HealthBarComponent> healthbarcomp;
	shared_ptr<PlayerStatsComponent> enemyStats;
	configureEnemyStats(enemyObject, enemyType, healthbarcomp, enemyStats);
}
