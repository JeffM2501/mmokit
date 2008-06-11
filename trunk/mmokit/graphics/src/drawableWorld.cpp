// world.cpp : Defines the entry point for the console application.
//

#include "drawableWorld.h"
using namespace std;

void DrawableMesh::draw ( DrawableID id, int matID )
{
	if (list == GL_INVALID_ID)
		list = DisplayListSystem::Instance().newList(this);

	DisplayListSystem::Instance().callList(list);
}

void DrawableMesh::buildGeometry ( GLDisplayList displayList )
{
	glPushMatrix();
	glColor4f(1,1,1,1);
	for (size_t f = 0; f < faces.size(); f++ )
	{
		glBegin(GL_POLYGON);

		Face &face = faces[f];
		for ( size_t v = 0; v < face.corners.size(); v++ )
		{
			FaceVert &corner = face.corners[v];

			glNormal3fv(norms[corner.n].vec);
			glTexCoord2fv(uvs[corner.u].vec);
			glVertex3fv(verts[corner.v].vec);
		}
		glEnd();
	}
	glPopMatrix();
}


//WorldObject
WorldMesh* DrawableObject::newMesh ( void )
{
	return new DrawableMesh();
}

void DrawableObject::deleteMesh ( WorldMesh* p )
{
	delete(p);
}

void DrawableObject::finalise ( void )
{
	CollisionObject::finalise();
	for (size_t i = 0; i < meshes.size(); i++)
	{
		DrawableMesh* mesh = (DrawableMesh*)meshes[i].mesh;
		mesh->matrix = transform;
	}
}

void DrawableObject::draw ( void )
{
	// take transform to the GL
	for (size_t i = 0; i < meshes.size(); i++)
	{
		int textureID;
		DrawableMesh* mesh = (DrawableMesh*)meshes[i].mesh;
		mesh->matrix = transform;

		if ( i >= textureIDs.size())
		{
			textureID = TextureSystem::Instance().getID(meshes[i].material.c_str());
			textureIDs.push_back(textureID);
		}
		else
			textureID = textureIDs[i];

		DrawablesSystem::Instance().addItem(textureID,mesh);
	}
}

//DrawableCell
WorldObject* DrawableCell::newObject ( void )
{
	return new DrawableObject();
}

void DrawableCell::deleteObject ( WorldObject* p )
{
	delete(p);
}

void DrawableCell::draw ( void )
{
	for (size_t i = 0; i < objects.size(); i++)
	{
		DrawableObject* object = (DrawableObject*)objects[i];

		object->draw();
	}
}

//DrawableWorld
WorldCell* DrawableWorld::newCell ( void )
{
	return new DrawableCell();
}

void DrawableWorld::deleteCell ( WorldCell* p )
{
	delete(p);
}

void DrawableWorld::draw ( void )
{
	for (size_t i = 0; i < cells.size(); i++)
	{
		DrawableCell* cell = (DrawableCell*)cells[i];
		cell->draw();
	}
}

