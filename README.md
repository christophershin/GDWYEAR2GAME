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

I, Christopher Shin, made each shader togglable using the 1,2,3 keys respectively.

Part 3a:

I, Christopher Shin made the color grading for the project. I created 3 LUTs that can be toggled using the num keys, one for warm, cold, and custom. I used this website https://photoshop.adobe.com/ to create them because I knew it was an efficient and fast tool, as well as part3 used photoshop as an example in which I used a similar version. The code I used to implement them came from https://learn.ontariotechu.ca/courses/34225/pages/color-correction?module_item_id=802768. The reason why is because part3 specified to use the shader code that applies the LUTs to the screen which implies that we made one already. The modifications were changing the contribution property from 1 to 0.65 to make the lerping effect less harsh and more smooth. 


Part 3b: 
