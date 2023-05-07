#include "stdafx.h"
#include "ProjectileSpawner.h"
#include "SpriteComponent.h"
#include "ProjectileComponent.h"
#include "ProjectileCollisionObserver.h"
#include "MagicbulletCollisionObserver.h"
#include "RenderManager.h"
#include "GameObjectManager.h"
#include "SoundManager.h"
#include "DespawnComponent.h"
#include "HealAreaCollisionObserver.h"

namespace
{
	float getDegreeFromVector(const Vector2f& v) {
		const auto PI = 3.14159265358979323846;

		const auto u = Vector2f(1, 0);
		const auto lengthU = sqrt(u.x * u.x + u.y * u.y);
		const auto lengthV = sqrt(v.x * v.x + v.y * v.y);

		auto degree = (u.x *v.x + u.y * v.y) / (lengthU * lengthV);
		degree = acos(degree);
		degree *= 180 / PI;

		if (v.y < 0) degree *= -1;
		return degree;
	}

	float getRadiantFromVector(const Vector2f& v)
	{
		const auto PI = 3.14159265358979323846;

		const auto u = Vector2f(1, 0);
		const auto lengthU = sqrt(u.x * u.x + u.y * u.y);
		const auto lengthV = sqrt(v.x * v.x + v.y * v.y);

		auto radiant = (u.x *v.x + u.y * v.y) / (lengthU * lengthV);
		radiant = acos(radiant);

		if (v.y < 0) radiant *= -1;
		return radiant;
	}

	Vector2f calculateOffsetArrowCollider(const Vector2f& direction, const Vector2f& initialOffset)
	{
		Vector2f offset;
		const auto radiant = getRadiantFromVector(direction);
		offset.x = initialOffset.x * cos(radiant) - initialOffset.y * sin(radiant);
		offset.y = initialOffset.y * cos(radiant) + initialOffset.x * sin(radiant);

		return offset;
	}

	Vector2f normalize(Vector2f vec) {
		const auto length = sqrt(vec.x * vec.x + vec.y * vec.y);

		if (length != 0) {
			vec.x /= length;
			vec.y /= length;
		}
		else {
			vec.x = 1;
			vec.y = 0;
		}
		return vec;
	}
}

void ProjectileSpawner::spawnProjectile(GameObject& gameObject, const string& heroType, int& projectileCounter, const int&
	controllerID, const PlayerStatsComponent& playerStats, Vector2f velocity)
{
	//Load Image
	Image image;
	if (heroType == "Archer") {
		image.loadFromFile("../assets/Images/Arrow.png");
	}
	else {
		image.loadFromFile("../assets/Images/magicbullet.png");
	}
	shared_ptr<Texture> texture = make_shared<Texture>();
	texture->loadFromImage(image);

	//Calculate position and velocity
	Vector2f position = Vector2f(gameObject.GetPosition().x + gameObject.getWidth() / 2, gameObject.GetPosition().y + gameObject.getHeight() / 2);

	//Create Projectile
	shared_ptr<GameObject> projectile = make_shared<GameObject>("Z " + std::to_string(controllerID) + " - Arrow" + std::to_string(projectileCounter++), position);

	//AddSpriteComponent
	auto spritecomp = make_shared<SpriteComponent>(*projectile, texture);
	spritecomp->getSprite().setOrigin(Vector2f(image.getSize().x / 2, image.getSize().y / 2));
	spritecomp->Rotate(getDegreeFromVector(normalize(velocity)));
	projectile->AddRenderComponent(spritecomp);

	//Add Collider
	projectile->AddRigidbody(make_shared<RigidbodyComponent>(*projectile, 1));
	sf::FloatRect collider = sf::FloatRect(position, sf::Vector2f(image.getSize().x / 5, image.getSize().y / 5));
	// calculate offset based on rotation;
	const auto offsetProjectile = calculateOffsetArrowCollider(velocity, Vector2f(static_cast<float>(image.getSize().x) / 2, 0));

	projectile->AddCollisionComponent(make_shared<BoxColliderComponent>(*projectile, collider,
		Vector2f(offsetProjectile.x - collider.width / 2, offsetProjectile.y - collider.height / 2)));
	projectile->getCollisionComponents()[0]->SetTrigger(true);

	//ProjectileComp + Observer
	projectile->AddComponent(make_shared<ProjectileComponent>(*projectile, gameObject.getID(), normalize(velocity) * playerStats.getProjectileSpeed()));
	if (heroType == "Archer") {
		projectile->getRigidbody()->registerObserver(make_shared<ProjectileCollisionObserver>(projectile->getID(), gameObject.getID()));
	}
	else {
		projectile->getRigidbody()->registerObserver(make_shared<MagicBulletCollisionObserver>(projectile->getID(), gameObject.getID()));
	}

	RenderManager::getInstance().AddComponent(projectile->getRenderComponentByIdx(0), projectile->getID(), 2);
	projectile->setType("Arrow");
	GameObjectManager::getInstance().addGameObject(projectile);
	SoundManager::getInstance().PlaySound(heroType + "_shooting", "first");
}

