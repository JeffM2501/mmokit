// world.cpp : Defines the entry point for the console application.
//

#include "collisionWorld.h"
#include "textUtils.h"

using namespace TextUtils;
using namespace std;

//CollisionMesh
void greaterAxis ( Vector3 &result, const Vector3 &input )
{
	if ( input.x() > result.x() )
		result.x(input.x());
	if ( input.y() > result.y() )
		result.y(input.y());
	if ( input.z() > result.z() )
		result.z(input.z());
}

void lesserAxis ( Vector3 &result, const Vector3 &input )
{
	if ( input.x() < result.x() )
		result.x(input.x());
	if ( input.y() < result.y() )
		result.y(input.y());
	if ( input.z() < result.z() )
		result.z(input.z());
}

void CollisionMesh::finalise ( void )
{
	WorldMesh::finalise();

	// build up the bbox and rad

	Vector3 bbox[2];

	if (!verts.size())
		return;

	bbox[0] = bbox[1] = verts[0];
	
	for (size_t f = 0; f < faces.size(); f++ )
	{
		Face &face = faces[f];

		bool bad = false;
		for ( size_t v = 0; v < face.corners.size(); v++ )
		{
			FaceVert &corner = face.corners[v];

			greaterAxis(bbox[0],verts[corner.v]);
			lesserAxis(bbox[1],verts[corner.v]);
		}
	}

	bboxSize = bbox[0] - bbox[1];
	bboxSize *= 0.5f;
	radius = bboxSize.magnitude();
	center += bboxSize + bbox[1];
}

bool CollisionMesh::collide ( const Vector3 &pos, const float rad, Vector3 *hit)
{
	Vector3 vec = pos - center;

	float dist = vec.magnitude();

	if ( abs(dist) > radius+rad)
		return false;

	if (hit)
	{
		dist -= rad;
		vec.normalise();
		vec *= dist;
		*hit = pos + vec;
	}
	return true;
}

bool CollisionMesh::collide ( const Vector3 &pos, const Vector3 &size, Vector3 *hit)
{
	return collide(pos,size.magnitude(),hit);
}

bool CollisionMesh::collide ( const Vector3 &pos, const Vector3 &size, const Matrix34& /*transform*/, Vector3 *hit)
{
	return collide(pos,size.magnitude(),hit);
}

//WorldObject
WorldMesh* CollisionObject::newMesh ( void )
{
	return new CollisionMesh();
}

void CollisionObject::deleteMesh ( WorldMesh* p )
{
	delete(p);
}

bool CollisionObject::collide ( const Vector3 &pos, const float rad, Vector3 *hit)
{
	for (size_t i = 0; i < meshes.size(); i++)
	{
		CollisionMesh* mesh = (CollisionMesh*)meshes[i].mesh;

		if (mesh->collide(pos,rad,hit))
			return true;
	}
	return false;
}

bool CollisionObject::collide ( const Vector3 &pos, const Vector3 &size, Vector3 *hit)
{
	for (size_t i = 0; i < meshes.size(); i++)
	{
		CollisionMesh* mesh = (CollisionMesh*)meshes[i].mesh;

		if (mesh->collide(pos,size,hit))
			return true;
	}
	return false;
}

bool CollisionObject::collide ( const Vector3 &pos, const Vector3 &size, const Matrix34& transform, Vector3 *hit)
{
	for (size_t i = 0; i < meshes.size(); i++)
	{
		CollisionMesh* mesh = (CollisionMesh*)meshes[i].mesh;

		if (mesh->collide(pos,size,transform,hit))
			return true;
	}
	return false;
}

//CollisionCellCell
WorldObject* CollisionCell::newObject ( void )
{
	return new CollisionObject();
}

void CollisionCell::deleteObject ( WorldObject* p )
{
	delete(p);
}

bool CollisionCell::collide ( const Vector3 &pos, const float rad, Vector3 *hit)
{
	for (size_t i = 0; i < objects.size(); i++)
	{
		CollisionObject* object = (CollisionObject*)objects[i];

		if (object->collide(pos,rad,hit))
			return true;
	}
	return false;
}

bool CollisionCell::collide ( const Vector3 &pos, const Vector3 &size, Vector3 *hit)
{
	for (size_t i = 0; i < objects.size(); i++)
	{
		CollisionObject* object = (CollisionObject*)objects[i];

		if (object->collide(pos,size,hit))
			return true;
	}
	return false;
}

bool CollisionCell::collide ( const Vector3 &pos, const Vector3 &size, const Matrix34& transform, Vector3 *hit)
{
	for (size_t i = 0; i < objects.size(); i++)
	{
		CollisionObject* object = (CollisionObject*)objects[i];

		if (object->collide(pos,size,transform,hit))
			return true;
	}
	return false;
}

//CollisionWorld
WorldCell* CollisionWorld::newCell ( void )
{
	return new CollisionCell();
}

void CollisionWorld::deleteCell ( WorldCell* p )
{
	delete(p);
}

bool CollisionWorld::collide ( const Vector3 &pos, const float rad, Vector3 *hit)
{
	for (size_t i = 0; i < cells.size(); i++)
	{
		CollisionCell* cell = (CollisionCell*)cells[i];

		if (cell->collide(pos,rad,hit))
			return true;
	}
	return false;
}

bool CollisionWorld::collide ( const Vector3 &pos, const Vector3 &size, Vector3 *hit)
{
	for (size_t i = 0; i < cells.size(); i++)
	{
		CollisionCell* cell = (CollisionCell*)cells[i];

		if (cell->collide(pos,size,hit))
			return true;
	}
	return false;
}

bool CollisionWorld::collide ( const Vector3 &pos, const Vector3 &size, const Matrix34& transform, Vector3 *hit)
{
	for (size_t i = 0; i < cells.size(); i++)
	{
		CollisionCell* cell = (CollisionCell*)cells[i];

		if (cell->collide(pos,size,transform,hit))
			return true;
	}
	return false;
}

