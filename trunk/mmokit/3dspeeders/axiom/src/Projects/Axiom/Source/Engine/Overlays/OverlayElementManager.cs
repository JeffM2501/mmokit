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
//     <id value="$Id: OverlayElementManager.cs 1004 2007-01-02 18:57:49Z borrillis $"/>
// </file>
#endregion SVN Version Information

#region Namespace Declarations

using System;
using System.Collections;
using System.Diagnostics;

using Axiom.Core;
using Axiom.Overlays.Elements;

#endregion Namespace Declarations

#region Ogre Synchronization Information
/// <ogresynchronization>
///     <file name="OgreOverlayManager.h"   revision="1.23.2.1" lastUpdated="10/5/2005" lastUpdatedBy="DanielH" />
///     <file name="OgreOverlayManager.cpp" revision="1.39.2.3" lastUpdated="10/5/2005" lastUpdatedBy="DanielH" />
/// </ogresynchronization>
#endregion

namespace Axiom.Overlays
{
    /// <summary>
    ///    This class acts as a repository and regitrar of overlay components.
    /// </summary>
    /// <remarks>
    ///    OverlayElementManager's job is to manage the lifecycle of OverlayElement (subclass)
    ///    instances, and also to register plugin suppliers of new components.
    /// </remarks>
    public sealed class OverlayElementManager : IDisposable
    {
        #region Singleton implementation

        /// <summary>
        ///     Singleton instance of this class.
        /// </summary>
        private static OverlayElementManager instance;

        /// <summary>
        ///     Internal constructor.  This class cannot be instantiated externally.
        /// </summary>
        internal OverlayElementManager()
        {
            if ( instance == null )
            {
                instance = this;

                // register the default overlay element factories
                instance.AddElementFactory( new BorderPanelFactory() );
                instance.AddElementFactory( new TextAreaFactory() );
                instance.AddElementFactory( new PanelFactory() );
            }
        }

        /// <summary>
        ///     Gets the singleton instance of this class.
        /// </summary>
        public static OverlayElementManager Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion Singleton implementation

        #region Fields

        /// <summary>
        ///     List of OverlayElement factories.
        /// </summary>
        private Hashtable factories = new Hashtable();
        /// <summary>
        ///     List of created elements.
        /// </summary>
        private Hashtable instances = new Hashtable();
        /// <summary>
        ///     List of template elements.
        /// </summary>
        private Hashtable templates = new Hashtable();

        #endregion Fields

        #region Methods

        /// <summary>
        ///     Registers a new OverlayElementFactory with this manager.
        /// </summary>
        /// <remarks>
        ///    Should be used by plugins or other apps wishing to provide
        ///    a new OverlayElement subclass.
        /// </remarks>
        /// <param name="factory"></param>
        public void AddElementFactory( IOverlayElementFactory factory )
        {
            factories.Add( factory.Type, factory );

            LogManager.Instance.Write( "OverlayElementFactory for type '{0}' registered.", factory.Type );
        }

        /// <summary>
        ///    Creates a new OverlayElement of the type requested.
        /// </summary>
        /// <param name="typeName">The type of element to create is passed in as a string because this
        ///    allows plugins to register new types of component.</param>
        /// <param name="instanceName">The type of element to create.</param>
        /// <returns></returns>
        public OverlayElement CreateElement( string typeName, string instanceName )
        {
            return CreateElement( typeName, instanceName, false );
        }

        /// <summary>
        ///    Creates a new OverlayElement of the type requested.
        /// </summary>
        /// <param name="typeName">The type of element to create is passed in as a string because this
        ///    allows plugins to register new types of component.</param>
        /// <param name="instanceName">The type of element to create.</param>
        /// <param name="isTemplate"></param>
        /// <returns></returns>
        public OverlayElement CreateElement( string typeName, string instanceName, bool isTemplate )
        {
            Hashtable elements = GetElementTable( isTemplate );

            if ( elements.ContainsKey( instanceName ) )
            {
                //throw new AxiomException( "OverlayElement with the name '{0}' already exists.", instanceName );
                return (OverlayElement)elements[ instanceName ];
            }

            OverlayElement element = CreateElementFromFactory( typeName, instanceName );

            // register
            elements.Add( instanceName, element );

            return element;
        }

        /// <summary>
        ///    Creates an element of the specified type, with the specified name
        /// </summary>
        /// <remarks>
        ///    A factory must be available to handle the requested type, or an exception will be thrown.
        /// </remarks>
        /// <param name="typeName"></param>
        /// <param name="instanceName"></param>
        /// <returns></returns>
        public OverlayElement CreateElementFromFactory( string typeName, string instanceName )
        {
            if ( !factories.ContainsKey( typeName ) )
            {
                throw new AxiomException( "Cannot locate factory for element type '{0}'", typeName );
            }

            // create the element
            return ( (IOverlayElementFactory)factories[ typeName ] ).Create( instanceName );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="typeName"></param>
        /// <param name="instanceName"></param>
        /// <param name="isTemplate"></param>
        /// <returns></returns>
        public OverlayElement CreateElementFromTemplate( string templateName, string typeName, string instanceName, bool isTemplate )
        {
            OverlayElement element = null;

            if ( templateName.Length == 0 )
            {
                element = CreateElement( typeName, instanceName, isTemplate );
            }
            else
            {
                OverlayElement template = GetElement( templateName, true );

                string typeToCreate = "";
                if ( typeName.Length == 0 )
                {
                    typeToCreate = template.GetType().Name;
                }
                else
                {
                    typeToCreate = typeName;
                }

                element = CreateElement( typeToCreate, instanceName, isTemplate );

                // Copy settings from template
                element.CopyFromTemplate( template );
            }

            return element;
        }

        /// <summary>
        ///    Gets a reference to an existing element.
        /// </summary>
        /// <param name="name">Name of the element to retrieve.</param>
        /// <param name="isTemplate"></param>
        /// <returns></returns>
        public OverlayElement GetElement( string name )
        {
            Hashtable elements = GetElementTable( false );

            Debug.Assert( elements[ name ] != null, string.Format( "OverlayElement with the name'{0}' was not found.", name ) );

            return (OverlayElement)elements[ name ];
        }

        /// <summary>
        ///    Gets a reference to an existing element.
        /// </summary>
        /// <param name="name">Name of the element to retrieve.</param>
        /// <param name="isTemplate"></param>
        /// <returns></returns>
        public OverlayElement GetElement( string name, bool isTemplate )
        {
            Hashtable elements = GetElementTable( isTemplate );

            Debug.Assert( elements[ name ] != null, string.Format( "OverlayElement with the name'{0}' was not found.", name ) );

            return (OverlayElement)elements[ name ];
        }

        /// <summary>
        ///    Quick helper method to return the lookup table for the right element type.
        /// </summary>
        /// <param name="isTemplate"></param>
        /// <returns></returns>
        private Hashtable GetElementTable( bool isTemplate )
        {
            return isTemplate ? templates : instances;
        }

        #endregion Methods

        #region IDisposable Implementation

        /// <summary>
        ///     Called when the engine is shutting down.
        /// </summary>
        public void Dispose()
        {
            instance = null;
        }

        #endregion IDisposable Implementation
    }
}