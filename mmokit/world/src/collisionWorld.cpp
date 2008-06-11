// world.cpp : Defines the entry point for the console application.
//

#include "collisionWorld.h"

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

BoundingInfo::BoundingInfo()
{
	clear();
}

void BoundingInfo::clear ( void )
{
	set = false;
}

void BoundingInfo::add ( const Vector3 &vert )
{
	if (!set)
	{
		bbox[0] = bbox[1] = vert;
		radius = 0;
		bboxSize = Vector3();
		center = vert;
		set = true;
	}
	else
	{
		greaterAxis(bbox[0],vert);
		lesserAxis(bbox[1],vert);

		bboxSize = bbox[0] - bbox[1];
		bboxSize *= 0.5f;
		radius = bboxSize.magnitude();
		center += bboxSize + bbox[1];
	}
}

bool BoundingInfo::sphereIn ( const Vector3 &pos, const float rad, Vector3 *hit )
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

bool BoundingInfo::boxIn ( const Vector3 &pos, const Vector3 &size, Vector3 *hit)
{
	if (!sphereIn(pos,size.magnitude()))
		return false;

	// this is cheap, do real tests later
	return sphereIn(pos,size.magnitude(),hit);
}

bool BoundingInfo::boxIn ( const Vector3 &pos, const Vector3 &size, const Matrix34& /*transform*/, Vector3 *hit)
{
	if (!sphereIn(pos,size.magnitude()))
		return false;

	// this is cheap, do real tests later
	return sphereIn(pos,size.magnitude(),hit);
}

void CollisionMesh::finalise ( void )
{
	WorldMesh::finalise();

	bounds.clear();
	// build up the bbox and rad

	if (!verts.size())
		return;
	
	for (size_t f = 0; f < faces.size(); f++ )
	{
		Face &face = faces[f];

		bool bad = false;
		for ( size_t v = 0; v < face.corners.size(); v++ )
		{
			FaceVert &corner = face.corners[v];

			bounds.add(verts[corner.v]);
		}
	}
}

bool CollisionMesh::collide ( const Vector3 &pos, const float rad, Vector3 *hit)
{
	return bounds.sphereIn(pos,rad,hit);
}

bool CollisionMesh::collide ( const Vector3 &pos, const Vector3 &size, Vector3 *hit)
{
	return bounds.boxIn(pos,size,hit);
}

bool CollisionMesh::collide ( const Vector3 &pos, const Vector3 &size, const Matrix34& transform, Vector3 *hit)
{
	return bounds.boxIn(pos,size,transform,hit);
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

void CollisionObject::finalise ( void )
{
	bounds.clear();

	float maxRad = 0;

	for (size_t i = 0; i < meshes.size(); i++)
	{
		CollisionMesh* mesh = (CollisionMesh*)meshes[i].mesh;

		bounds.add(mesh->bounds.center);
		if (mesh->bounds.radius > maxRad)
			maxRad = mesh->bounds.radius;

	}
	bounds.radius += maxRad;
}

bool CollisionObject::collide ( const Vector3 &pos, const float rad, Vector3 *hit)
{
	// easy out
	if (!bounds.sphereIn(pos,rad))
		return false;

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
	// easy out
	if (!bounds.sphereIn(pos,size.magnitude()))
		return false;

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
	// easy out
	if (!bounds.sphereIn(pos,size.magnitude()))
		return false;

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

void CollisionCell::finalise ( void )
{
	bounds.clear();

	float maxRad = 0;

	for (size_t i = 0; i < objects.size(); i++)
	{
		CollisionObject* object = (CollisionObject*)objects[i];

		bounds.add(object->bounds.center);
		if (object->bounds.radius > maxRad)
			maxRad = object->bounds.radius;
	}
	bounds.radius += maxRad;
}

bool CollisionCell::collide ( const Vector3 &pos, const float rad, Vector3 *hit)
{
	// easy out
	if (!bounds.sphereIn(pos,rad))
		return false;

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
	// easy out
	if (!bounds.sphereIn(pos,size.magnitude()))
		return false;

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
	// easy out
	if (!bounds.sphereIn(pos,size.magnitude()))
		return false;

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

void CollisionWorld::finalise ( void )
{
	bounds.clear();

	float maxRad = 0;

	for (size_t i = 0; i < cells.size(); i++)
	{
		CollisionCell* cell = (CollisionCell*)cells[i];

		bounds.add(cell->bounds.center);
		if (cell->bounds.radius > maxRad)
			maxRad = cell->bounds.radius;
	}
	bounds.radius += maxRad;
}

bool CollisionWorld::collide ( const Vector3 &pos, const float rad, Vector3 *hit)
{
	// easy out
	if (!bounds.sphereIn(pos,rad))
		return false;

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
	// easy out
	if (!bounds.sphereIn(pos,size.magnitude()))
		return false;

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
	// easy out
	if (!bounds.sphereIn(pos,size.magnitude()))
		return false;

	for (size_t i = 0; i < cells.size(); i++)
	{
		CollisionCell* cell = (CollisionCell*)cells[i];

		if (cell->collide(pos,size,transform,hit))
			return true;
	}
	return false;
}

