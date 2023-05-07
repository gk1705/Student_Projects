#include "stdafx.h"
#include "HeroLoader.h"
#include "HumanController.h"
#include "CollisionResolverObserver.h"
#include "HealthBarComponent.h"
#include "HealthObserver.h"
#include "PlayerStatsComponent.h"
#include "MovementComponent.h"
#include "BoxColliderComponent.h"
#include "RenderManager.h"
#include "AnimationManager.h"
#include "GameObjectManager.h"
#include "UpdateManager.h"
#include "PlayFieldManager.h"

void HeroLoader::LoadHeros(const string& hero1, const string& hero2)
{
	countid = 1;
	const shared_ptr<GameObject> player1 = CreateHero("Player1", hero1, Vector2f(200, 200));
	const shared_ptr<GameObject> player2 = CreateHero("Player2", hero2, Vector2f(1400, 200));

	initBorder({ player1, player2 });

	GameObjectManager::getInstance().addGameObject(player1);
	GameObjectManager::getInstance().addGameObject(player2);
}

void HeroLoader::initBorder(const initializer_list<const shared_ptr<GameObject>> listOfPlayers)
{
	if (listOfPlayers.size() > 2)
	{
		throw invalid_argument("Currently only two players are supported.");
	}

	int counter = 0;
	for(const auto& player : listOfPlayers)
	{
		const shared_ptr<BorderComponent> bordercomp = make_shared<BorderComponent>(*player, PlayFieldManager::getInstance().getPlayfieldsRect()[counter++]);
		player->AddComponent(bordercomp);
		UpdateManager::getInstance().AddComponent(bordercomp, player->getID());
	}
}

shared_ptr<GameObject> HeroLoader::CreateHero(const string& player, const string& heroName, const Vector2f& position) {

	shared_ptr<GameObject> gameObject = make_shared<GameObject>(player, position);
	gameObject->setType("PlayerObject");

	shared_ptr<playerType> heroType;
	Image colliderReferenceImage;
	shared_ptr<Texture> texture;
	sf::FloatRect collider;
	shared_ptr<PlayerStatsComponent> playerstats;

	createPlayerType(heroName, heroType);
	initSpriteSheet(heroName, colliderReferenceImage, texture);
	initAnimations(heroName, gameObject, colliderReferenceImage, texture, collider);
	initPhysics(heroName, gameObject, collider);
	initHealthbarAndStats(gameObject, heroType, playerstats);
	initMovement(gameObject, playerstats, heroName);
	addToManagers(gameObject);

	return gameObject;
}

void HeroLoader::initSpriteSheet(const string& heroName, Image& colliderReferenceImage, shared_ptr<Texture>& texture)
{
	Image image;
	loadSpriteSheet(image, colliderReferenceImage, heroName);

	texture = make_shared<Texture>();
	texture->loadFromImage(image);
}

void HeroLoader::initAnimations(const string& heroName, const shared_ptr<GameObject>& gameObject,
                                const Image& colliderReferenceImage, shared_ptr<Texture> texture, sf::FloatRect& collider)
{
	loadAnimationComponent(gameObject, texture, heroName);

	collider = sf::FloatRect(gameObject->GetPosition(), 
		sf::Vector2f(colliderReferenceImage.getSize().x, colliderReferenceImage.getSize().y));
	correctPlayerCollider(collider, heroName);

	// add animation component to manager
	AnimationManager::getInstance().AddComponent(dynamic_pointer_cast<AnimationComponent>(gameObject->getRenderComponentByIdx(0)), gameObject->getID());
	// play idle animation
	dynamic_pointer_cast<AnimationComponent>(gameObject->getRenderComponentByIdx(0))->playAnimation("idle_front", 0.2);
}

void HeroLoader::initPhysics(const string& heroName, const shared_ptr<GameObject>& gameObject, sf::FloatRect collider)
{
	//Rigidbody + Boxcollider
	gameObject->AddRigidbody(make_shared<RigidbodyComponent>(*gameObject, 10));
	gameObject->getRigidbody()->registerObserver(make_shared<CollisionResolverObserver>(gameObject->getID()));
	gameObject->AddCollisionComponent(make_shared<BoxColliderComponent>(*gameObject, collider, getPlayerOffset(heroName)));
}

