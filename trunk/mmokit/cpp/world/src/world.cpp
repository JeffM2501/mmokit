// world.cpp : Defines the entry point for the console application.
//

#include "world.h"
#include "textUtils.h"

using namespace TextUtils;
using namespace std;

//AttributeList

bool AttributeList::exists ( const std::string &key )
{
	return attributes.find(key) != attributes.end();
}

bool AttributeList::exists ( const char* key )
{
	return attributes.find(std::string(key)) != attributes.end();
}

const std::string& AttributeList::get ( const std::string &key )
{
	static std::string blank;
	std::map<std::string,std::string>::iterator itr = attributes.find(key);
	if (itr == attributes.end())
		return blank;
	return itr->second;
}

const std::string& AttributeList::get ( const char* key )
{
	static std::string blank;
	std::map<std::string,std::string>::iterator itr = attributes.find(std::string(key));
	if (itr == attributes.end())
		return blank;
	return itr->second;
}

void AttributeList::set ( const std::string &key, const std::string &value )
{
	attributes[key] = value;
}

void AttributeList::set ( const char* key, const std::string &value )
{
	if (key)
		attributes[std::string(key)] = value;
}

void AttributeList::set ( const std::string &key, const  char* value )
{
	if (value)
		attributes[key] = std::string(value);
}

void AttributeList::set ( const char *key, const char* value )
{
	if (key && value)
		attributes[std::string(key)] = std::string(value);
}


//WorldMesh
void WorldMesh::finalise ( void )
{
	// go thru the faces and verify that all the indexes are valid, ditching any faces that have invalid indexes
	std::vector<size_t> badFaces;

	for (size_t f = 0; f < faces.size(); f++ )
	{
		Face &face = faces[f];

		bool bad = false;
		for ( size_t v = 0; v < face.corners.size(); v++ )
		{
			FaceVert &corner = face.corners[v];
			if ( corner.v >= verts.size() || corner.n >= norms.size() || corner.u >= uvs.size())
				bad = true;
		}

		if (bad)
			badFaces.push_back(f);
	}

	std::vector<size_t>::reverse_iterator itr = badFaces.rbegin();
	while (itr != badFaces.rend())
	{
		faces.erase(faces.begin()+*itr);
		itr++;
	}
}


//WorldObject
WorldMesh* WorldObject::newMesh ( void )
{
	return new WorldMesh(world);
}

void WorldObject::deleteMesh ( WorldMesh* p )
{
	delete(p);
}

void WorldObject::finalise ( void )
{
	// check the meshes to make sure they are valid
	std::vector<size_t> badMeshes;

	for (size_t m = 0; m < meshes.size(); m++ )
	{
		if (!meshes[m].mesh->faces.size())
			badMeshes.push_back(m);
	}

	std::vector<size_t>::reverse_iterator itr = badMeshes.rbegin();
	while (itr != badMeshes.rend())
	{
		meshes.erase(meshes.begin()+*itr);
		itr++;
	}
}


//WorldCell
WorldObject* WorldCell::newObject ( void )
{
	return new WorldObject(world);
}

void WorldCell::deleteObject ( WorldObject* p )
{
	delete(p);
}

void WorldCell::finalise ( void )
{
	// check the meshes to make sure they are valid
	std::vector<size_t> badObjects;

	for (size_t o = 0; o < objects.size(); o++ )
	{
		if (!objects[o]->meshes.size())
			badObjects.push_back(o);
	}

	std::vector<size_t>::reverse_iterator itr = badObjects.rbegin();
	while (itr != badObjects.rend())
	{
		objects.erase(objects.begin()+*itr);
		itr++;
	}
}

//World
WorldCell* World::newCell ( void )
{
	return new WorldCell(this);
}

void World::deleteCell ( WorldCell* p )
{
	delete(p);
}

WorldMaterial* World::newMaterial ( void )
{
	return new WorldMaterial(this);
}

void World::deleteMaterial ( WorldMaterial* p )
{
	delete(p);
}

void World::finalise ( void )
{
	// check the meshes to make sure they are valid
	std::vector<size_t> badCells;

	for (size_t c = 0; c < cells.size(); c++ )
	{
		if (!cells[c]->objects.size())
			badCells.push_back(c);
	}

	std::vector<size_t>::reverse_iterator itr = badCells.rbegin();
	while (itr != badCells.rend())
	{
		cells.erase(cells.begin()+*itr);
		itr++;
	}
}


