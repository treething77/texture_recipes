# texture_recipes
Unity tool for offline or runtime procedural texture creation.

The main part of this tool is a node-based shader-creation tool specifically aimed at creating procedural textures. Some of the nodes in the editor include:
-texture sample
-noise
-recolor
-scale
-rotate

A recipe is comprised of layers, where each layer is either a shader layer or a text layer. The inclusion of text rendering was important for the use cases I was considering when developing this.

Another big consideration was WebGL support. Every feature works in WebGL.

Examples:
-Soccer - Create the textures for two soccer teams from a list of player names and team logos.
-Spaceships - Create a bunch of random spaceship textures.
-Spaceships2 - Slightly more complex version that requires some script support.