void ProjectileSpawner::spawnUltimate(GameObject& gameObject, const string& heroType, int& projectileCount, const int&
	controllerID, const PlayerStatsComponent& playerStats)
{
	if (heroType == "Archer") {
		spawnProjectile(gameObject, heroType, projectileCount, controllerID, playerStats, Vector2f(1.0, 0));
		spawnProjectile(gameObject, heroType, projectileCount, controllerID, playerStats, Vector2f(-1.0, 0));
		spawnProjectile(gameObject, heroType, projectileCount, controllerID, playerStats, Vector2f(0, 1.0));
		spawnProjectile(gameObject, heroType, projectileCount, controllerID, playerStats, Vector2f(0, -1.0));
		spawnProjectile(gameObject, heroType, projectileCount, controllerID, playerStats, Vector2f(1.0, 1.0));
		spawnProjectile(gameObject, heroType, projectileCount, controllerID, playerStats, Vector2f(1.0, -1.0));
		spawnProjectile(gameObject, heroType, projectileCount, controllerID, playerStats, Vector2f(-1.0, 1.0));
		spawnProjectile(gameObject, heroType, projectileCount, controllerID, playerStats, Vector2f(-1.0, -1.0));
		spawnProjectile(gameObject, heroType, projectileCount, controllerID, playerStats, Vector2f(-1.0, 0.5));
		spawnProjectile(gameObject, heroType, projectileCount, controllerID, playerStats, Vector2f(-1.0, -0.5));
		spawnProjectile(gameObject, heroType, projectileCount, controllerID, playerStats, Vector2f(1.0, 0.5));
		spawnProjectile(gameObject, heroType, projectileCount, controllerID, playerStats, Vector2f(1.0, -0.5));
		spawnProjectile(gameObject, heroType, projectileCount, controllerID, playerStats, Vector2f(0.5, 1));
		spawnProjectile(gameObject, heroType, projectileCount, controllerID, playerStats, Vector2f(0.5, -1));
		spawnProjectile(gameObject, heroType, projectileCount, controllerID, playerStats, Vector2f(-0.5, 1));
		spawnProjectile(gameObject, heroType, projectileCount, controllerID, playerStats, Vector2f(-0.5, -1));

		// play sound
		SoundManager::getInstance().PlaySound(heroType + "_ultimate", "first");
	}
	else if (heroType == "Witch") {
		//Load Image
		Image image;
		image.loadFromFile("../assets/Images/healarea.png");
		shared_ptr<Texture> texture = make_shared<Texture>();
		texture->loadFromImage(image);

		//Calculate position
		Vector2f position = Vector2f(gameObject.GetPosition().x + gameObject.getWidth() / 2, gameObject.GetPosition().y + gameObject.getHeight() / 2);
		position.x -= image.getSize().x / 2;
		position.y -= image.getSize().y / 2;
		shared_ptr<GameObject> healarea = make_shared<GameObject>("healarea" + std::to_string(projectileCount), position);
		healarea->AddRenderComponent(make_shared<SpriteComponent>(*healarea, texture));

		//Add Collider
		healarea->AddRigidbody(make_shared<RigidbodyComponent>(*healarea, 1));
		sf::FloatRect collider = sf::FloatRect(position, sf::Vector2f(image.getSize().x * 0.8f, image.getSize().y * 0.8f));
		healarea->AddCollisionComponent(make_shared<BoxColliderComponent>(*healarea, collider, Vector2f(image.getSize().x*0.1, image.getSize().y * 0.1)));
		healarea->getCollisionComponents()[0]->SetTrigger(true);
		healarea->getRigidbody()->registerOnCollisionObserver(make_shared<HealAreaCollisionObserver>());

		//Add DespawnComponent
		healarea->AddComponent(make_shared<DespawnComponent>(*healarea, 5.0));

		RenderManager::getInstance().AddComponent(healarea->getRenderComponentByIdx(0), healarea->getID(), 1);
		healarea->setType("Healarea");
		GameObjectManager::getInstance().addGameObject(healarea);

		// play sound
		SoundManager::getInstance().PlayMusic("../assets/Sounds/" + heroType + "_ultimate.wav");
	}
}
