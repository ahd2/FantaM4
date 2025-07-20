using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostOutline : ScriptableRendererFeature
{
    public Material outlineMaterial;
    private PostOutlinePass postOutlinePass;

    [System.Serializable]
    public class PostOutlineSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
        public Material outlineMaterial;
    }

    public PostOutlineSettings settings = new PostOutlineSettings();

    public override void Create()
    {
        if (settings.outlineMaterial == null)
        {
            Debug.LogError("Outline material is not assigned.");
            return;
        }

        postOutlinePass = new PostOutlinePass(settings.renderPassEvent, settings.outlineMaterial);
        name = "Post Outline";
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(postOutlinePass);
    }
}

internal class PostOutlinePass : ScriptableRenderPass
{
    private static readonly int MainTexId = Shader.PropertyToID("_MainTex");
    private static readonly string ProfilerTag = "Post Outline";

    private RenderTargetIdentifier source;
    private Material outlineMaterial;

    public PostOutlinePass(RenderPassEvent renderPassEvent, Material outlineMaterial)
    {
        this.renderPassEvent = renderPassEvent;
        this.outlineMaterial = outlineMaterial;
    }

    public void Setup(ref RenderingData renderingData)
    {
        source = renderingData.cameraData.renderer.cameraColorTargetHandle;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get();
        using (new ProfilingScope(cmd, new ProfilingSampler(ProfilerTag)))
        {
            RenderTargetIdentifier tempTarget = renderingData.cameraData.renderer.cameraColorTargetHandle;
            Blit(cmd, source, tempTarget, outlineMaterial);
        }
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}