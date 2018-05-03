// NextGenSprites (copyright) 2016 Ruben de la Torre, www.studio-delatorre.com
// Version 1.3.7

inline half3 SetHue(half3 aColor, half aHue)
{
	half angle = radians(aHue);
	half3 k = half3(0.57735, 0.57735, 0.57735);
	half cosAngle = cos(angle);
	return aColor * cosAngle + cross(k, aColor) * sin(angle) + k * dot(k, aColor) * (1 - cosAngle);
}

inline half3 ConvertToHSB(half3 inputColor, fixed4 hsbc)
{
	//Credits to Andrey Postelzhuk and Rodrigues rotation formula

	half _Hue = 360 * hsbc.r;
	half _Saturation = hsbc.g * 2;
	half _Brightness = hsbc.b * 2 - 1;
	half _Contrast = hsbc.a * 2;

	half3 outputColor = inputColor;
	outputColor.rgb = SetHue(outputColor.rgb, _Hue);
	outputColor.rgb = (outputColor.rgb - 0.5f) * (_Contrast)+0.5f;
	outputColor.rgb = outputColor.rgb + _Brightness;
	half3 intensity = dot(outputColor.rgb, half3(0.299, 0.587, 0.114));
	outputColor.rgb = lerp(intensity, outputColor.rgb, _Saturation);

	return outputColor;
}

inline half2 ScrollUV(float2 inputUV, half ScrollingX, half ScrollingY)
{
	return half2(inputUV.r + _Time.y * ScrollingX, inputUV.g + _Time.y * ScrollingY);
}

inline half3 BlendWithCanvas(half4 textureTarget, half3 textureCanvas, half3 tint)
{
	textureTarget.rgb *= tint;
	return lerp(textureTarget.rgb, textureCanvas, ((textureTarget.a - 1) * -1));
}

inline half3 DissolveBlending(half dissolveChannel, half borderWidth, half blendAmount, half3 glowColor, half glowStrenght)
{
	half remap = (blendAmount * -1) + 1;
	half extended = pow(((dissolveChannel * remap) * 5.0), 35.0);
	clip(clamp(extended, 0, 1) - 0.5);
	half DissolveCondition_A = step(borderWidth, extended);
	half DissolveCondition_B = step(extended, borderWidth);
	return glowColor * (lerp(DissolveCondition_B * 1, float(0), DissolveCondition_A * DissolveCondition_B) * glowStrenght);
}

inline half DissolveClip(half dissolveChannel, half blendAmount)
{
	half remap = (blendAmount * -1) + 1;
	half extended = pow(((dissolveChannel * remap) * 5.0), 35.0);
	return (clamp(extended, 0, 1) - 0.5);
}