void HeroLoader::initHealthbarAndStats(const shared_ptr<GameObject>& gameObject, const shared_ptr<playerType>& heroType, shared_ptr<PlayerStatsComponent>& playerstats)
{
	//Add Healthbar - after boxcollider (boxcollider as a reference to healthbar size)
	shared_ptr<HealthBarComponent> healthbarcomp = make_shared<HealthBarComponent>(*gameObject);
	playerstats = make_shared<PlayerStatsComponent>(*gameObject, heroType->HP);
	playerstats->registerObserver(make_shared<HealthObserver>(healthbarcomp));
	playerstats->setMovementSpeed(heroType->movementspeed);
	playerstats->setAttackSpeed(heroType->attackSpeed);
	playerstats->setProjectileSpeed(heroType->projectileSpeed);
	gameObject->AddComponent(playerstats);
	gameObject->AddRenderComponent(healthbarcomp);
}

void HeroLoader::createPlayerType(const string& heroName, shared_ptr<playerType>& heroType)
{
	if (heroName == "Witch") 
		heroType = make_shared<witch>(); 
	else 
		heroType = make_shared<archer>();
	heroType->init();
}

void HeroLoader::initMovement(const shared_ptr<GameObject>& gameObject, shared_ptr<PlayerStatsComponent> playerstats, const string& heroName)
{
	//AddMovement (statemachine, states are directly embedded into the human/movement controller)
	shared_ptr<MovementComponent> movement = make_shared<MovementComponent>(*gameObject, countid++);
	movement->setStrategy(make_shared<HumanController>(*gameObject, heroName, playerstats));
	gameObject->AddMovementComponent(movement);
}

void HeroLoader::addToManagers(const shared_ptr<GameObject>& gameObject)
{
	RenderManager::getInstance().AddComponent(gameObject->getRenderComponentByIdx(0), gameObject->getID(), 2);//Animations
	UpdateManager::getInstance().AddComponent(gameObject->getRenderComponentByIdx(0), gameObject->getID()); //Animations
	RenderManager::getInstance().AddComponent(gameObject->getRenderComponentByIdx(1), gameObject->getID(), 2); //Health bar
}

// load sprite sheet by what hero picked
// also: load reference image for boxcollider-size
void HeroLoader::loadSpriteSheet(Image& spriteSheet, Image& colliderReferenceImage, const string& heroName)
{
	if (heroName == "Archer")
	{
		if (!spriteSheet.loadFromFile("../assets/Images/archer_sheet.png")) {
			err() << "loadSprite: Could not load texture for sprite: " <<
				"../assets/Images/archer_sheet.png" << endl;
		}

		if (!colliderReferenceImage.loadFromFile("../assets/Images/archer_reference.png")) {
			err() << "loadSprite: Could not load texture for sprite: " <<
				"../assets/Images/archer_reference.png" << endl;
		}
	}
	else if (heroName == "Witch")
	{
		if (!spriteSheet.loadFromFile("../assets/Images/witch_sheet.png")) {
			err() << "loadSprite: Could not load texture for sprite: " <<
				"../assets/Images/witch_sheet.png" << endl;
		}

		if (!colliderReferenceImage.loadFromFile("../assets/Images/witch_reference.png")) {
			err() << "loadSprite: Could not load texture for sprite: " <<
				"../assets/Images/witch_reference.png" << endl;
		}
	}
	else
	{
		throw std::invalid_argument("No existing sprite sheet for given object found.");
	}
}

// based on character -> differing sprite sheet size
// Vector2u is used to specify image row and column count
void HeroLoader::loadAnimationComponent(const shared_ptr<GameObject>& gameObject, shared_ptr<Texture> texture, const string& heroName)
{
	if (heroName == "Archer")
	{
		gameObject->AddRenderComponent({ make_shared<AnimationComponent>(*gameObject, texture, Vector2u(4, 20), 0.2) });
	}
	else if (heroName == "Witch")
	{
		gameObject->AddRenderComponent({ make_shared<AnimationComponent>(*gameObject, texture, Vector2u(5, 17), 0.2) });
	}
	else
	{
		throw std::invalid_argument("Invalid object name given. Could not create animation component.");
	}

	loadAnimations(dynamic_pointer_cast<AnimationComponent>(gameObject->getRenderComponentByIdx(0)), heroName);
}

