// GPx01
// Copyright (c) 2005 - 2006 Jeff Myers
//
// This package is free software;  you can redistribute it and/or
// modify it under the terms of the license found in the file
// named COPYING that should have accompanied this file.
//
// THIS PACKAGE IS PROVIDED ``AS IS'' AND WITHOUT ANY EXPRESS OR
// IMPLIED WARRANTIES, INCLUDING, WITHOUT LIMITATION, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
//

#include "materials.h"
#include "textures.h"

Material::Material ( float r, float g, float b, float a )
{
	texutreID = -1;
	diffuse.set(r,g,b,a);

	specular = ambient = diffuse;
	shine = 0;
}

Material::Material ( const char* texture, float a )
{
	texutreID = -1;
	shine = 0;

	setTexture(texture);
	diffuse.set(diffuse.getV()[0],diffuse.getV()[1],diffuse.getV()[2],a);
}

Material::Material ( const Material* mat )
{
	texutreID = -1;
	shine = 0;

	if (mat)
	{
		texutreID = mat->texutreID;
		diffuse = mat->diffuse;
		specular = mat->specular;
		ambient = mat->ambient;
		shine = mat->shine;
	}
}

Material::Material ( const Material& mat )
{
	texutreID = mat.texutreID;
	diffuse = mat.diffuse;
	specular = mat.specular;
	ambient = mat.ambient;
	shine = mat.shine;
}

void Material::setDiffuse ( float r, float g, float b, float a )
{
	diffuse.set(r,g,b,a);
}

void Material::setAmbient ( float r, float g, float b )
{
	ambient.set(r,g,b);
}

void Material::setSpecular ( float r, float g, float b )
{
	specular.set(r,g,b);
}

void Material::setShine ( float s )
{
	shine = s;
}

void Material::setTexture ( const char* texture )
{
	texutreID = TextureSystem::Instance().getID(texture);
}

void Material::setGL ( Material *lastMaterial )
{
	if (lastMaterial == this || *lastMaterial == *this )
		return;

	if (!lastMaterial)	// no last material, do a full state
	{
		diffuse.setGL();
		if (isTextured())
		{
			glEnable(GL_TEXTURE_2D);
			TextureSystem::Instance().bind(texutreID);
		}
		else
			glDisable(GL_TEXTURE_2D);
	}
	else	// there was a last mat, do a delta state
	{
		if (isTextured() && lastMaterial->isTextured())	// texturing was on 
		{
			if (texutreID != lastMaterial->texutreID) // only need a new texture
				TextureSystem::Instance().bind(texutreID);
		}
		else	// texturing is not the same
		{
			if (isTextured()) // it's on now, and was off, so enable it
			{
				glEnable(GL_TEXTURE_2D);
				TextureSystem::Instance().bind(texutreID);
			}
			else if (lastMaterial->isTextured()) // it's off now, and was on so disable it.
				glDisable(GL_TEXTURE_2D);
		}

		if (diffuse != lastMaterial->diffuse) // the colors are not the same, change em
			diffuse.setGL();
	}
}

bool Material::isTextured ( void )
{
	return texutreID >= 0;
}

bool Material::operator == ( const Material &m ) const
{
	return diffuse == m.diffuse && texutreID == m.texutreID;
}

MaterialsSystem::MaterialsSystem()
{
	lastID = 0;
}

Material* MaterialsSystem::findMat ( Material* mat )
{
	if (!mat)
		return NULL;

	MaterialMap::iterator itr = materials.begin();
	while ( itr != materials.end())
	{
		if ( *mat == *(itr->second))
			return itr->second;
		itr++;
	}

	return NULL;
}

Material* MaterialsSystem::newMaterial ( void )
{
	Material *mat = new Material();
	mat->id = lastID;
	
	Material *m = findMat(mat);
	if (m)
	{
		delete mat;
		return m;
	}

	materials[lastID++] = mat;
	return mat;
}

Material* MaterialsSystem::newMaterial ( float r , float g , float b , float a )
{
	Material *mat = new Material(r,g,b,a);
	mat->id = lastID;

	Material *m = findMat(mat);
	if (m)
	{
		delete mat;
		return m;
	}

	materials[lastID++] = mat;
	return mat;
}

Material* MaterialsSystem::newMaterial ( const char* texture, float r, float g, float b, float a  )
{
	Material *mat = new Material(r,g,b,a);
	mat->setTexture(texture);
	mat->id = lastID;

	Material *m = findMat(mat);
	if (m)
	{
		delete mat;
		return m;
	}

	materials[lastID++] = mat;
	return mat;
}

Material* MaterialsSystem::newMaterial ( const char* texture, float a )
{
	Material *mat = new Material(texture,a);
	mat->id = lastID;

	Material *m = findMat(mat);
	if (m)
	{
		delete mat;
		return m;
	}

	materials[lastID++] = mat;
	return mat;
}

Material* MaterialsSystem::newMaterial ( std::string texture, float r, float g, float b, float a )
{
	Material *mat = new Material(r,g,b,a);
	mat->setTexture(texture.c_str());
	mat->id = lastID;

	Material *m = findMat(mat);
	if (m)
	{
		delete mat;
		return m;
	}

	materials[lastID++] = mat;
	return mat;

}

Material* MaterialsSystem::newMaterial ( std::string texture, float a )
{
	Material *mat = new Material(texture.c_str(),a);
	mat->id = lastID;

	Material *m = findMat(mat);
	if (m)
	{
		delete mat;
		return m;
	}

	materials[lastID++] = mat;
	return mat;
}

Material* MaterialsSystem::newMaterial( const Material &mat )
{
	Material *newMat = new Material(mat);
	newMat->id = lastID;

	Material *m = findMat(newMat);
	if (m)
	{
		delete newMat;
		return m;
	}

	materials[lastID++] = newMat;
	return newMat;
}

Material* MaterialsSystem::getMaterial ( MaterialID id )
{
	MaterialMap::iterator itr = materials.find(id);
	if (itr == materials.end())
		return NULL;

	return itr->second;
}

void MaterialsSystem::deleteMaterial ( MaterialID id )
{
	MaterialMap::iterator itr = materials.find(id);
	if (itr == materials.end())
		return ;

	materials.erase(itr);
}

void MaterialsSystem::setMaterial ( MaterialID id, Material *lastMaterial )
{
	Material *mat = getMaterial(id);
	
	if (!mat)
		return;

	if (mat == lastMaterial || *mat == *lastMaterial )
		return;

	// send the last in for delta state stuff
	mat->setGL(lastMaterial);
}