void World::clear ( void )
{
	for (size_t c = 0; c < cells.size(); c++)
	{
		WorldCell *cell = cells[c];

		if (cell)
		{
			for (size_t o = 0; o < cell->objects.size(); o++)
			{	
				WorldObject *object = cell->objects[o];
				if (object)
				{
					for (size_t m = 0; m < object->meshes.size(); m++)
					{
						WorldObject::ObjectMesh &mesh = object->meshes[m];
						if (mesh.mesh)
							object->deleteMesh(mesh.mesh);
					}
					cell->deleteObject(object);
				}
			}
			deleteCell(cell);
		}
	}
	cells.clear();
	name = "";

	for (size_t m = 0; m < materials.size(); m++)
	{
		if (materials[m])
			deleteMaterial(materials[m]);
	}

	materials.clear();
}

//WorldStreamReader
WorldStreamReader::WorldStreamReader(World &w): world(w)
{
}

bool WorldStreamReader::read ( std::istream &input )
{
	string token;
	input >> token;

	world.clear();

	WorldCell					*cell = NULL;
	WorldObject					*object = NULL;
	WorldObject::ObjectMesh		mesh;
	WorldMesh::Face				face;
	WorldMaterial				*material = NULL;
//	char temp[512];

	if ( same_no_case(token,"world:") )
	{
		// read the name
		input >> world.name;
		input >> token;

		while (token.size())
		{
			if (same_no_case(token,"material:"))
			{
				if (material && material->name.size())
				{
					material->finalise();
					world.materials.push_back(material);
				}
				else if (material)
					world.deleteMaterial(material);

				material = world.newMaterial();
				input >> material->name;
			}
			else if (same_no_case(token,"texture:"))
			{
				if (material)
					input >> material->texture;
			}
			else if (same_no_case(token,"cell:"))
			{
				if (cell && cell->objects.size())
				{
					cell->finalise();
					world.cells.push_back(cell);
				}
				else if (cell)
					world.deleteCell(cell);

				cell = world.newCell();

				size_t cellID;
				input >> cellID;
			}
			else if (same_no_case(token,"object:"))
			{
				if (object && object->meshes.size() && cell)
				{
					object->finalise();
					cell->objects.push_back(object);
				}
				else if (object && cell)
					cell->deleteObject(object);

				if (cell)
					object = cell->newObject();
				else
					object = NULL;

				size_t objectID;
				input >> objectID;
			}
			else if (same_no_case(token,"transform:"))
			{
				if (object)
					input >> object->transform;
			}
			else if (same_no_case(token,"mesh:"))
			{
				if (mesh.mesh && mesh.mesh->faces.size() && object)
				{
					mesh.mesh->finalise();
					object->meshes.push_back(mesh);
				}
				else if (mesh.mesh && object)
					object->deleteMesh(mesh.mesh);

				mesh.mesh = NULL;
				mesh.material = "";

				if (object)
				{
					mesh.mesh = object->newMesh();
					input >> mesh.material;
				}
			}
			else if (same_no_case(token,"vert:"))
			{
				if (mesh.mesh)
				{
					Vector3 v;
					input >> v;
					mesh.mesh->verts.push_back(v);
				}
			}
			else if (same_no_case(token,"norm:"))
			{
				if (mesh.mesh)
				{
					Vector3 v;
					input >> v;
					mesh.mesh->norms.push_back(v);
				}
			}
			else if (same_no_case(token,"uv:"))
			{
				if (mesh.mesh)
				{
					Vector2 v;
					input >> v;
					mesh.mesh->uvs.push_back(v);
				}
			}
			else if (same_no_case(token,"face:"))
			{
				if (face.corners.size() && mesh.mesh)
					mesh.mesh->faces.push_back(face);

				face.corners.clear();
				face.normal = Vector3();

				int faceID;
				input >> faceID;
			}
			else if (same_no_case(token,"normal:"))
			{
				input >> face.normal;
			}
			else if (same_no_case(token,"corner:"))
			{
				WorldMesh::FaceVert fv;

				input >> fv.v >> fv.n >> fv.u;
				face.corners.push_back(fv);
			}
			else if (same_no_case(token,"attribute:"))
			{
				std::string tag, key, value;
				input >> tag >> key >> value;
				if (same_no_case(tag,"world:"))
					world.attributes.set(key,value);
				else if (same_no_case(tag,"cell:") && cell)
					cell->attributes.set(key,value);
				else if (same_no_case(tag,"object:") && object)
					object->attributes.set(key,value);
				else if (same_no_case(tag,"material:") && material)
					material->attributes.set(key,value);
			}

			token = "";
			input >> token;
		}
	}

	if (face.corners.size() && mesh.mesh)
		mesh.mesh->faces.push_back(face);
	
	if (mesh.mesh && mesh.mesh->faces.size() && object)
	{
		mesh.mesh->finalise();
		object->meshes.push_back(mesh);
	}
	else if ( mesh.mesh && object)
		object->deleteMesh(mesh.mesh);

	if (object && object->meshes.size() && cell)
	{
		object->finalise();
		cell->objects.push_back(object);
	}
	else if (object && cell)
		cell->deleteObject(object);

	if (cell && cell->objects.size())
	{
		cell->finalise();
		world.cells.push_back(cell);
	}
	else if (cell)
		world.deleteCell(cell);

	if (material && material->name.size())
	{
		material->finalise();
		world.materials.push_back(material);
	}
	else if (material)
		world.deleteMaterial(material);

	world.finalise();
	return world.cells.size() > 0;
}

