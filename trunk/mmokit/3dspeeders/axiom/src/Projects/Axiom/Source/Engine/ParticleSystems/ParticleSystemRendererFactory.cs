#region LGPL License
/*
Axiom Graphics Engine Library
Copyright (C) 2003-2006 Axiom Project Team

The overall design, and a majority of the core engine and rendering code 
contained within this library is a derivative of the open source Object Oriented 
Graphics Engine OGRE, which can be found at http://ogre.sourceforge.net.  
Many thanks to the OGRE team for maintaining such a high quality project.

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
*/
#endregion

#region SVN Version Information
// <file>
//     <license see="http://axiomengine.sf.net/wiki/index.php/license.txt"/>
//     <id value="$Id: ParticleSystem.cs 884 2006-09-14 06:32:07Z borrillis $"/>
// </file>
#endregion SVN Version Information

#region Namespace Declarations
using System;
using T = Axiom.ParticleSystems.ParticleSystemRenderer;
#endregion Namespace Declarations

namespace Axiom.ParticleSystems
{
    public class ParticleSystemRendererFactory
    {
        /// <summary>
        /// Creates a new object.
        /// </summary>
        /// <param name="name">Name of the object to create</param>
        /// <returns>
        /// An object created by the factory. The type of the object depends on the factory.
        /// </returns>
        public T CreateInstance( string name )
        {
            return new T( name );
        }

        /// <summary>
        /// Destroys an object which was created by this factory.
        /// </summary>
        /// <param name="instance">Pointer to the object to destroy</param>
        public void DestroyInstance( T instance )
        {
            //instance.Dispose();
            instance = null;
        }
    }
}
