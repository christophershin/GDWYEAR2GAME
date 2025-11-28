Course Project
by Christopher Shin 100974007 and Connor McCarthy 100964926

Video for deliverables:
https://www.youtube.com/watch?v=xgssuoZV3nk


Deliverables:

1. Improvements

A small improvement we made was changing the way the rim lighting shader looked as a bright green didn’t fit the environment. We changed it to a lighter blue so that it would match the colour of the water spouts.
The other big improvement we made was to our build, where all the shaders are now present and working.

I, Christopher Shin made improvements on the color correction as the one we had before wasn't working properly, as it didn't show in runtime and looked a little drastic for this effect. 

<img width="733" height="407" alt="ColorCorrectionCold" src="https://github.com/user-attachments/assets/91584aa1-2acb-49db-9130-7e7894e0fc3c" />

This Cold one is more mellow in appearance, which can induce a feeling of calm. In here, we can use special ice projectiles for our game, such as an ice puck or a snowball.

<img width="731" height="407" alt="ColorCorrectionWarm" src="https://github.com/user-attachments/assets/824dbca7-97a8-488b-bf1e-7cb8fae2b6e3" />

This warm one creates a summer vibe, especially with the water, where we can experiment using fish or other sea animals as projectiles.

<img width="735" height="411" alt="ColorCorrectionCustom" src="https://github.com/user-attachments/assets/f1dcd505-a147-46e3-9c40-96e627611690" />

This custom one is more for an effect that the player experiences when hit with a specific projectile that makes the player stunned or dazed. I wanted to make an effect that looks weird but somewhat clear, as to show the state that the player is in. 

Original Look of the game:

<img width="731" height="407" alt="Original Look" src="https://github.com/user-attachments/assets/6a9123b8-da8b-4b62-87e4-d1bf382cb6b1" />


2. Texturing.

I, Christopher Shin made most of the textures toggleable, which are the water and the scrolling texture. The rest seemed unnecessary as the TA said we didn't need to, as well as I wanted to show what was the most important feature we added.

We added textures to all objects in the scene to enhance the look of the game. 

Made some textures toggleable by pressing 4 on the numpad to turn on the textures, and 0 on the keyboard to turn them off.




4. Visual Effects

I, Christopher Shin added toggling for the visual effects and then made decals, transparency, and fog.

<img width="1919" height="1079" alt="Screenshot 2025-11-27 222052" src="https://github.com/user-attachments/assets/d762a7cb-0324-4720-a008-40dd30c278cb" />
Created a decal shader.
Applied it to a tree stump.
The reason we applied it to a tree stump is because it is in a watery area so mold and fungi usually grow on old trees/ tree stumps.

Tree bark:
https://in.pinterest.com/pin/24769866694930216/

I used this tree texture to resemble a tree, and it also fits the scene and gels well with the decal. 

Decal:
https://s3.amazonaws.com/texturemax_th/decals/stained-decals/stained-decals_0042_01_T_thr.jpg

I used this decal as it resembles closely enough to a mold/fungi texture and adds some more depth to the environment. 



<img width="1919" height="1079" alt="Screenshot 2025-11-27 221041" src="https://github.com/user-attachments/assets/8ecf7c90-9014-496e-9b2b-5a8f7f65a23b" />
Created a transparency shader
Applied to background elements such as the trees and mountain
This is so we don’t have to create a mountain or tree model and then apply textures to it.

Mountain: 
https://www.freepik.com/free-vector/majestic-mountain-range-illustration_238591228.htm#fromView=keyword&page=1&position=0&uuid=8da0f7ef-e56f-40b8-8ad2-153b2a24ad04&query=Mountain+png

I, Chistopher Shin added this mountain texture as it looks like a mountain range that would fit the environment well with the snowy peaks in the distance. 

<img width="1919" height="1079" alt="Screenshot 2025-11-27 222231" src="https://github.com/user-attachments/assets/d080b44a-355c-477e-b202-c646f03e8d0c" />
We created a fog shader
We did this by taking a noise texture, altering its speed and other properties, and then overlay a texture that updates over time.
We then used this to create clouds overhead within our scene.
Source: https://aetuts.itch.io/volumetric-fog-unity-lwrpurp-shader-graph 


I, Connor McCarthy made scrolling texture, used the particle system and made water shader.

<img width="1919" height="1079" alt="Screenshot 2025-11-27 221828" src="https://github.com/user-attachments/assets/64e20310-2344-4ede-965d-724fdae6052a" />
Made a scrolling texture shader.
Made two different materials so they would scroll in opposite directions
Applied it to two twin planes in our scene to create twin waterfalls.

<img width="1919" height="1079" alt="Screenshot 2025-11-27 221746" src="https://github.com/user-attachments/assets/bdadab07-d74d-4fb7-8368-91e5e1a47f73" />
Made a water shader and applied it to a plane.
Didn’t want a scrolling texture here as it would have not made sense at all.
There are hardly any waves with our water as through the inspector we made changes so that the water is calm below

<img width="1919" height="1079" alt="Screenshot 2025-11-27 221528" src="https://github.com/user-attachments/assets/2970c70e-e3c0-46bc-a6d1-481b763a9081" />
Added a particle system to our scene.
Added a gradient through the particle system so that the particles would have a range of colours.
Wanted to replicate water particles in some water for the twin waterfalls.


Link for the fog:
[https://aetuts.itch.io/volumetric-fog-unity-lwrpurp-shader-graph](https://aetuts.itch.io/volumetric-fog-unity-lwrpurp-shader-graph)


