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
//     <id value="$Id: IHardwareOcclusionQuery.cs 1004 2007-01-02 18:57:49Z borrillis $"/>
// </file>
#endregion SVN Version Information

#region Namespace Declarations

using System;

#endregion Namespace Declarations

namespace Axiom.Graphics
{
    /// <summary>
    ///		Interface specification for hardware queries that can be used to find the number
    ///		of fragments rendered by the last render operation.
    /// </summary>
    /// Original Author: Lee Sandberg.
    public interface IHardwareOcclusionQuery
    {
        /// <summary>
        ///		Begins the query.
        /// </summary>
        void Begin();

        /// <summary>
        ///		Ends the current active occlusion test.
        /// </summary>
        void End();

        /// <summary>
        ///		Checks to see if there are results returned from the most recent execution
        ///		of this query.
        /// </summary>
        /// <param name="flush">
        ///		True if currently batched API calls should be processed.
        ///		Note: Only D3D uses this parameter at this time.
        /// </param>
        /// <returns>The number of fragment returned by the query.</returns>
        int PullResults( bool flush );

        /// <summary>
        ///		Gets the fragment count from the last execution of this query.
        /// </summary>
        int LastFragmentCount
        {
            get;
        }

        /// <summary>
        ///		Gets/Sets the number of frames that are skipped between each execution of the
        ///		query.
        /// </summary>
        int SkipRate
        {
            get;
            set;
        }
    }
}