#pragma once
#include <memory>
#include <unordered_map>
#include <vector>
#include "IComponent.h"
#include "IGraphicsComponent.h"
#include "MovementComponent.h"

using namespace std;

class UpdateManager
{
public:
	static UpdateManager& getInstance();
	static void release();

	void AddComponent(const shared_ptr<IComponent>& components, const std::string& ownerID);
	void AddComponent(const shared_ptr<IGraphicsComponent>& components, const std::string& ownerID);
	void AddComponent(const shared_ptr<MovementComponent>& component, const std::string& ownerID);
	/*void AddComponent(const shared_ptr<RigidbodyComponent>& component, const std::string& ownerID);*/

	void DeleteComponentsOfGameObject(const string& ownerID);
	void Update(float fDeltaTime);

	void reset();

	UpdateManager(const UpdateManager& p) = delete;
	UpdateManager& operator=(UpdateManager const&) = delete;

private:
	UpdateManager(void) = default;
	~UpdateManager(void) = default;

	static UpdateManager *m_instance;

	unordered_map <string, vector<shared_ptr<IComponent>>> m_components;
	unordered_map <string, vector<shared_ptr<IGraphicsComponent>>> m_graphicscomponents;
	unordered_map <string, shared_ptr<MovementComponent>> m_movementComponents;

};