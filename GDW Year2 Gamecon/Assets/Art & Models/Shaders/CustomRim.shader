Shader "CompGraphics/CustomRim"
{
    Properties
    {
       _MainTex ("Texture", 2D) = "white" {}
       _OutlineColor ("Outline Color", Color) = (0,0,0,1)
	   _Outline ("Outline Width", Range (-0.001, 0.11)) = .00
       _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
       _RimColor ("Rim Color", Color) = (0, 0.5, 0.5, 1)
       _RimPower ("Rim Power", Range(0.5, 8.0)) = 3.0
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalRenderPipeline" "RenderType" = "Opaque" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 viewDirWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _RimColor;
                float _RimPower;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.normalWS = normalize(TransformObjectToWorldNormal(IN.normalOS));
                float3 worldPosWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.viewDirWS = normalize(GetCameraPositionWS() - worldPosWS);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half3 normalWS = normalize(IN.normalWS);
                half3 viewDirWS = normalize(IN.viewDirWS);
                half rimFactor = 100.0 - saturate(dot(viewDirWS, normalWS));
                half rimLighting = pow(rimFactor, _RimPower);
                half3 finalColor = _BaseColor.rgb + _RimColor.rgb * rimLighting;
                return half4(finalColor, _BaseColor.a);
            }

            ENDHLSL
        }
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalRenderPipeline" }
		Tags {"Queue"="Transparent" }

			
        Pass
        {	
        // Stencil operations
        Stencil
        {
            Ref 1  // Reference value to check against
            Comp Equal  // Only render where the stencil buffer is NOT equal to the reference
        }
        
        Name "OutlineColor" ZWrite Off
			
			
			HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
           
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            CBUFFER_START(UnityPerMaterial)
            float _Outline;
			half4 _OutlineColor;
			CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
				float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
            };

            // Vertex Shader (handles extrusion)
            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                // Extrude the vertex position along its normal
                float3 extrudedPosition = IN.positionOS.xyz + IN.normalOS * _Outline;

                // Transform object space to homogeneous clip space
                OUT.positionCS = TransformObjectToHClip(extrudedPosition);

                // Pass UV coordinates to fragment shader
                OUT.uv = IN.uv;

                // Calculate the world-space normal for lighting
                OUT.worldNormal = normalize(TransformObjectToWorldNormal(IN.normalOS));

				return OUT;
            }

            // Fragment Shader ()
            half4 frag(Varyings IN) : SV_Target
            {
                 //Set the color of the outline.
			    half4 finalColor = _OutlineColor;

                return finalColor; // Output final color
            }

            ENDHLSL
        }


        Pass
        {	Name "Texture Color" ZWrite on
			Tags { "LightMode" = "UniversalForward" }

			HLSLPROGRAM

            #pragma vertex vertTex
            #pragma fragment fragTex
           
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // Declare texture and extrusion amount
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            
            struct Attributes
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };
            struct Varyings
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormalT : TEXCOORD1;
            };

            // Vertex Shader (handles extrusion)
            Varyings vertTex(Attributes IN)
            {
                Varyings OUT;

                // Transform object space to homogeneous clip space
                OUT.position = TransformObjectToHClip(IN.vertex);

                // Pass UV coordinates to fragment shader
                OUT.uv = IN.uv;

                // Calculate the world-space normal for lighting
                OUT.worldNormalT = normalize(TransformObjectToWorldNormal(IN.normal));

                return OUT;
            }

            // Fragment Shader (handles texture sampling and lighting)
            half4 fragTex(Varyings IN) : SV_Target
            {
                // Sample the texture
                half4 albedo = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);

                // Get the main light direction and color
                Light mainLight = GetMainLight();
                half3 lightDir = normalize(mainLight.direction);
                half3 lightColor = mainLight.color;

                // Calculate diffuse lighting using Lambert's cosine law
                half NdotL = max(dot(IN.worldNormalT, lightDir), 0.0);
                // Calculate final color based on diffuse lighting and texture color
                half3 finalColortex = albedo.rgb * lightColor * NdotL;

                return half4(finalColortex, 1.0); // Output final color
            }

            ENDHLSL
        }
	}
}

