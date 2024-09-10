//UNITY_SHADER_NO_UPGRADE
#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED



void CRTDistortion_float(float2 texCoord, float distortion, out float2 output)
{
    float2 uv = texCoord * 2.0 -1.0;
    float angle = atan2(uv.y, uv.x);
    float radius = length(uv);

    //appling curvature
    radius = pow(radius, distortion);
    
    //Reverting to cartesian
    uv.x = radius * cos(angle);
    uv.y = radius * sin(angle);

    //remaping to [0,1]
    output = uv * 0.5 + 0.5;
}

void CRTViginette_float(float2 uv, float viginetteWidth, out float2 output){
    uv = uv * 2.0f - 1.0f;
    float2 viginette = viginetteWidth / _ScreenParams.xy;
    viginette = smoothstep(0.0f, viginette, 1.0f - abs(uv));
    viginette = saturate(viginette);
    output = viginette;
}

#endif //MYHLSLINCLUDE_INCLUDED