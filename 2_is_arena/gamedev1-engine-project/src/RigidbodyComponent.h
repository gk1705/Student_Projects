#pragma once
#include "stdafx.h"
#include "IObserverComponent.h"
#include "IComponent.h"

class GameObject;

using namespace std;
using namespace sf;

class RigidbodyComponent : public IComponent
{
public:
	// Parameters used for the rigid body physics
	RigidbodyComponent(GameObject& gameObject, float mass_)
		: IComponent(gameObject)
		, imovAble(false)
	{
		mass = mass_;
		invMass = (mass == 0.0f) ? 0.0f : 1.0f / mass;
	}

	vector<sf::String> CollisionsLastFrame;
	vector<sf::String> CollisionsThisFrame;

	float mass;
	float invMass;

	list<Vector2f> m_forces;
	list<Vector2f> m_impulses;

	Vector2f acceleration;
	Vector2f velocity;

	void OnCollisionEnter(GameObject& gameObject);
	void OnCollision(GameObject& gameObject);
	void Update(float fDeltatime) override;
	void registerObserver(const shared_ptr<IObserverComponent>& observer);
	void registerOnCollisionObserver(const shared_ptr<IObserverComponent>& observer);
	bool imovAble;
	Vector2f& getPosition() const;

private:
	vector<shared_ptr<IObserverComponent>> m_observer;
	vector<shared_ptr<IObserverComponent>> m_onCollisionObserver;
};
