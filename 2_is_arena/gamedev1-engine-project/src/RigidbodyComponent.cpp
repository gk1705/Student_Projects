#include "stdafx.h"
#include "RigidbodyComponent.h"
#include "GameObject.h"

void RigidbodyComponent::OnCollisionEnter(GameObject& gameObject) {
	for (const auto& obs : m_observer)
	{
		obs->notify(gameObject);
	}
}

void RigidbodyComponent::OnCollision(GameObject& gameObject) {

	for (const auto& obs : m_onCollisionObserver)
	{
		obs->notify(gameObject);
	}
}

void RigidbodyComponent::Update(float fDeltaTime)
{
	auto length2 = [](const Vector2f &vec) -> float { return vec.x * vec.x + vec.y * vec.y; };

	Vector2f forces{};
	for (const auto& f : m_forces)
		forces += f;

	for (const auto& i : m_impulses)
		forces += i;
	m_impulses.clear();

	// symplectic Euler
	acceleration = forces * invMass;
	velocity += acceleration * fDeltaTime;

	m_gameObject.GetPosition() += velocity * fDeltaTime;

	CollisionsLastFrame.clear();
	CollisionsLastFrame = CollisionsThisFrame;
	CollisionsThisFrame.clear();
}

void RigidbodyComponent::registerObserver(const shared_ptr<IObserverComponent>& observer)
{
	m_observer.push_back(observer);
}

void RigidbodyComponent::registerOnCollisionObserver(const shared_ptr<IObserverComponent>& observer)
{
	m_onCollisionObserver.push_back(observer);
}

sf::Vector2f& RigidbodyComponent::getPosition() const
{
	return m_gameObject.GetPosition();
}
