Shader "Hidden/ImageOverlay"
{
    Properties
    {
        _MainTex ("-", 2D) = "white" {}
        _OverlayTex ("-", 2D) = "black" {}
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    sampler2D _MainTex;
    sampler2D _OverlayTex;
    sampler2D _CameraGBufferTexture2;

    half4 frag(v2f_img i) : SV_Target
    {
        half4 src = tex2D(_MainTex, i.uv);

        float3 norm_o = tex2D(_CameraGBufferTexture2, i.uv).xyz * 2 - 1;
        half4 ov = tex2D(_OverlayTex, i.uv + norm_o.xz * float2(-0.09, 0.16) * 0.3);
        //ov = lerp(0, ov, pow(1.0 - Luminance(src), 30));
        ov.rgb = LinearToGammaSpace(ov.rgb);
        ov.rgb *= smoothstep(0.6, 1.2, 1.0 - Luminance(LinearToGammaSpace(src)));
        ov.rgb = GammaToLinearSpace(ov.rgb);
        return src + ov;
    }

    ENDCG

    SubShader
    {
        Pass
        {
            ZTest Always Cull Off ZWrite Off
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            ENDCG
        }
    }
}
