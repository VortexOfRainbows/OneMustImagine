using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(EdgeDetectionRenderer), PostProcessEvent.AfterStack, "Custom/EdgeDetection")]
public sealed class EdgeDetection : PostProcessEffectSettings
{
    [Range(0f, 5f), Tooltip("Edge detection effect intensity.")]
    public FloatParameter range = new FloatParameter { value = 1f };
}

public sealed class EdgeDetectionRenderer : PostProcessEffectRenderer<EdgeDetection>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/EdgeDetection"));
        sheet.properties.SetFloat("_Range", settings.range);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