//WorldStreamWriter
WorldStreamWriter::WorldStreamWriter(World &w): world(w)
{
}

void WorldStreamWriter::dumpAttributes ( ostream &output, const char* tag, const AttributeList &attributes )
{
	for ( std::map<std::string,std::string>::const_iterator a = attributes.attributes.begin(); a != attributes.attributes.end(); a++ )
		output << "attribute: " << tag << ": " << a->first << " " << a->second << "\n";
} 

bool WorldStreamWriter::write ( std::ostream &output )
{
	if (!world.cells.size())
		return false;

	output << "world: " << world.name << "\n";

	dumpAttributes(output,"world",world.attributes);

	for ( size_t m = 0; m < world.materials.size(); m++ )
	{
		WorldMaterial *material = world.materials[m];
		if (material)
		{
			output << "material: " << material->name << "\n";
			if (material->texture.size())
				output << "texture: " << material->texture << "\n";

			dumpAttributes(output,"material", material->attributes);
		}
	}

	for ( size_t c = 0; c < world.cells.size(); c++ )
	{
		WorldCell *cell = world.cells[c];
		if (cell)
		{
			output << "cell: " << c << "\n";

			dumpAttributes(output,"cell",cell->attributes);

			for ( size_t o = 0; o < cell->objects.size(); o++ )
			{
				output << "object: " << o << "\n";
				WorldObject * object = cell->objects[o];

				dumpAttributes(output,"object",object->attributes);

				output << "transform: " << object->transform << "\n";

				for ( size_t m = 0; m < object->meshes.size(); m++ )
				{
					const WorldObject::ObjectMesh& mesh = object->meshes[m];

					output << "mesh: " << mesh.material << "\n";

					for ( size_t v = 0; v < mesh.mesh->verts.size(); v++ )
						output << "vert: " << mesh.mesh->verts[v] << "\n";

					for ( size_t n = 0; n < mesh.mesh->norms.size(); n++ )
						output << "norm: " << mesh.mesh->norms[n] << "\n";

					for ( size_t u = 0; u < mesh.mesh->uvs.size(); u++ )
						output << "uv: " << mesh.mesh->uvs[u] << "\n";

					for ( size_t f =0; f < mesh.mesh->faces.size(); f++ )
					{
						WorldMesh::Face &face = mesh.mesh->faces[f];
						output << "face: " << f << "\n";
						output << "normal: " << face.normal << "\n";

						for ( size_t v = 0; v < face.corners.size(); v++ )
							output << "corner: " << face.corners[v].v << " " << face.corners[v].n << " " << face.corners[v].u << "\n";
					}
				}
			}
		}
	}
	return true;
}