// define animations based on Archer' and Witch' sprite sheet
void HeroLoader::loadAnimations(const shared_ptr<AnimationComponent>& animationComponent, const string& objectName)
{
	if (objectName == "Archer")
	{
		animationComponent->addAnimation("die_back", 0, 3);
		animationComponent->addAnimation("die_front", 1, 3);
		animationComponent->addAnimation("die_left", 2, 3);
		animationComponent->addAnimation("die_right", 3, 3);

		animationComponent->addAnimation("hit_back", 4, 1);
		animationComponent->addAnimation("hit_front", 5, 1);
		animationComponent->addAnimation("hit_left", 6, 1);
		animationComponent->addAnimation("hit_right", 7, 1);

		animationComponent->addAnimation("idle_back", 8, 4);
		animationComponent->addAnimation("idle_front", 9, 4);
		animationComponent->addAnimation("idle_left", 10, 4);
		animationComponent->addAnimation("idle_right", 11, 4);

		animationComponent->addAnimation("attack_back", 12, 4);
		animationComponent->addAnimation("attack_front", 13, 4);
		animationComponent->addAnimation("attack_left", 14, 4);
		animationComponent->addAnimation("attack_right", 15, 4);

		animationComponent->addAnimation("walk_back", 16, 4);
		animationComponent->addAnimation("walk_front", 17, 4);
		animationComponent->addAnimation("walk_left", 18, 4);
		animationComponent->addAnimation("walk_right", 19, 4);
	}
	else if (objectName == "Witch")
	{
		animationComponent->addAnimation("die_left", 0, 5);
		animationComponent->addAnimation("die_right", 0, 5);
		animationComponent->addAnimation("die_back", 0, 5);
		animationComponent->addAnimation("die_front", 0, 5);

		animationComponent->addAnimation("hit_back", 1, 4);
		animationComponent->addAnimation("hit_front", 2, 4);
		animationComponent->addAnimation("hit_left", 3, 4);
		animationComponent->addAnimation("hit_right", 4, 4);

		animationComponent->addAnimation("idle_back", 5, 3);
		animationComponent->addAnimation("idle_front", 6, 3);
		animationComponent->addAnimation("idle_left", 7, 3);
		animationComponent->addAnimation("idle_right", 8, 3);

		animationComponent->addAnimation("attack_back", 9, 5);
		animationComponent->addAnimation("attack_front", 10, 5);
		animationComponent->addAnimation("attack_left", 11, 5);
		animationComponent->addAnimation("attack_right", 12, 5);

		animationComponent->addAnimation("walk_back", 13, 4);
		animationComponent->addAnimation("walk_front", 14, 4);
		animationComponent->addAnimation("walk_left", 15, 4);
		animationComponent->addAnimation("walk_right", 16, 4);
	}
	else
	{
		throw std::invalid_argument("Invalid object name given. Could not load animations.");
	}
}

// magic value -> rect needs to be corrected to fit the sprite of the player as good as possible -> fairness
// this method is used to adjust the rect's size
void HeroLoader::correctPlayerCollider(FloatRect& collider, const string& heroName)
{
	if (heroName == "Archer")
	{
		/* *** Magic values for Archer *** */
		collider.height -= 60 * 0.5f;
		collider.width -= 350 * 0.5f;
		/* ****************************** */
	}
	else if (heroName == "Witch")
	{
		/* *** Magic values for Archer *** */
		collider.height -= 60 * 0.3;
		collider.width /= 2.6f;
		/* ****************************** */
	}
	else
	{
		throw std::invalid_argument("Function not available for player name.");
	}
}

// same as the method beforehand
// this method is used to move the rect's position to center around the player
Vector2f HeroLoader::getPlayerOffset(const string& heroName)
{
	if (heroName == "Archer")
	{
		return Vector2f(205 * 0.5f, 50 * 0.5f);
	}
	else if (heroName == "Witch")
	{
		return Vector2f(50, 10);
	}
	else
	{
		throw std::invalid_argument("Function not available for player name.");
	}
}