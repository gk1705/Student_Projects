#pragma once
#include "IComponent.h"
#include "IObserverComponent.h"

class GameObject;

class PlayerStatsComponent : public IComponent {
public:
	PlayerStatsComponent(GameObject& gameObject, int maxHP);
	void Update(float fDeltaTime) override;

	void registerObserver(shared_ptr<IObserverComponent> observer);

	//Getter
	int getHP() const;
	int getMaxHP() const;
	int getMaxUltimate() const;
	float getMaxAttackSpeed() const;
	float getMovementSpeed() const;
	int getUltimate() const;
	bool UltimateReady() const;
	bool getIsDead() const;
	bool IsDamaged() const;
	bool IsSlowed() const;
	float getProjectileSpeed() const;

	//Setter
	void getDamage(const int damage);
	void getUltimatePoints(const int value);
	void getSlowed(const float percentage);
	void setMovementSpeed(const float movementspeed);
	void setAttackSpeed(const float attackspeed);
	void resetUltimate();//set ultimate to 0
	void setIsDead(const bool isDead);
	void receiveHealing();
	void setProjectileSpeed(const float speed);

	void notifyObservers();

	//reset if respawn
	void resetStats();

private:
	const int m_MAX_HP;
	float m_MAX_attackspeed;
	float m_MAX_movementspeed;
	const int m_MAX_ultimate = 100;

	int m_HP;
	float m_movementspeed;
	float m_projectileSpeed = 10.0f;
	int m_ultimate = 0;
	bool m_receiveHealing = false;
	bool m_isDead = false;

	float m_slowedTimer;
	float m_invincibleTimer;
	float m_receiveHealingTimer = 0;

	vector<shared_ptr<IObserverComponent>> m_observer;
};