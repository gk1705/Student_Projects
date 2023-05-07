#include "stdafx.h"
#include "PhysicsManager.h"

using namespace std;
using namespace sf;

PhysicsManager* PhysicsManager::m_instance = nullptr;

PhysicsManager& PhysicsManager::getInstance() {
	if (m_instance == nullptr)
		m_instance = new PhysicsManager();
	return *m_instance;
}

void PhysicsManager::release() {
	if (m_instance != nullptr)
		delete m_instance;
	m_instance = nullptr;
}

void PhysicsManager::findCollisions() {
	for (auto itA = m_bodies.begin(); itA != m_bodies.end(); ++itA)
	{
		auto& body1 = *itA;
		for (auto itB = itA; itB != m_bodies.end(); ++itB)
		{
			if (itA == itB)
				continue;

			auto& body2 = *itB;
			// if both object don't have a mass or body is the same skip
			if (body1->getRigidbody()->mass == 0 && body2->getRigidbody()->mass == 0)
			{
				continue;
			}

			Vector2f normal;
			float penetration;
			for (const auto& boxOfObj1 : body1->getCollisionComponents())
			{
				if (boxOfObj1->GetDisabled()) //despawning objects!!
					continue;

				for (const auto& boxOfObj2 : body2->getCollisionComponents())
				{
					if (boxOfObj2->GetDisabled()) // despawning objects!!
						continue;

					if (AABBvsAABB(boxOfObj1->GetShape(),
						boxOfObj2->GetShape(),
						normal,
						penetration))
					{
						if (!boxOfObj1->GetTrigger() &&
							!boxOfObj2->GetTrigger()) {
							manifold manifold;
							manifold.body1 = body1->getRigidbody();
							manifold.body2 = body2->getRigidbody();
							manifold.normal = normal;
							manifold.penetration = penetration;

							m_manifolds.push_back(manifold);

							//observing collisions --> CollisionResolverObserver
							body1->getRigidbody()->OnCollisionEnter(*body2);
							body2->getRigidbody()->OnCollisionEnter(*body1);
						}
						else
						{
							//Find out if it is OnCollisionEnter
							body1->getRigidbody()->CollisionsThisFrame.push_back(body2->getID());
							body2->getRigidbody()->CollisionsThisFrame.push_back(body1->getID());

							bool objectsCollidedLastFrame = false;
							for (const auto& objectID : body1->getRigidbody()->CollisionsLastFrame) {
								if (body2->getID() == objectID) objectsCollidedLastFrame = true;
							}
							body2->getRigidbody()->OnCollision(*body1);
							body1->getRigidbody()->OnCollision(*body2);

							if (!objectsCollidedLastFrame) {
								body2->getRigidbody()->OnCollisionEnter(*body1);
								body1->getRigidbody()->OnCollisionEnter(*body2);
							}
						}
					}
				}
			}
		}
	}
}

void PhysicsManager::resolveCollisions()
{
	for (auto man : m_manifolds)
	{
		// Calculate relative velocity
		const Vector2f rv = man.body1->velocity - man.body2->velocity;

		// Calculate relative velocity in terms of the normal direction (length through vector projection)
		float velAlongNormal = rv.x * man.normal.x + rv.y * man.normal.y;

		// Do not resolve if velocities are separating
		if (velAlongNormal > 0)
			return;

		const bool restitutionOn = true;
		if (restitutionOn)
		{
			// Calculate impulse scalar
			//const float e = 1.0f; //< 1.0 = all objects are bouncy
			const float e = 0;
			float j = -(1 + e) * velAlongNormal;
			j /= man.body1->invMass + man.body2->invMass;

			// Apply impulse
			const Vector2f impulse = j * man.normal;

			man.body1->velocity += impulse * man.body1->invMass;
			man.body2->velocity -= impulse * man.body2->invMass;

		}
		else
		{
			// Apply impulse
			Vector2f impulse = velAlongNormal * man.normal;

			man.body1->velocity -= 0.5f * impulse;
			man.body2->velocity += 0.5f * impulse;
		}

		// Positional correction
		const bool positionalCorrection = true;
		if (positionalCorrection)
		{
			const float percent = 0.2f; // usually 20% to 80%
			const float slop = 0.01f; // usually 0.01 to 0.1
			const Vector2f correction = std::max(man.penetration - slop, 0.0f) /
				(man.body1->invMass + man.body2->invMass) * percent * man.normal;
			// Apply directly to position
			man.body1->getPosition() += man.body1->invMass * correction;
			man.body2->getPosition() -= man.body2->invMass * correction;
		}
	}
}

void PhysicsManager::handleCollisions()
{
	m_manifolds.clear();

	findCollisions();
	resolveCollisions();
}

void PhysicsManager::addObject(const shared_ptr<GameObject>& gameObject)
{
	m_bodies.push_back(gameObject);
}

void PhysicsManager::removeObject(const string& gameObject)
{
	int removeObject = -1;
	int count = 0;

	for (const auto& obj : m_bodies) {
		if (obj->getID() == gameObject) {
			removeObject = count;
		}
		count++;
	}

	if (removeObject != -1)
		m_bodies.erase(m_bodies.begin() + removeObject);
}

void PhysicsManager::reset()
{
	m_bodies.clear();
}

bool PhysicsManager::AABBvsAABB(const FloatRect& a, const FloatRect& b,
	Vector2f& normal, float& penetration) const
{
	const auto getCenter = [](const sf::FloatRect& rect) -> Vector2f
	{
		return Vector2f(rect.left, rect.top) + 0.5f * Vector2f(rect.width, rect.height);
	};

	const Vector2f n = getCenter(b) - getCenter(a);		// Vector from A to B
	const float a_extent = a.width * 0.5f;			    // Calculate half extents along x axis
	const float b_extent = b.width * 0.5f;
	const float x_overlap = a_extent + b_extent - abs(n.x);		// Calculate overlap on x axis
															// SAT test on x axis
	if (x_overlap > 0) {
		float a_extent = a.height * 0.5f;		// Calculate half extents along y axis
		float b_extent = b.height * 0.5f;
		const float y_overlap = a_extent + b_extent - abs(n.y);	// Calculate overlap on y axis

															// SAT test on y axis
		if (y_overlap > 0) {
			// Find out which axis is axis of least penetration
			if (x_overlap < y_overlap) {
				// Point towards B knowing that n points from A to B
				if (n.x < 0)
					normal = Vector2f(1.0f, 0.0f);
				else
					normal = Vector2f(-1.0f, 0.0f);
				penetration = x_overlap;
				return true;
			}
			else {
				// Point towards B knowing that n points from A to B
				if (n.y < 0)
					normal = Vector2f(0, 1);
				else
					normal = Vector2f(0, -1);
				penetration = y_overlap;
				return true;
			}
		}
	}
	return false;
}
