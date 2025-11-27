Shader "CompGraphics/LeafFalling"
{
    Properties //properties input by the dev to the shader
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {} //a 2d texture, pulled from a spriterenderer
        _Color("Tint", Color) = (1,1,1,1) //a tint color
 
        _DistTex("Distortion Texture", 2D) = "grey" {} //distortion texture
        _DistTiling("Tiling Of Distortion Texture", float) = 1 //how many times to tile the distortion texture
        _DistortionMagnitude("Distortion Magnitude", float) = 0.025 //a float which sets the strength of the distortion
        _OverlayXSpeed("Overlay X Scroll Speed", float) = 1 //controls scroll speed of overlay texture on x axis
        _OverlayYSpeed("Overlay Y Scroll Speed", float) = 1 //controls scroll speed of overlay texture on y axis
        _OverlayBrightness("Overlay Brightness", float) = 1 //controls brightness of the overlay
    }
 
        SubShader //a shader block
        {
            Tags //properties of this shader block
            {
                "Queue" = "Transparent"  //render this in the transparent sector of the render queue
                "IgnoreProjector" = "True"  //ingore projectors
                "RenderType" = "Transparent"  //render this as a transparent object
                "PreviewType" = "Plane" //preview this on a plane
                "CanUseSpriteAtlas" = "True" //state that we can use sprite atlasses
            }
 
            Cull Off //do not cull
            Lighting Off //don't have lighting affect this object
            ZWrite Off //don't write to the depth buffer
            Blend One OneMinusSrcAlpha //use the One OneMinusSrcAlpha blend mode
 
            Pass //shader pass
            {
            CGPROGRAM //use the CG programming lanage
                #pragma vertex vert //vert is our vertex function
                #pragma fragment frag //frag is our fragment function
                #include "UnityCG.cginc" //use methods in UnityCG.cginc
 
                struct appdata_t //a struct to hold vertex data
                {
                    float4 vertex   : POSITION; //place to hold vertex position
                    float4 color    : COLOR; //place to hold vertex color
                    float2 texcoord : TEXCOORD0; //place to hold vertex UV coordinate
                };
 
                struct v2f //struct to convert vertex data into fragment (pixel) data
                {
                    float4 vertex   : SV_POSITION; //place to hold position
                    float4 color : COLOR; //place to hold color
                    float2 texcoord  : TEXCOORD0; //place to hold UV coordinate
                };
 
 
                //declare the various inputs
                float4 _Color;
                float _DistortionMagnitude;
                float _DistTiling;
                float _OverlayXSpeed;
                float _OverlayYSpeed;
                float _OverlayBrightness;
 
                v2f vert(appdata_t IN) //create a vertex struct
                {
                    v2f OUT; //make a v2f struct
                    OUT.vertex = UnityObjectToClipPos(IN.vertex); //set the vertex position
                    OUT.texcoord = IN.texcoord; //set the UV coord
                    OUT.color = IN.color * _Color; //set the color as the texture color * the tint
                    return OUT; //return the v2f
                }
 
                sampler2D _MainTex; //the main texture as a sampler2D
                sampler2D _DistTex; //sampler for the distortion texture
 
                float4 frag(v2f IN) : SV_Target //the fragment method (consider it a "for each pixel in inputted triangle" loop)
                {
                    float2 tiled_uv = IN.texcoord * _DistTiling; //get the tiled uv coordinate for the distortion texture
 
                    //we grab time so we have a continually changing value and multiply the x and y
                    //values by the scroll speed to increase/decrease the pace of movement
                    float2 scroll_factor = float2((_Time.x * _OverlayXSpeed), (_Time.x * _OverlayYSpeed));
 
                    //apply scrolling to the UV coordinate - I am doing this per-pixel so I can apply a 
                    //mask texture if needed. If not needed, can be done per-vertex
                    float2 scrolling_texcoord = tiled_uv + scroll_factor;
 
                    //sample the distortion texture at the scrolling texture coordinate
                    float4 untinted_overlay = tex2D(_DistTex, scrolling_texcoord);
 
                    //treat the g layer of the distortion texture as the rgb value to use as 
                    //the overlay tex and apply the vertex color on top of it
                    float4 overlay = (untinted_overlay.g * IN.color);
                    overlay.rgb *= _OverlayBrightness; //increase the overlay by the brightness value
                    overlay.rgb *= overlay.a; //apply the alpha value
 
                    //we get a distortion value by sampling the distortion texture (with the tiling value applied) 
                    //at the maintex UV coordinate and then adding our time-distance scroll value to it.
                    //Then we clamp it into the range between -1 and 1.
                    //We sample .rg (same as .xy) because that's where the offset values are encoded in the distortion texture
                    float2 dist = (tex2D(_DistTex, scrolling_texcoord).rg - 0.5) * 2;
                    dist *= _DistortionMagnitude; //increase the distortion by our magnitude value
 
                    //sample the uv coordinate of the main texture, adding in the distortion value
                    float4 maintx = tex2D(_MainTex, IN.texcoord + dist);
                    maintx.rgb *= maintx.a; //apply the alpha value
 
                    //multiply the overlay value by the alpha value of the main texture. This will
                    //result in overlay only shwoing on top of the maintex.
                    overlay *= maintx.a;
 
                    return overlay;
                }
            ENDCG
            }
        }
}