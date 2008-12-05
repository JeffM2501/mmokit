// collisionWorld.h : Defines the entry point for the console application.
//

#ifndef _COLLISION_WORLD_H_
#define _COLLISION_WORLD_H_
#include "world.h"

class BoundingInfo
{
public:
	BoundingInfo();

	Vector3 center;
	Vector3 bboxSize, bbox[2];
	float radius;
	bool set;

	void clear ( void );

	void add ( const Vector3 &vert );

	bool sphereIn ( const Vector3 &pos, const float rad, Vector3 *hit = NULL );
	bool boxIn ( const Vector3 &pos, const Vector3 &size, Vector3 *hit = NULL );
	bool boxIn ( const Vector3 &pos, const Vector3 &size, const Matrix34& transform, Vector3 *hit = NULL );
};

class CollisionMesh : public WorldMesh
{
public:
	CollisionMesh(World*w):WorldMesh(w){};
	virtual ~CollisionMesh(){};

	virtual void finalise ( void );

	virtual bool collide ( const Vector3 &pos, const float rad, Vector3 *hit);
	virtual bool collide ( const Vector3 &pos, const Vector3 &size, Vector3 *hit);
	virtual bool collide ( const Vector3 &pos, const Vector3 &size, const Matrix34& transform, Vector3 *hit);

	BoundingInfo bounds;
};

class CollisionObject : public WorldObject
{
public:
	CollisionObject(World*w):WorldObject(w){};
	virtual ~CollisionObject(){};

	virtual void finalise ( void );

	virtual WorldMesh* newMesh ( void );
	virtual void deleteMesh ( WorldMesh* p );

	virtual bool collide ( const Vector3 &pos, const float rad, Vector3 *hit);
	virtual bool collide ( const Vector3 &pos, const Vector3 &size, Vector3 *hit);
	virtual bool collide ( const Vector3 &pos, const Vector3 &size, const Matrix34& transform, Vector3 *hit);

	BoundingInfo bounds;
};

class CollisionCell : public WorldCell
{
public:
	CollisionCell(World*w):WorldCell(w){};
	virtual ~CollisionCell(){};

	virtual void finalise ( void );

	virtual WorldObject* newObject ( void );
	virtual void deleteObject ( WorldObject* p );

	virtual bool collide ( const Vector3 &pos, const float rad, Vector3 *hit);
	virtual bool collide ( const Vector3 &pos, const Vector3 &size, Vector3 *hit);
	virtual bool collide ( const Vector3 &pos, const Vector3 &size, const Matrix34& transform, Vector3 *hit);

	BoundingInfo bounds;
};

class CollisionWorld : public World
{
public:
	CollisionWorld():World(){};
	virtual ~CollisionWorld(){};

	virtual void finalise ( void );

	virtual WorldCell* newCell ( void );
	virtual void deleteCell ( WorldCell* p );

	virtual bool collide ( const Vector3 &pos, const float rad, Vector3 *hit);
	virtual bool collide ( const Vector3 &pos, const Vector3 &size, Vector3 *hit);
	virtual bool collide ( const Vector3 &pos, const Vector3 &size, const Matrix34& transform, Vector3 *hit);

	BoundingInfo bounds;
};

#endif _COLLISION_WORLD_H_
