
Shader "Custom/Cell-Shaded Default" {
    Properties {

    	[Header((Outline x Default))]

        [Space(20)][Header((Texture Color))]_Texture ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (0.8014706,0.8014706,0.8014706,1)
        [MaterialToggle] _Transparent ("Transparent", Float ) = 0
        [MaterialToggle] _TexturePatternStyle ("Texture Pattern Style", Float ) = 0

        [Space(20)][Header((NormalMap))][Normal]_NormalMap ("Normal Map", 2D) = "bump" {}
        _NormalIntensity ("Normal Intensity", Float ) = 1

        [Space(20)][Header((Color Adjustment))]_Saturation ("Saturation", Range(0, 2)) = 1
        _ReduceWhite ("Reduce White", Range(0, 1)) = 0
        _TextureWashout ("Texture Washout", Range(0, 1)) = 0

        [Space(20)][Header((Outline))]_OutlineWidth ("Outline Width", Range(0, 1)) = 0.003
        _OutlineNoiseIntensity ("Outline Noise Intensity", Range(0, 1)) = 0
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        [MaterialToggle] _DynamicNoiseOutline ("Dynamic Noise Outline", Float ) = 0

        [Space(20)][Header((Self Lit))]_SelfLitIntensity ("Self Lit Intensity", Range(0, 1)) = 0
        _SelfLitPower ("Self Lit Power", Float ) = 1
        _SelfLitColor ("Self Lit Color", Color) = (1,1,1,1)
        [Space(8)]_MaskSelfLit ("Mask Self Lit", 2D) = "white" {}

        [Space(20)][Header((Gloss))]_GlossIntensity ("Gloss Intensity", Float ) = 0
        _Glossiness ("Glossiness", Range(0, 1)) = 0.5
        _GlossColor ("Gloss Color", Color) = (1,1,1,1)
        [MaterialToggle] _MainTextureColorGloss ("Main Texture Color Gloss", Float ) = 0
        [MaterialToggle] _SoftGloss ("Soft Gloss", Float ) = 0
        [Space(8)]_GlossTextureIntensity ("Gloss Texture Intensity", Range(0, 1)) = 0
        _GlossTexture ("Gloss Texture", 2D) = "black" {}
        [MaterialToggle] _ShadowMaskGlossTexture ("Shadow Mask Gloss Texture", Float ) = 1
        [Space(8)]_GlossMask ("Gloss Mask", 2D) = "white" {}

        [Space(20)][Header((Shadow))]_ShadowIntensity ("Shadow Intensity", Range(0, 1)) = 1
        _ShadowColor ("Shadow Color", Color) = (0,0,0,1)
        [Space(8)][MaterialToggle] _MainTextureColorShadow ("Main Texture Color Shadow", Float ) = 0
        [MaterialToggle] _MainTextureColorSMixShadowColor ("Main Texture Color S (Mix Shadow Color)", Float ) = 0
        _MainTextureColorSSaturation ("Main Texture Color S (Saturation)", Range(0, 2)) = 1
        [Space(8)]_ReceiveShadowsIntensity ("Receive Shadows Intensity", Range(0, 1)) = 1
        [Space(8)]_ShadowPTextureIntensity ("Shadow PTexture Intensity", Range(0, 1)) = 0
        _ShadowPTexture ("Shadow PTexture", 2D) = "black" {}

        [Space(20)][Header((Self Shadow))]_SelfShadowIntensity ("Self Shadow Intensity", Range(0, 1)) = 1
        _SelfShadowSize ("Self Shadow Size", Range(0, 1)) = 0.56
        _SelfShadowHardness ("Self Shadow Hardness", Range(0, 1)) = 1
        [MaterialToggle] _SelfShadowatViewDirection ("Self Shadow at View Direction", Float ) = 0

        [Space(20)][Header((ShadowT))]_ShadowTIntensity ("ShadowT Intensity", Range(0, 1)) = 0
        _ShadowTTexture ("ShadowT Texture", 2D) = "white" {}
        _ShadowTSize ("ShadowT Size", Float ) = 0
        _ShadowTLimit ("ShadowT Limit", Float ) = 1.5
        [Space(8)][MaterialToggle] _MainTextureColorShadowT ("Main Texture Color ShadowT", Float ) = 0
        _MainTextureColorSTSaturation ("Main Texture Color ST (Saturation)", Range(0, 2)) = 1
        [Space(8)][MaterialToggle] _ShowShadowTOnLight ("Show ShadowT On Light", Float ) = 1
        [MaterialToggle] _ShowShadowTOnShadow ("Show ShadowT On Shadow", Float ) = 0
        [MaterialToggle] _ShowShadowTOnAmbientLight ("Show ShadowT On Ambient Light", Float ) = 0

        [Space(20)][Header((FReflection))]_FReflectionIntensity ("FReflection Intensity", Range(0, 1)) = 0
        _FReflection ("FReflection", 2D) = "black" {}
        [MaterialToggle] _FReflectionTextureBlend ("FReflection Texture Blend", Float ) = 0
        [Space(8)]_MaskFReflection ("Mask FReflection", 2D) = "white" {}

        [Space(20)][Header((Fresnel))]_FresnelIntensity ("Fresnel Intensity", Range(0, 1)) = 0
        _FresnelColor ("Fresnel Color", Color) = (1,1,1,1)
        _FresnelFill ("Fresnel Fill", Float ) = 1
        [MaterialToggle] _HardEdgeFresnel ("Hard Edge Fresnel", Float ) = 0
        [MaterialToggle] _FresnelOnLight ("Fresnel On Light", Float ) = 1
        [MaterialToggle] _FresnelOnShadow ("Fresnel On Shadow", Float ) = 1
        [MaterialToggle] _FresnelVisibleOnDarkAmbientLight ("Fresnel Visible On Dark/Ambient Light", Float ) = 0
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "Outline"
            Tags {
            }
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float4 _OutlineColor;
            uniform float _OutlineWidth;
            uniform fixed _Transparent;
            uniform float _OutlineNoiseIntensity;
            uniform fixed _DynamicNoiseOutline;
            uniform fixed _TexturePatternStyle;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float4 node_6085 = _Time + _TimeEditor;
                float node_5371_ang = node_6085.g;
                float node_5371_spd = 0.002;
                float node_5371_cos = cos(node_5371_spd*node_5371_ang);
                float node_5371_sin = sin(node_5371_spd*node_5371_ang);
                float2 node_5371_piv = float2(0.5,0.5);
                float2 node_5371 = (mul(o.uv0-node_5371_piv,float2x2( node_5371_cos, -node_5371_sin, node_5371_sin, node_5371_cos))+node_5371_piv);
                float2 _DynamicNoiseOutline_var = lerp( o.uv0, node_5371, _DynamicNoiseOutline );
                float2 node_7493_skew = _DynamicNoiseOutline_var + 0.2127+_DynamicNoiseOutline_var.x*0.3713*_DynamicNoiseOutline_var.y;
                float2 node_7493_rnd = 4.789*sin(489.123*(node_7493_skew));
                float node_7493 = frac(node_7493_rnd.x*node_7493_rnd.y*(1+node_7493_skew.x));
                o.pos = UnityObjectToClipPos( float4(v.vertex.xyz + v.normal*(lerp(1.0,node_7493,_OutlineNoiseIntensity)*_OutlineWidth),1) );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float2 _TexturePatternStyle_var = lerp( i.uv0, float2(i.screenPos.x*(_ScreenParams.r/_ScreenParams.g), i.screenPos.y).rg, _TexturePatternStyle );
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(_TexturePatternStyle_var, _Texture));
                clip(lerp( 1.0, _Texture_var.a, _Transparent ) - 0.5);
                return fixed4(_OutlineColor.rgb,0);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One OneMinusSrcAlpha
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float4 _Color;
            uniform float _Glossiness;
            uniform float4 _GlossColor;
            uniform float4 _ShadowColor;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _SelfShadowHardness;
            uniform float _NormalIntensity;
            uniform float _Saturation;
            uniform float _SelfShadowIntensity;
            uniform sampler2D _MaskSelfLit; uniform float4 _MaskSelfLit_ST;
            uniform sampler2D _FReflection; uniform float4 _FReflection_ST;
            uniform sampler2D _MaskFReflection; uniform float4 _MaskFReflection_ST;
            uniform float _FReflectionIntensity;
            uniform sampler2D _GlossMask; uniform float4 _GlossMask_ST;
            uniform fixed _SoftGloss;
            uniform float _SelfLitIntensity;
            uniform fixed _Transparent;
            uniform float _SelfShadowSize;
            uniform float _GlossIntensity;
            uniform float4 _SelfLitColor;
            uniform float _SelfLitPower;
            uniform float _ReduceWhite;
            uniform float _FresnelFill;
            uniform float4 _FresnelColor;
            uniform float _FresnelIntensity;
            uniform fixed _HardEdgeFresnel;
            uniform sampler2D _ShadowPTexture; uniform float4 _ShadowPTexture_ST;
            uniform float _ShadowPTextureIntensity;
            uniform fixed _SelfShadowatViewDirection;
            uniform float _ReceiveShadowsIntensity;
            uniform sampler2D _ShadowTTexture; uniform float4 _ShadowTTexture_ST;
            uniform float _ShadowTIntensity;
            uniform fixed _ShowShadowTOnLight;
            uniform fixed _ShowShadowTOnAmbientLight;
            uniform float _ShadowIntensity;
            uniform fixed _MainTextureColorShadow;
            uniform fixed _MainTextureColorShadowT;
            uniform fixed _FresnelOnLight;
            uniform fixed _FresnelOnShadow;
            uniform fixed _FresnelVisibleOnDarkAmbientLight;
            uniform float _TextureWashout;
            uniform sampler2D _GlossTexture; uniform float4 _GlossTexture_ST;
            uniform float _GlossTextureIntensity;
            uniform fixed _ShadowMaskGlossTexture;
            uniform fixed _MainTextureColorGloss;
            uniform fixed _FReflectionTextureBlend;
            uniform fixed _TexturePatternStyle;
            uniform fixed _ShowShadowTOnShadow;
            uniform float _MainTextureColorSSaturation;
            uniform fixed _MainTextureColorSMixShadowColor;
            uniform float _ShadowTSize;
            uniform float _ShadowTLimit;
            uniform float _MainTextureColorSTSaturation;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 screenPos : TEXCOORD5;
                LIGHTING_COORDS(6,7)
                UNITY_FOG_COORDS(8)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.screenPos = o.pos;
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap)));
                float3 normalLocal = lerp(float3(0,0,1),_NormalMap_var.rgb,_NormalIntensity);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float2 _TexturePatternStyle_var = lerp( i.uv0, float2(i.screenPos.x*(_ScreenParams.r/_ScreenParams.g), i.screenPos.y).rg, _TexturePatternStyle );
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(_TexturePatternStyle_var, _Texture));
                clip(lerp( 1.0, _Texture_var.a, _Transparent ) - 0.5);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
