#pragma once
#include <SFML/Graphics.hpp>
#include "stdafx.h"
#include "BorderComponent.h"
#include "IComponent.h"

#include "PlayerInputComponent.h""
#include "RigidbodyComponent.h"

class IGraphicsComponent;
class BoxColliderComponent;
class MovementComponent;

using namespace std;
using namespace sf;

class GameObject
{
public:
	//Constructor
	GameObject(std::string id, sf::Vector2f position);

	//Methods
	void Update(float deltatime, sf::RenderWindow& window);
	void Render(sf::RenderWindow &window);
	void resetPosition();

	//Getter
	float getWidth();
	float getHeight();
	FloatRect& getColliderShape();
	string getID() const;
	string getType() const;
	Vector2f& GetPosition();
	shared_ptr<RigidbodyComponent> getRigidbody();
	shared_ptr<MovementComponent> getMovementComponent();
	shared_ptr<IGraphicsComponent> getRenderComponentByIdx(const int idx) const;
	vector<shared_ptr<IComponent>> getComponents();
	vector<shared_ptr<BoxColliderComponent>> getCollisionComponents();

	template<typename T>
	shared_ptr<T> getComponent()
	{
		for (const auto comp : m_components)
		{
			auto retVal = std::dynamic_pointer_cast<T>(comp);
			if (retVal)
				return retVal;
		}
		// error handling!!!
		return nullptr;
	}

	//setter
	void setType(string type);
	void setPosition(Vector2f position);
	void setHeight(float height);

	//AddComponents
	void AddComponent(shared_ptr<IComponent> component);
	void AddRigidbody(shared_ptr<RigidbodyComponent> rigidbody);
	void AddMovementComponent(shared_ptr<MovementComponent> movementComponent);
	void AddRenderComponent(initializer_list<shared_ptr<IGraphicsComponent>> renderComponents);
	void AddRenderComponent(shared_ptr<IGraphicsComponent> renderComponent);
	void AddCollisionComponent(shared_ptr<BoxColliderComponent> collisionComponent);

private:
	string m_id;
	string m_type = "none"; //GameObject does not necessarily need a type
	Vector2f m_startPosition;
	Vector2f m_position;

	vector<shared_ptr<IComponent>> m_components;
	vector<shared_ptr<IGraphicsComponent>> renderComponents;
	vector<shared_ptr<BoxColliderComponent>> collisonComponents;
	shared_ptr<RigidbodyComponent> m_rigidbody; //gameObject can only have 1 Rigidbody
	shared_ptr<MovementComponent> m_movementComponent;
};