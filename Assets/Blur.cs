using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(CameraBlurRenderer), PostProcessEvent.AfterStack, "Custom/CameraBlur")]
public sealed class CameraBlur : PostProcessEffectSettings
{
    [Range(1, 10), Tooltip("Blur effect intensity.")]
    public IntParameter range = new IntParameter { value = 1 };
    [Range(1, 10), Tooltip("Bake intensity.")]
    public FloatParameter bake = new FloatParameter { value = 1 };
}

public sealed class CameraBlurRenderer : PostProcessEffectRenderer<CameraBlur>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/CameraBlur"));
        sheet.properties.SetInt("_Range", settings.range);
        sheet.properties.SetFloat("_Bake", settings.bake);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