////// Emissive:
                float node_8889 = 0.0;
                float node_6003 = pow(1.0-max(0,dot(normalDirection, viewDirection)),exp2((1.0 - _FresnelFill)));
                float node_3815 = 0.0;
                float node_4621 = 1.0;
                float node_573 = 0.5*dot(lerp( lightDirection, viewDirection, _SelfShadowatViewDirection ),normalDirection)+0.5; // Selfshadow
                float node_9545 = smoothstep( lerp(0.3,0.899,_SelfShadowHardness), 0.9, saturate((node_573*lerp(2.8,0.79,_SelfShadowSize))) ); // Converted hard edge
                float node_2697 = (attenuation*node_9545);
                float3 node_137 = (((lerp( node_6003, smoothstep( 0.38, 0.4, node_6003 ), _HardEdgeFresnel )*lerp(lerp( node_3815, node_4621, _FresnelOnShadow ),lerp( node_3815, node_4621, _FresnelOnLight ),node_2697))*_FresnelColor.rgb)*_FresnelIntensity); // Fresnel
                float3 node_9875 = ( lerp(0.5,1.0,_TextureWashout) > 0.5 ? (1.0-(1.0-2.0*(lerp(0.5,1.0,_TextureWashout)-0.5))*(1.0-_Texture_var.rgb)) : (2.0*lerp(0.5,1.0,_TextureWashout)*_Texture_var.rgb) );
                float3 node_4607 = (node_9875*_Color.rgb);
                float2 node_4034 = (1.0 - (reflect(viewDirection,normalDirection).rg*0.5+0.5));
                float4 _FReflection_var = tex2D(_FReflection,TRANSFORM_TEX(node_4034, _FReflection));
                float3 node_2273 = lerp(node_4607,_FReflection_var.rgb,_FReflection_var.a);
                float4 _MaskFReflection_var = tex2D(_MaskFReflection,TRANSFORM_TEX(i.uv0, _MaskFReflection));
                float3 node_3766 = lerp(node_4607,lerp(lerp( node_2273, lerp(node_4607,node_2273,_FReflection_var.rgb), _FReflectionTextureBlend ),node_4607,(1.0 - _MaskFReflection_var.rgb)),_FReflectionIntensity); // FReflection
                float node_1736 = 1.0;
                float node_939 = 0.0;
                float3 node_2114 = (node_9875*0.9);
                float node_2503 = (1.0 - _MainTextureColorSTSaturation);
                float node_1159 = 1.0;
                float4 _ShadowTTexture_var = tex2D(_ShadowTTexture,TRANSFORM_TEX(i.uv0, _ShadowTTexture));
                float3 node_2724 = lerp(float3(node_1736,node_1736,node_1736),lerp(lerp(float3(node_939,node_939,node_939),lerp(node_2114,dot(node_2114,float3(0.3,0.59,0.11)),node_2503),_MainTextureColorShadowT),float3(node_1159,node_1159,node_1159),step((1.0 - saturate((node_573*_ShadowTSize))),(_ShadowTTexture_var.r*_ShadowTLimit))),lerp(0.0,lerp(1.0,10.0,_MainTextureColorShadowT),_ShadowTIntensity));
                float3 node_4224 = (node_3766*UNITY_LIGHTMODEL_AMBIENT.rgb*lerp( 1.0, node_2724, _ShowShadowTOnAmbientLight ));
                float4 _MaskSelfLit_var = tex2D(_MaskSelfLit,TRANSFORM_TEX(i.uv0, _MaskSelfLit));
                float node_5038 = (1.0 - _ReduceWhite);
                float node_3161 = (1.0 - _Saturation);
                float3 emissive = lerp(saturate(min((lerp(float3(node_8889,node_8889,node_8889),node_137,_FresnelVisibleOnDarkAmbientLight)+lerp(node_4224,lerp(node_4224,(_SelfLitColor.rgb*node_3766*_SelfLitPower),_MaskSelfLit_var.rgb),lerp(0.0,1.0,_SelfLitIntensity))),node_5038)),dot(saturate(min((lerp(float3(node_8889,node_8889,node_8889),node_137,_FresnelVisibleOnDarkAmbientLight)+lerp(node_4224,lerp(node_4224,(_SelfLitColor.rgb*node_3766*_SelfLitPower),_MaskSelfLit_var.rgb),lerp(0.0,1.0,_SelfLitIntensity))),node_5038)),float3(0.3,0.59,0.11)),node_3161);
                float node_7397 = (lerp(1.0,attenuation,_ReceiveShadowsIntensity)*lerp(1.0,node_9545,_SelfShadowIntensity));
                float node_2192 = lerp(1.0,node_7397,lerp(0.0,lerp(1.0,2.6,_MainTextureColorShadow),_ShadowIntensity));
                float node_6054 = (1.0 - node_2192); // Inverted to able to change color
                float node_6731 = 0.0;
                float3 node_2285 = saturate((1.0-((1.0-0.44)/node_9875)));
                float node_7988 = 0.0;
                float node_557 = 1.0;
                float4 _ShadowPTexture_var = tex2D(_ShadowPTexture,TRANSFORM_TEX(float2(i.screenPos.x*(_ScreenParams.r/_ScreenParams.g), i.screenPos.y).rg, _ShadowPTexture));
                float node_3933 = 0.0;
                float node_3448 = 1.0;
                float node_7581 = 0.0;
                float node_9744 = pow(max(0,dot(normalDirection,halfDirection)),exp2(lerp(-2,15,_Glossiness)));
                float4 _GlossTexture_var = tex2D(_GlossTexture,TRANSFORM_TEX(node_4034, _GlossTexture));
                float3 node_3708 = lerp(_GlossTexture_var.rgb,dot(_GlossTexture_var.rgb,float3(0.3,0.59,0.11)),0.0);
                float node_1292 = 0.0;
                float node_3179 = lerp(lerp(0.0,(lerp( smoothstep( 0.79, 0.9, (node_9744*3.0) ), node_9744, _SoftGloss )*1.0),node_2697),lerp( node_3708, lerp(float3(node_1292,node_1292,node_1292),node_3708,node_2697), _ShadowMaskGlossTexture ),_GlossTextureIntensity);
                float node_7359 = 0.0;
                float4 _GlossMask_var = tex2D(_GlossMask,TRANSFORM_TEX(i.uv0, _GlossMask));
                float node_2583 = 0.0;
                float node_8388_if_leA = step(_GlossIntensity,node_2583);
                float node_8388_if_leB = step(node_2583,_GlossIntensity);
                float node_8355 = 0.0;
                float3 finalColor = emissive + lerp(saturate(min(((((node_3766*(lerp(float3(node_2192,node_2192,node_2192),lerp(lerp((node_6054*_ShadowColor.rgb),lerp(lerp(float3(node_6731,node_6731,node_6731),lerp( node_2285, saturate(( _ShadowColor.rgb > 0.5 ? (1.0-(1.0-2.0*(_ShadowColor.rgb-0.5))*(1.0-node_2285)) : (2.0*_ShadowColor.rgb*node_2285) )), _MainTextureColorSMixShadowColor ),node_6054),dot(lerp(float3(node_6731,node_6731,node_6731),lerp( node_2285, saturate(( _ShadowColor.rgb > 0.5 ? (1.0-(1.0-2.0*(_ShadowColor.rgb-0.5))*(1.0-node_2285)) : (2.0*_ShadowColor.rgb*node_2285) )), _MainTextureColorSMixShadowColor ),node_6054),float3(0.3,0.59,0.11)),(1.0 - _MainTextureColorSSaturation)),_MainTextureColorShadow),lerp(float3(node_7988,node_7988,node_7988),(lerp(lerp(float3(node_557,node_557,node_557),_ShadowPTexture_var.rgb,_ShadowPTexture_var.a),float3(node_3933,node_3933,node_3933),node_7397)*lerp(0.54,1.65,_MainTextureColorShadow)),_ShadowIntensity),_ShadowPTextureIntensity),0.65)*2.86))*lerp(lerp( node_3448, node_2724, _ShowShadowTOnShadow ),lerp( node_3448, node_2724, _ShowShadowTOnLight ),node_7397))+(lerp(float3(node_7581,node_7581,node_7581),lerp( lerp(float3(node_3179,node_3179,node_3179),(node_3179*_GlossColor.rgb),2.22), lerp(float3(node_7359,node_7359,node_7359),node_9875,node_3179), _MainTextureColorGloss ),_GlossMask_var.rgb)*lerp((node_8388_if_leA*node_2583)+(node_8388_if_leB*_GlossIntensity),_GlossIntensity,node_8388_if_leA*node_8388_if_leB))+lerp(node_137,float3(node_8355,node_8355,node_8355),_FresnelVisibleOnDarkAmbientLight))*_LightColor0.rgb),node_5038)),dot(saturate(min(((((node_3766*(lerp(float3(node_2192,node_2192,node_2192),lerp(lerp((node_6054*_ShadowColor.rgb),lerp(lerp(float3(node_6731,node_6731,node_6731),lerp( node_2285, saturate(( _ShadowColor.rgb > 0.5 ? (1.0-(1.0-2.0*(_ShadowColor.rgb-0.5))*(1.0-node_2285)) : (2.0*_ShadowColor.rgb*node_2285) )), _MainTextureColorSMixShadowColor ),node_6054),dot(lerp(float3(node_6731,node_6731,node_6731),lerp( node_2285, saturate(( _ShadowColor.rgb > 0.5 ? (1.0-(1.0-2.0*(_ShadowColor.rgb-0.5))*(1.0-node_2285)) : (2.0*_ShadowColor.rgb*node_2285) )), _MainTextureColorSMixShadowColor ),node_6054),float3(0.3,0.59,0.11)),(1.0 - _MainTextureColorSSaturation)),_MainTextureColorShadow),lerp(float3(node_7988,node_7988,node_7988),(lerp(lerp(float3(node_557,node_557,node_557),_ShadowPTexture_var.rgb,_ShadowPTexture_var.a),float3(node_3933,node_3933,node_3933),node_7397)*lerp(0.54,1.65,_MainTextureColorShadow)),_ShadowIntensity),_ShadowPTextureIntensity),0.65)*2.86))*lerp(lerp( node_3448, node_2724, _ShowShadowTOnShadow ),lerp( node_3448, node_2724, _ShowShadowTOnLight ),node_7397))+(lerp(float3(node_7581,node_7581,node_7581),lerp( lerp(float3(node_3179,node_3179,node_3179),(node_3179*_GlossColor.rgb),2.22), lerp(float3(node_7359,node_7359,node_7359),node_9875,node_3179), _MainTextureColorGloss ),_GlossMask_var.rgb)*lerp((node_8388_if_leA*node_2583)+(node_8388_if_leB*_GlossIntensity),_GlossIntensity,node_8388_if_leA*node_8388_if_leB))+lerp(node_137,float3(node_8355,node_8355,node_8355),_FresnelVisibleOnDarkAmbientLight))*_LightColor0.rgb),node_5038)),float3(0.3,0.59,0.11)),node_3161);
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float4 _Color;
            uniform float _Glossiness;
            uniform float4 _GlossColor;
            uniform float4 _ShadowColor;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _SelfShadowHardness;
            uniform float _NormalIntensity;
            uniform float _Saturation;
            uniform float _SelfShadowIntensity;
            uniform sampler2D _MaskSelfLit; uniform float4 _MaskSelfLit_ST;
            uniform sampler2D _FReflection; uniform float4 _FReflection_ST;
            uniform sampler2D _MaskFReflection; uniform float4 _MaskFReflection_ST;
            uniform float _FReflectionIntensity;
            uniform sampler2D _GlossMask; uniform float4 _GlossMask_ST;
            uniform fixed _SoftGloss;
            uniform float _SelfLitIntensity;
            uniform fixed _Transparent;
            uniform float _SelfShadowSize;
            uniform float _GlossIntensity;
            uniform float4 _SelfLitColor;
            uniform float _SelfLitPower;
            uniform float _ReduceWhite;
            uniform float _FresnelFill;
            uniform float4 _FresnelColor;
            uniform float _FresnelIntensity;
            uniform fixed _HardEdgeFresnel;
            uniform sampler2D _ShadowPTexture; uniform float4 _ShadowPTexture_ST;
            uniform float _ShadowPTextureIntensity;
            uniform fixed _SelfShadowatViewDirection;
            uniform float _ReceiveShadowsIntensity;
            uniform sampler2D _ShadowTTexture; uniform float4 _ShadowTTexture_ST;
            uniform float _ShadowTIntensity;
            uniform fixed _ShowShadowTOnLight;
            uniform fixed _ShowShadowTOnAmbientLight;
            uniform float _ShadowIntensity;
            uniform fixed _MainTextureColorShadow;
            uniform fixed _MainTextureColorShadowT;
            uniform fixed _FresnelOnLight;
            uniform fixed _FresnelOnShadow;
            uniform fixed _FresnelVisibleOnDarkAmbientLight;
            uniform float _TextureWashout;
            uniform sampler2D _GlossTexture; uniform float4 _GlossTexture_ST;
            uniform float _GlossTextureIntensity;
            uniform fixed _ShadowMaskGlossTexture;
            uniform fixed _MainTextureColorGloss;
            uniform fixed _FReflectionTextureBlend;
            uniform fixed _TexturePatternStyle;
            uniform fixed _ShowShadowTOnShadow;
            uniform float _MainTextureColorSSaturation;
            uniform fixed _MainTextureColorSMixShadowColor;
            uniform float _ShadowTSize;
            uniform float _ShadowTLimit;
            uniform float _MainTextureColorSTSaturation;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 screenPos : TEXCOORD5;
                LIGHTING_COORDS(6,7)
                UNITY_FOG_COORDS(8)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.screenPos = o.pos;
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap)));
                float3 normalLocal = lerp(float3(0,0,1),_NormalMap_var.rgb,_NormalIntensity);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float2 _TexturePatternStyle_var = lerp( i.uv0, float2(i.screenPos.x*(_ScreenParams.r/_ScreenParams.g), i.screenPos.y).rg, _TexturePatternStyle );
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(_TexturePatternStyle_var, _Texture));
                clip(lerp( 1.0, _Texture_var.a, _Transparent ) - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 node_9875 = ( lerp(0.5,1.0,_TextureWashout) > 0.5 ? (1.0-(1.0-2.0*(lerp(0.5,1.0,_TextureWashout)-0.5))*(1.0-_Texture_var.rgb)) : (2.0*lerp(0.5,1.0,_TextureWashout)*_Texture_var.rgb) );
                float3 node_4607 = (node_9875*_Color.rgb);
                float2 node_4034 = (1.0 - (reflect(viewDirection,normalDirection).rg*0.5+0.5));
                float4 _FReflection_var = tex2D(_FReflection,TRANSFORM_TEX(node_4034, _FReflection));
                float3 node_2273 = lerp(node_4607,_FReflection_var.rgb,_FReflection_var.a);
                float4 _MaskFReflection_var = tex2D(_MaskFReflection,TRANSFORM_TEX(i.uv0, _MaskFReflection));
                float3 node_3766 = lerp(node_4607,lerp(lerp( node_2273, lerp(node_4607,node_2273,_FReflection_var.rgb), _FReflectionTextureBlend ),node_4607,(1.0 - _MaskFReflection_var.rgb)),_FReflectionIntensity); // FReflection
                float node_573 = 0.5*dot(lerp( lightDirection, viewDirection, _SelfShadowatViewDirection ),normalDirection)+0.5; // Selfshadow
                float node_9545 = smoothstep( lerp(0.3,0.899,_SelfShadowHardness), 0.9, saturate((node_573*lerp(2.8,0.79,_SelfShadowSize))) ); // Converted hard edge
                float node_7397 = (lerp(1.0,attenuation,_ReceiveShadowsIntensity)*lerp(1.0,node_9545,_SelfShadowIntensity));
                float node_2192 = lerp(1.0,node_7397,lerp(0.0,lerp(1.0,2.6,_MainTextureColorShadow),_ShadowIntensity));
                float node_6054 = (1.0 - node_2192); // Inverted to able to change color
                float node_6731 = 0.0;
                float3 node_2285 = saturate((1.0-((1.0-0.44)/node_9875)));
                float node_7988 = 0.0;
                float node_557 = 1.0;
                float4 _ShadowPTexture_var = tex2D(_ShadowPTexture,TRANSFORM_TEX(float2(i.screenPos.x*(_ScreenParams.r/_ScreenParams.g), i.screenPos.y).rg, _ShadowPTexture));
                float node_3933 = 0.0;
                float node_3448 = 1.0;
                float node_1736 = 1.0;
                float node_939 = 0.0;
                float3 node_2114 = (node_9875*0.9);
                float node_2503 = (1.0 - _MainTextureColorSTSaturation);
                float node_1159 = 1.0;
                float4 _ShadowTTexture_var = tex2D(_ShadowTTexture,TRANSFORM_TEX(i.uv0, _ShadowTTexture));
                float3 node_2724 = lerp(float3(node_1736,node_1736,node_1736),lerp(lerp(float3(node_939,node_939,node_939),lerp(node_2114,dot(node_2114,float3(0.3,0.59,0.11)),node_2503),_MainTextureColorShadowT),float3(node_1159,node_1159,node_1159),step((1.0 - saturate((node_573*_ShadowTSize))),(_ShadowTTexture_var.r*_ShadowTLimit))),lerp(0.0,lerp(1.0,10.0,_MainTextureColorShadowT),_ShadowTIntensity));
                float node_7581 = 0.0;
                float node_9744 = pow(max(0,dot(normalDirection,halfDirection)),exp2(lerp(-2,15,_Glossiness)));
                float node_2697 = (attenuation*node_9545);
                float4 _GlossTexture_var = tex2D(_GlossTexture,TRANSFORM_TEX(node_4034, _GlossTexture));
                float3 node_3708 = lerp(_GlossTexture_var.rgb,dot(_GlossTexture_var.rgb,float3(0.3,0.59,0.11)),0.0);
                float node_1292 = 0.0;
                float node_3179 = lerp(lerp(0.0,(lerp( smoothstep( 0.79, 0.9, (node_9744*3.0) ), node_9744, _SoftGloss )*1.0),node_2697),lerp( node_3708, lerp(float3(node_1292,node_1292,node_1292),node_3708,node_2697), _ShadowMaskGlossTexture ),_GlossTextureIntensity);
                float node_7359 = 0.0;
                float4 _GlossMask_var = tex2D(_GlossMask,TRANSFORM_TEX(i.uv0, _GlossMask));
                float node_2583 = 0.0;
                float node_8388_if_leA = step(_GlossIntensity,node_2583);
                float node_8388_if_leB = step(node_2583,_GlossIntensity);
                float node_6003 = pow(1.0-max(0,dot(normalDirection, viewDirection)),exp2((1.0 - _FresnelFill)));
                float node_3815 = 0.0;
                float node_4621 = 1.0;
                float3 node_137 = (((lerp( node_6003, smoothstep( 0.38, 0.4, node_6003 ), _HardEdgeFresnel )*lerp(lerp( node_3815, node_4621, _FresnelOnShadow ),lerp( node_3815, node_4621, _FresnelOnLight ),node_2697))*_FresnelColor.rgb)*_FresnelIntensity); // Fresnel
                float node_8355 = 0.0;
                float node_5038 = (1.0 - _ReduceWhite);
                float node_3161 = (1.0 - _Saturation);
                float3 finalColor = lerp(saturate(min(((((node_3766*(lerp(float3(node_2192,node_2192,node_2192),lerp(lerp((node_6054*_ShadowColor.rgb),lerp(lerp(float3(node_6731,node_6731,node_6731),lerp( node_2285, saturate(( _ShadowColor.rgb > 0.5 ? (1.0-(1.0-2.0*(_ShadowColor.rgb-0.5))*(1.0-node_2285)) : (2.0*_ShadowColor.rgb*node_2285) )), _MainTextureColorSMixShadowColor ),node_6054),dot(lerp(float3(node_6731,node_6731,node_6731),lerp( node_2285, saturate(( _ShadowColor.rgb > 0.5 ? (1.0-(1.0-2.0*(_ShadowColor.rgb-0.5))*(1.0-node_2285)) : (2.0*_ShadowColor.rgb*node_2285) )), _MainTextureColorSMixShadowColor ),node_6054),float3(0.3,0.59,0.11)),(1.0 - _MainTextureColorSSaturation)),_MainTextureColorShadow),lerp(float3(node_7988,node_7988,node_7988),(lerp(lerp(float3(node_557,node_557,node_557),_ShadowPTexture_var.rgb,_ShadowPTexture_var.a),float3(node_3933,node_3933,node_3933),node_7397)*lerp(0.54,1.65,_MainTextureColorShadow)),_ShadowIntensity),_ShadowPTextureIntensity),0.65)*2.86))*lerp(lerp( node_3448, node_2724, _ShowShadowTOnShadow ),lerp( node_3448, node_2724, _ShowShadowTOnLight ),node_7397))+(lerp(float3(node_7581,node_7581,node_7581),lerp( lerp(float3(node_3179,node_3179,node_3179),(node_3179*_GlossColor.rgb),2.22), lerp(float3(node_7359,node_7359,node_7359),node_9875,node_3179), _MainTextureColorGloss ),_GlossMask_var.rgb)*lerp((node_8388_if_leA*node_2583)+(node_8388_if_leB*_GlossIntensity),_GlossIntensity,node_8388_if_leA*node_8388_if_leB))+lerp(node_137,float3(node_8355,node_8355,node_8355),_FresnelVisibleOnDarkAmbientLight))*_LightColor0.rgb),node_5038)),dot(saturate(min(((((node_3766*(lerp(float3(node_2192,node_2192,node_2192),lerp(lerp((node_6054*_ShadowColor.rgb),lerp(lerp(float3(node_6731,node_6731,node_6731),lerp( node_2285, saturate(( _ShadowColor.rgb > 0.5 ? (1.0-(1.0-2.0*(_ShadowColor.rgb-0.5))*(1.0-node_2285)) : (2.0*_ShadowColor.rgb*node_2285) )), _MainTextureColorSMixShadowColor ),node_6054),dot(lerp(float3(node_6731,node_6731,node_6731),lerp( node_2285, saturate(( _ShadowColor.rgb > 0.5 ? (1.0-(1.0-2.0*(_ShadowColor.rgb-0.5))*(1.0-node_2285)) : (2.0*_ShadowColor.rgb*node_2285) )), _MainTextureColorSMixShadowColor ),node_6054),float3(0.3,0.59,0.11)),(1.0 - _MainTextureColorSSaturation)),_MainTextureColorShadow),lerp(float3(node_7988,node_7988,node_7988),(lerp(lerp(float3(node_557,node_557,node_557),_ShadowPTexture_var.rgb,_ShadowPTexture_var.a),float3(node_3933,node_3933,node_3933),node_7397)*lerp(0.54,1.65,_MainTextureColorShadow)),_ShadowIntensity),_ShadowPTextureIntensity),0.65)*2.86))*lerp(lerp( node_3448, node_2724, _ShowShadowTOnShadow ),lerp( node_3448, node_2724, _ShowShadowTOnLight ),node_7397))+(lerp(float3(node_7581,node_7581,node_7581),lerp( lerp(float3(node_3179,node_3179,node_3179),(node_3179*_GlossColor.rgb),2.22), lerp(float3(node_7359,node_7359,node_7359),node_9875,node_3179), _MainTextureColorGloss ),_GlossMask_var.rgb)*lerp((node_8388_if_leA*node_2583)+(node_8388_if_leB*_GlossIntensity),_GlossIntensity,node_8388_if_leA*node_8388_if_leB))+lerp(node_137,float3(node_8355,node_8355,node_8355),_FresnelVisibleOnDarkAmbientLight))*_LightColor0.rgb),node_5038)),float3(0.3,0.59,0.11)),node_3161);
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform fixed _Transparent;
            uniform fixed _TexturePatternStyle;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float4 screenPos : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                o.screenPos = o.pos;
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float2 _TexturePatternStyle_var = lerp( i.uv0, float2(i.screenPos.x*(_ScreenParams.r/_ScreenParams.g), i.screenPos.y).rg, _TexturePatternStyle );
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(_TexturePatternStyle_var, _Texture));
                clip(lerp( 1.0, _Texture_var.a, _Transparent ) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
