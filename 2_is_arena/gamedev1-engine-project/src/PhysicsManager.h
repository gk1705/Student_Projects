#pragma once
#include <vector>
#include "RigidbodyComponent.h"
#include "BoxColliderComponent.h"
#include "GameObject.h"

using namespace sf;
using namespace std;

class PhysicsManager
{
public:
	static PhysicsManager& getInstance();
	static void release();

	void handleCollisions();

	void addObject(const shared_ptr<GameObject>& gameObject);
	void removeObject(const string& gameObject);
	
	void reset();

	struct manifold
	{
		shared_ptr<RigidbodyComponent> body1;
		shared_ptr<RigidbodyComponent> body2;

		float penetration;
		Vector2f normal;
	};

	PhysicsManager(const PhysicsManager& p) = delete;
	PhysicsManager& operator=(PhysicsManager const&) = delete;

private:
	PhysicsManager(void) = default;
	~PhysicsManager(void) = default;

	static PhysicsManager *m_instance;

	vector<shared_ptr<GameObject>> m_bodies;
	vector<manifold> m_manifolds;

	void findCollisions();
	void resolveCollisions();
	bool AABBvsAABB(const FloatRect& a, const FloatRect& b, Vector2f& normal, float& penetration) const;
};