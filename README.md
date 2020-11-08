Tool for fixing textures names in DAE files of Soul Reaver models converted with Ben Lincoln's ToolChain for importing into Unity.

1. Follow instructions of ToolChain, do everything except script execution inside Blender;
2. Copy AreaModelsMaterial, AreaModelsSpectral and ObjectModels folders from ToolChain build directory to directory with this tool, then run it;
3. Copy AreaModelsMaterial, AreaModelsSpectral and ObjectModels back to ToolChain build directory.
4. Continue following instructions of ToolChain.
5.  When you done importing to Blender, you can import saved .blend file to Unity. Unity will name all materials as textures and can auto-assign textures to materials.

What this tool actually do:

1. Opens each *.dae file in AreaModelsMaterial, AreaModelsSpectral and ObjectModels folders;
2. Renames each <image> node to texture file name;
3. fixes <effect> and <material> nodes to new <image> names;
4. removes duplicate <image>, <effect>, and <material> nodes. Now several meshes can you same material and not doplicate it.
