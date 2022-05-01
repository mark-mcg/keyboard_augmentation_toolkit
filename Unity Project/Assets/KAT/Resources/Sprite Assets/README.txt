If you want to generate sprites that can be rendered by TextMeshPro, here's an example of how the emoji sprites were built:

1) Grab https://github.com/iamcal/emoji-data 
2) Select which emojiis you want to use, and dump them in a folder
3) Use https://www.codeandweb.com/texturepacker and pack the sprites into an atlas (JSON Array) - all defaults
4) Use TextMeshPro to create a library from that atlas (Window -> TextMeshPro -> Sprite Importer )
N.B. if the sprites are mis-aligned, chances are Unity is compressing the texture and messing up the UV coordinates. Disable power of two/compression on the texture then generate the library.
N.B.2. Set the imported .png to be a sprite!
5) For the given TextMeshPro element, point it to the library or add it as fallback on EmojiCore
6) If it's a unicode character named as per the emoji-data file names, use the surrogate pair to create the character e.g. \uD83D\uDE00 
   or use the sprite file name directly e.g. <sprite="office" name="Copy"> where office is the library and Copy is the sprite name
7) Put the sprite pack in the Resources/Sprite Assets folder (textmeshpro uses this by default) and it should be picked up in every textmesh
8) Select the EmojiOne sprite atlas and add the new pack as a fallback sprite asset (step 7 might actually be pointless!)