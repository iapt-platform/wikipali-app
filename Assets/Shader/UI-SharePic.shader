// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "UI/SharePic"
{
    Properties
    {
		//[PerRendererData]
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

		_Test("test", Float) = (1,1,1,1)
		_SizeXY("SizeXY", Float) = (1,1,1,1)//x=layer���,y = layer�߶�
		_ImgSizeXY("ImgSizeXY", Float) = (1,1,1.5,1)//x=ͼƬ���,y = ͼƬ�߶�,z = ����,w = ����
		_StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
			float4 _ClipRect;
			float4 _Test;
			float4 _ImgSizeXY;
			float4 _SizeXY;
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                OUT.color = v.color * _Color;
                return OUT;
            }

			fixed4 frag(v2f IN) : SV_Target
			{
				//half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
				//half4 color = _Test;
				//half4 color = half4(_MainTex_TexelSize.z / 1000,_MainTex_TexelSize.w / 1000,0,1);
				half ratio1 = _SizeXY.x/ _SizeXY.y;
				half ratio = _ImgSizeXY.y / _SizeXY.y ;
				half2 xy = half2(IN.texcoord.x, IN.texcoord.y/ ratio1* _ImgSizeXY.z) * _ImgSizeXY.w;
				xy.x -= (_ImgSizeXY.w-1)*0.5;
				half4 color = tex2D(_MainTex, xy) * IN.color;
				//half4 color = tex2D(_MainTex,IN.worldPosition.xy*0.001*_Test.zw + _Test.xy) * IN.color;

              //  #ifdef UNITY_UI_CLIP_RECT
				//color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
				//color.a *= UnityGet2DClipping(IN.worldPosition.xy, _Test);
             //   #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                return color;
            }
        ENDCG
        }
    }
}
