Computer Graphics Project Progression Assignment

Team Members: Connor McCarthy(100964926), Christopher Shin(100974007)

Link to Video: 

All Deliverables Below:

Part 1:
Created a 3D unity project where the main player controls a platypus and must make their way from the start point to the goal at the end of the level. While making their way there, other platypuses will shoot projectiles at you, if you are hit by one of these projectiles, you lose. There is also a section where you have to jump across platforms to get to the other side. If you fall into the void, you also lose. If you are able to avoid the other platypuses' projectiles and get to the goal, you win the game.

Part 2:
I, Connor McCarthy, created the three illumination shaders: simple diffuse lighting, diffuse lighting with ambient and simple specular.
I made use of the following code:
https://learn.ontariotechu.ca/courses/34225/pages/lambert-shader?module_item_id=800751 
https://learn.ontariotechu.ca/courses/34225/pages/ambient-shader?module_item_id=800760 
https://learn.ontariotechu.ca/courses/34225/pages/diffuse-ambient-specular?module_item_id=800862

I decided to attach each of these illuminations to the projectiles as the projectiles were the most previlant within the scene, so we wanted to make sure that they could be seen.

I, Christopher Shin, made each shader togglable using the 1,2,3,4 keys respectively. I additionally made a custom shader where I combined Simple Specualar and Rim Lighting and got some interesting effects where you can get some shininess as well as an outline color. I used the code for diffuse ambient specular and Rim Lighting Connor used to create something new and to experiment with what I could do. I mainly wanted to use this for some projectiles so that the rim lighting would make them pop out and the simple specular would enhance the look of them. 

Part 3a:

I, Christopher Shin made the color grading for the project. I created 3 LUTs that can be toggled using the num keys, one for warm, cold, and custom. I used this website https://photoshop.adobe.com/ to create them because I knew it was an efficient and fast tool, as well as part3 used photoshop as an example in which I used a similar version. The code I used to implement them came from https://learn.ontariotechu.ca/courses/34225/pages/color-correction?module_item_id=802768. The reason why is because part3 specified to use the shader code that applies the LUTs to the screen which implies that we made one already and the one I made was also the same one from the canvas already. The modifications were changing the contribution property from 1 to 0.65 to make the lerping effect less harsh and more smooth. 


Part 3b: 

I, Connor McCarthy, created the three shader implementations and applied them where I saw fit.

First, I started by creating the bump mapping shader. The main application of this shader was to the terrain within the level. When it comes to the actual code, I made the shader using the base bump mapping shader, and then modified it so that it can take a base color so that it conducts the bump mapping while also taking a base color into the final outcome. The reason I did this was to essential reduce the amount of textures we would need to import. For our terrain, using the modified bump mapping shader, I made grass, rock, and dirt. For the grass, I had a low bump amount for the grass texture and for the color I made it a bright green. For the rock, I had the bump amount to the max and then applied a grey to it to make it look like rock. Finally for the mud, I had the bump amount for the texture somewhat in the middle,and then applied a brown colour to it.
Texture used: https://assetstore.unity.com/packages/2d/textures-materials/glass/stylized-grass-texture-153153

Next, I decided that for the platypuses, I wanted them to have a toon look, so I made the toon shader for the platypuses. When I applied them however, it came out looking weird. The bill, tail and feet of the platypus looked fine, however the body of the platypus looked off. I tried playing around with some of the math in half4 frag however i didn't like how they came out looking after the modifications, so I reverted it back to the base toon shader. I discovered the reason it was looking so weird, it was because the body was not smooth so it was showing all the individual surfaces on the model.

Finally, I created the rim lighting shaders and applied it some platforms that the player will need to cross over to get to the next section. I made a modification to the base code so that the rim factor appears brighter. I did this by taking the rim factor equation in the code and instead of subtracting saturate from 1.0, you are adding saturate to 1.0. I also made it so the base color is being multipled instead of added into the final color.
