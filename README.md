Computer Graphics Project Progression Assignment

Team Members: Connor McCarthy(100964926), Christopher Shin(100974007)

Link to Video: 

All Deliverables Below:

Part 1:
Created a 3D unity project where the main player controls a platypus and must make their way from the start point to the goal at the end of the level. While making their way there, other platypuses will shoot projectiles at you, if you are hit by one of these projectiles, you lose. There is also a section where you have to jump across platforms to get to the other side. If you fall into the void, you also lose. If you are able to avoid the other platypuses' projectiles and get to the goal, you win the game.
<img width="1915" height="1069" alt="Screenshot 2025-10-24 120057" src="https://github.com/user-attachments/assets/76660e30-d984-4d5c-a4f9-fbc63648dfd5" />

Part 2:
I, Connor McCarthy, created the three illumination shaders: simple diffuse lighting, diffuse lighting with ambient and simple specular.
I made use of the following code:
https://learn.ontariotechu.ca/courses/34225/pages/lambert-shader?module_item_id=800751 
https://learn.ontariotechu.ca/courses/34225/pages/ambient-shader?module_item_id=800760 
https://learn.ontariotechu.ca/courses/34225/pages/diffuse-ambient-specular?module_item_id=800862

I decided to attach each of these illuminations to the projectiles as the projectiles were the most previlant within the scene, so we wanted to make sure that they could be seen.

I, Christopher Shin, made each shader togglable using the 1,2,3,4 keys respectively. I additionally made a custom shader where I combined Simple Specualar and Rim Lighting and got some interesting effects where you can get some shininess as well as an outline color. I used the code for diffuse ambient specular and Rim Lighting Connor used to create something new and to experiment with what I could do. I mainly wanted to use this for some projectiles so that the rim lighting would make them pop out and the simple specular would enhance the look of them. 
<img width="1284" height="715" alt="image" src="https://github.com/user-attachments/assets/9248b5dc-a734-4d19-af62-9a800b7a63ed" />
<img width="1186" height="640" alt="image" src="https://github.com/user-attachments/assets/02f05964-18b1-4804-86d8-e1d79d41a035" />
<img width="1303" height="742" alt="image" src="https://github.com/user-attachments/assets/ba70ccc2-dc91-4515-96e8-4ed59ff3396c" />
<img width="1246" height="718" alt="image" src="https://github.com/user-attachments/assets/ef46588b-0a12-4d4c-ba78-154dc0cc5844" />




Part 3a:

I, Christopher Shin made the color grading for the project. I created 3 LUTs that can be toggled using the num keys, one for warm, cold, and custom. I used this website https://photoshop.adobe.com/ to create them because I knew it was an efficient and fast tool, as well as part3 used photoshop as an example in which I used a similar version. The code I used to implement them came from https://learn.ontariotechu.ca/courses/34225/pages/color-correction?module_item_id=802768. The reason why is because part3 specified to use the shader code that applies the LUTs to the screen which implies that we made one already and the one I made was also the same one from the canvas already. The modifications were changing the contribution property from 1 to 0.65 to make the lerping effect less harsh and more smooth. I also experiemented with the code so that the threshhold is added instead of multiplied, which creates a painterly or illustration of the game which can be used as a loading screen of some sort. Additionally, I used the camera code from here https://learn.ontariotechu.ca/courses/34225/files/5545568?module_item_id=802763 to apply a texture to a canvas image so that the camera can use Graphics.Blit to render the material.
<img width="787" height="440" alt="image" src="https://github.com/user-attachments/assets/0ae0aa1d-62f8-4b74-a21a-9b9d5e14e488" />
Cold
<img width="787" height="446" alt="image" src="https://github.com/user-attachments/assets/73179490-3ed4-4fac-b522-edf05e80acd4" />
Warm
<img width="787" height="440" alt="image" src="https://github.com/user-attachments/assets/ad090625-21ff-45c1-af19-ac41f9f4f04f" />
Custom

LUTs used for this project:
<img width="1024" height="32" alt="NeutralLUT" src="https://github.com/user-attachments/assets/e39814d4-c878-49b0-90e8-a12f57fd4964" />
<img width="512" height="15" alt="LUTcold" src="https://github.com/user-attachments/assets/6c35c5ff-c1b8-4c44-8b61-c7c4365808a5" />
<img width="512" height="16" alt="LUTwarm" src="https://github.com/user-attachments/assets/1d2adafc-7a36-41ce-81f9-e532ce02b479" />
<img width="512" height="17" alt="LUTcustom" src="https://github.com/user-attachments/assets/44634580-1af8-4d56-818c-e34b69904f03" />





Part 3b: 

I, Connor McCarthy, created the three shader implementations and applied them where I saw fit.

First, I started by creating the bump mapping shader. The main application of this shader was to the terrain within the level. When it comes to the actual code, I made the shader using the base bump mapping shader, and then modified it so that it can take a base color so that it conducts the bump mapping while also taking a base color into the final outcome. The reason I did this was to essential reduce the amount of textures we would need to import. For our terrain, using the modified bump mapping shader, I made grass, rock, and dirt. For the grass, I had a low bump amount for the grass texture and for the color I made it a bright green. For the rock, I had the bump amount to the max and then applied a grey to it to make it look like rock. Finally for the mud, I had the bump amount for the texture somewhat in the middle,and then applied a brown colour to it.
Texture used: https://assetstore.unity.com/packages/2d/textures-materials/glass/stylized-grass-texture-153153
<img width="1188" height="703" alt="image" src="https://github.com/user-attachments/assets/d788cffb-f234-4d8c-8372-d521e15d4249" />
<img width="1118" height="632" alt="image" src="https://github.com/user-attachments/assets/5433dd01-c6b8-4bc4-ac8a-bc5f0a850800" />
<img width="1258" height="656" alt="image" src="https://github.com/user-attachments/assets/3e8bfcdb-6852-4789-a93d-a6994d92f9fb" />
<img width="1112" height="626" alt="image" src="https://github.com/user-attachments/assets/9f182920-29f4-4293-ba7f-676826e2ae32" />




Next, I decided that for the platypuses, I wanted them to have a toon look, so I made the toon shader for the platypuses. When I applied them however, it came out looking weird. The bill, tail and feet of the platypus looked fine, however the body of the platypus looked off. I tried playing around with some of the math in half4 frag however i didn't like how they came out looking after the modifications, so I reverted it back to the base toon shader. I discovered the reason it was looking so weird, it was because the body was not smooth so it was showing all the individual surfaces on the model.
<img width="1263" height="686" alt="image" src="https://github.com/user-attachments/assets/4fda3aa2-a892-45be-acde-316bd5ba470d" />
<img width="954" height="597" alt="image" src="https://github.com/user-attachments/assets/bdb23c60-d235-46f7-a04f-c9b46de17a27" />


Finally, I created the rim lighting shaders and applied it some platforms that the player will need to cross over to get to the next section. I made a modification to the base code so that the rim factor appears brighter. I did this by taking the rim factor equation in the code and instead of subtracting saturate from 1.0, you are adding saturate to 1.0. I also made it so the base color is being multipled instead of added into the final color.
<img width="1204" height="654" alt="image" src="https://github.com/user-attachments/assets/af335dc8-5cbc-4e80-b62b-062eb5fdefe1" />
<img width="852" height="535" alt="image" src="https://github.com/user-attachments/assets/97a55bd5-a6f6-4c44-9af4-293f3f7bd837" />


