ManaSource Sprite Tool
Version 0.0.0.3

Additional info can be found at http://www.awesomelaser.com/?page_id=122

The ManaSource Sprite Tool(MSST) is a cross platform tool use to create and modify sprite XML files for the ManaSource Project, the engine behind The Mana World. The tool is open source under the terms of the LGPL.

The author can be contacted at jeffm2501+msst@gmail.com or as JeffM2501 on The Mana World forums


Install Instructions (Windows)

Unzip the file and run the tool
Install Instructions (Linux and OSX)

Unzip the file. Make sure you have Mono(http://www.mono-project.com/Main_Page). It us usually a package in most linux distributions. OSX users must install it manually. When it is installed, run the msst.sh file
Source Code

The source code is located in a Goggle code SVN repository here: http://code.google.com/p/mmokit/.

The project can be build on windows with Visual Studio 2008 (full or express), or MonoDevelop on linux and OSX
Manual
Overview

The interface for the tool is a little confusing, since it directly ties to the XML that is being edited. The general concepts for the tool are that a number (as in more then one) sprites are loaded and displayed. Each sprite is shown as a layer and can be edited independently. The reason for editing multiple sprites is to allow for easy editing of full outfits, and to see how various sprites interact.

The Main View is where all visible layers are shown. The large red cross represents the sprite origin. The position of the sprite can be be dragged using the right mouse button. Additionally the view can be zoomed using the Zoom item in the View menu. The white grid lines represent 10 pixels at the current zoom, and the lighter grid lines represent a single pixel. Single pixel grid lines will not be shown unless the zoom level is high enough to warrant it.

The Direction buttons will change the view and animations to reflect the selected direction.

The Layers section lists all the layers in the current view. Layers are drawn in the order they are displayed, so items above others in the list will be drawn on top of those times in the view. A layer’s position can be changed using the Up and Down arrow buttons to the right of the layer list. Layers can be added from existing XML files by using the + button, or the Add Layer from File menu item. The - button will remove a layer, and the H button will hide or show a layer. Edits to an individual layer can be saved using the S button, or all layers can be saved using the File menu.

The Action Info section lists all the actions in the currently selected layer, as well as it’s image set. Actions and image sets can be added using the + buttons next to each list, and like layers, the - button will remove them. The i button will allow the current item to be edited.

The Sequence Info section shows the frame segments that make up the animation for the current action/direction combination. Like layers and actions the + and - buttons will and and remove segments. The >| button will step the animation to the next frame in the animation.

The Frame Info and Offset sections show detailed information about the currently selected animation segment. Each segment is ether a single frame, or a series of consecutive frames. Each segment has a single offset from the master sprite origin that is applied to the entire segment.
