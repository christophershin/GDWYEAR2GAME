Course Project
by Christopher Shin 100974007 and Connor McCarthy 100964926


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

Made some textures toggleable by pressing 4 on the numpad to turn on the textures, and 0 on the keyboard to turn them off.

Scrolling Textures:

Made a scrolling texture shader.
Made two different materials so they would scroll in opposite directions
Applied it to two twin planes in our scene to create twin waterfalls.




4. Visual Effects

I, Christopher Shin added toggling for the visual effects and then made decals, transparency, and fog.

<img width="1919" height="1079" alt="Screenshot 2025-11-27 222052" src="https://github.com/user-attachments/assets/d762a7cb-0324-4720-a008-40dd30c278cb" />
Created a decal shader.
Applied it to a tree stump.
The reason we applied it to a tree stump is because it is in a watery area so mold and fungi usually grow on old trees/ tree stumps.

<img width="1919" height="1079" alt="Screenshot 2025-11-27 221041" src="https://github.com/user-attachments/assets/8ecf7c90-9014-496e-9b2b-5a8f7f65a23b" />
Created a transparency shader
Applied to background elements such as the trees and mountain
This is so we don’t have to create a mountain or tree model and then apply textures to it.

<img width="1919" height="1079" alt="Screenshot 2025-11-27 222231" src="https://github.com/user-attachments/assets/d080b44a-355c-477e-b202-c646f03e8d0c" />
We created a fog shader
We did this by taking a noise texture, altering its speed and other properties, and then overlay a texture that updates over time.
We then used this to create clouds overhead within our scene.
Source: https://aetuts.itch.io/volumetric-fog-unity-lwrpurp-shader-graph 


I, Connor McCarthy made scrolling texture, used the particle system and made water shader. 

Link for the fog:
[https://aetuts.itch.io/volumetric-fog-unity-lwrpurp-shader-graph](https://aetuts.itch.io/volumetric-fog-unity-lwrpurp-shader-graph)


