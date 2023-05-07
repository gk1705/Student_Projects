#include "stdafx.h"
#include "GameObject.h";
#include "IGraphicsComponent.h"
#include "RigidbodyComponent.h"
#include "BoxColliderComponent.h"

class RigidbodyComponent;

GameObject::GameObject(std::string id, sf::Vector2f position)
	: m_id(id)
	, m_position(position)
	, m_startPosition(position)
	, m_rigidbody(nullptr)
	, m_movementComponent(nullptr)
{
}

void GameObject::AddComponent(shared_ptr<IComponent> component)
{
	m_components.push_back(component);	
}

void GameObject::AddRigidbody(shared_ptr<RigidbodyComponent> rigidbody) {
	m_rigidbody = rigidbody;
	m_components.push_back(rigidbody);
}

void GameObject::AddMovementComponent(shared_ptr<MovementComponent> movementComponent)
{
	m_movementComponent = movementComponent;
}

void GameObject::AddRenderComponent(std::initializer_list<shared_ptr<IGraphicsComponent>> renderComponents_)
{
	for (auto& gcmp : renderComponents_)
	{
		renderComponents.push_back(gcmp);
	}
}

void GameObject::AddRenderComponent(shared_ptr<IGraphicsComponent> renderComponent_)
{
	renderComponents.push_back(renderComponent_);
}

void GameObject::AddCollisionComponent(shared_ptr<BoxColliderComponent> collisionComponent)
{
	collisonComponents.push_back(collisionComponent);
	m_components.push_back(collisionComponent);
}

shared_ptr<RigidbodyComponent> GameObject::getRigidbody()
{
	if (m_rigidbody)
		return m_rigidbody;
	// throw exception if no rigidbody is attached to object
	else
		throw std::invalid_argument("No rigidbody attached to object.");
}

shared_ptr<MovementComponent> GameObject::getMovementComponent()
{
	return m_movementComponent;
}

shared_ptr<IGraphicsComponent> GameObject::getRenderComponentByIdx(const int idx) const
{
	// throw exception if idx is oor
	if (idx >= renderComponents.size())
		throw std::invalid_argument("Can't return renderComponent. Index was out of range.");
	return renderComponents[idx];
}

vector<shared_ptr<IComponent>> GameObject::getComponents()
{
	return m_components;
}

std::vector<shared_ptr<BoxColliderComponent>> GameObject::getCollisionComponents()
{
	return collisonComponents;
}

void GameObject::setType(string type)
{
	m_type = type;
}

void GameObject::setPosition(Vector2f position)
{
	m_position = position;
}

void GameObject::setHeight(float height)
{
	renderComponents[0]->setHeight(height);
	collisonComponents[0]->GetShape().height = height;
}

void GameObject::Update(float deltatime, sf::RenderWindow& window) {
	for (auto comp : m_components)
		comp->Update(deltatime);
}

void GameObject::Render(sf::RenderWindow &window) {
	for (auto& renderer : renderComponents)
	{
		renderer->Draw(window);
	}
}

float GameObject::getWidth()
{
	// workaround, bad
	return renderComponents[0]->getWidth();
}

float GameObject::getHeight()
{
	// workaround, bad
	return renderComponents[0]->getHeight();
}

FloatRect& GameObject::getColliderShape()
{
	return collisonComponents[0]->GetShape();
}

std::string GameObject::getID() const
{
	return m_id;
}

string GameObject::getType() const
{
	return m_type;
}

sf::Vector2f & GameObject::GetPosition()
{
	return m_position;
}

void GameObject::resetPosition()
{
	m_position = m_startPosition;
	srand(time(nullptr));
	float x = rand() % 20000;
	float negative = rand() % 2;
	if (negative == 0) x *= -1;

	float y = rand() % 20000;
	negative = rand() % 2;
	if (negative == 0) y *= -1;

	getRigidbody()->velocity = sf::Vector2f(0,0);
	getRigidbody()->m_impulses.push_back(sf::Vector2f(x, y));
}
