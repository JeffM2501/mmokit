// world.cpp : Defines the entry point for the console application.
//

#include "drawableWorld.h"
#include "textUtils.h"

using namespace std;
using namespace TextUtils;

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
	return new DrawableMesh(world);
}

void DrawableObject::deleteMesh ( WorldMesh* p )
{
	delete(p);
}

void DrawableObject::finalise ( void )
{
	CollisionObject::finalise();
	materialIDs.clear();

	for (size_t i = 0; i < meshes.size(); i++)
	{
		DrawableMesh* mesh = (DrawableMesh*)meshes[i].mesh;
		mesh->matrix = transform;

		MaterialID id = _INVALID_MATERIAL;
		if (world)
		{
			DrawableWorld *dw = (DrawableWorld*)world;

			DrawableMaterial *mat = dw->findMaterial(meshes[i].material);
			if (mat)
				id = mat->material;
		}
		materialIDs.push_back(id);
	}
}

void DrawableObject::draw ( void )
{
	// take transform to the GL
	for (size_t i = 0; i < meshes.size(); i++)
	{
		DrawableMesh* mesh = (DrawableMesh*)meshes[i].mesh;
		mesh->matrix = transform;
		DrawablesSystem::Instance().addItem(materialIDs[i],mesh);
	}
}

//DrawableCell
WorldObject* DrawableCell::newObject ( void )
{
	return new DrawableObject(world);
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

//DrawableMaterial

DrawableMaterial::~DrawableMaterial()
{
	MaterialsSystem::Instance().deleteMaterial(material);
}

void DrawableMaterial::finalise ( void )
{
	MaterialsSystem &ms = MaterialsSystem::Instance();

	float diffuse[4] = {1,1,1,1};
	const std::string &dif = attributes.get("diffuse");

	if (dif.size())
	{
		std::vector<std::string> chunk = tokenize(dif,std::string("."));
		if (chunk.size())
			diffuse[0] = (float)atof(chunk[0].c_str());
		if (chunk.size() > 1)
			diffuse[1] = (float)atof(chunk[1].c_str());
		if (chunk.size() > 2)
			diffuse[2] = (float)atof(chunk[2].c_str());
		if (chunk.size() > 3)
			diffuse[3] = (float)atof(chunk[3].c_str());
	}

	Material *mat = ms.newMaterial(texture.c_str(),diffuse[0],diffuse[1],diffuse[2],diffuse[3]);

	if (mat)
		material = mat->id;
	else 
		material = _INVALID_MATERIAL;
}

//DrawableWorld
WorldCell* DrawableWorld::newCell ( void )
{
	return new DrawableCell(this);
}

void DrawableWorld::deleteCell ( WorldCell* p )
{
	delete(p);
}

WorldMaterial* DrawableWorld::newMaterial ( void )
{
	return new DrawableMaterial(this);
}

void DrawableWorld::deleteMaterial ( WorldMaterial* p )
{
	delete(p);
}

void DrawableWorld::draw ( void )
{
	DrawablesSystem::Instance().useMaterials(true);

	for (size_t i = 0; i < cells.size(); i++)
	{
		DrawableCell* cell = (DrawableCell*)cells[i];
		cell->draw();
	}
}

void DrawableWorld::finalise ( void )
{
	CollisionWorld::finalise();

	materialNameMap.clear();
	for (size_t i = 0; i < materials.size(); i++)
		materialNameMap[materials[i]->name] = i;
}

DrawableMaterial* DrawableWorld::findMaterial ( const std::string &name )
{
	std::map<std::string,size_t>::iterator itr = materialNameMap.find(name);
	if (itr == materialNameMap.end())
		return NULL;

	return (DrawableMaterial*)materials[itr->second];
}


