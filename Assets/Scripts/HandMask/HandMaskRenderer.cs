﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera))]
public class HandMaskRenderer : MonoBehaviour
{
	[SerializeField]
	List<Renderer> _handMaskRenderers = new List<Renderer>();
	[SerializeField]
	Material _handMaskMaterial;

	RenderTexture _texture;
	public RenderTexture texture
	{
		get { return _texture; }
	}

	Camera _camera;

    void Start()
    {
		_camera = GetComponent<Camera>();
		var width = _camera.pixelWidth;
		var height = _camera.pixelHeight;

		var descriptor = new RenderTextureDescriptor(width, height, RenderTextureFormat.R8, 0);

		_texture = new RenderTexture(descriptor);

		var commandBuffer = new CommandBuffer();
		commandBuffer.SetRenderTarget(new RenderTargetIdentifier(_texture));
		commandBuffer.ClearRenderTarget(false, true, Color.black);
		for (int i = 0; i < _handMaskRenderers.Count; i++)
		{
			var renderer = _handMaskRenderers[i];
			commandBuffer.DrawRenderer(renderer, _handMaskMaterial);
		}
		commandBuffer.SetRenderTarget(null as RenderTexture);

		_camera.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, commandBuffer);

		var right = _camera.stereoTargetEye == StereoTargetEyeMask.Right;
		Shader.SetGlobalTexture(right ? "_HandMaskRight" : "_HandMaskLeft", _texture);
    }

    void Update()
    {

    }
}