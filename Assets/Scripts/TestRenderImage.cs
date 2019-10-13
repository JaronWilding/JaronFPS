using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class TestRenderImage : MonoBehaviour
{
    //Variables
    public Shader curShader;
    [Range(0.0f, 1.0f)] public float grayScaleAmount = 1.0f;
    private Material curMaterial;

    public LayerMask excludeLayers = 0;

    private GameObject tmpCam = null;
    private Camera _camera;

    Material material
    {
        get
        {
            if(curMaterial == null)
            {
                curMaterial = new Material(curShader);
                curMaterial.hideFlags = HideFlags.HideAndDontSave;
                curMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
                curMaterial.renderQueue = 4;
            }
            return curMaterial;
        }
    }

    private void Start()
    {
        if(!SystemInfo.supportsImageEffects)
        {
            enabled = false;
            return;
        }
        if (!curShader && !curShader.isSupported)
            enabled = false;
    }

    private void OnRenderImage(RenderTexture sourceText, RenderTexture desText)
    {
        /*
        if(curShader != null)
        {
            material.SetFloat("_Luminosity", grayScaleAmount);
            Graphics.Blit(sourceText, desText, material);
        }
        else
        {
            Graphics.Blit(sourceText, desText);
        }*/

        material.SetFloat("_Luminosity", grayScaleAmount);
        Graphics.Blit(sourceText, desText, material);
        /*
        Camera cam = null;
        if (excludeLayers.value != 0) cam = GetTmpCam();

        if (cam && excludeLayers.value != 0)
        {
            cam.targetTexture = desText;
            cam.cullingMask = excludeLayers;
            cam.Render();
        }*/

    }

    private void Update()
    {
        grayScaleAmount = Mathf.Clamp01(grayScaleAmount);
    }

    private void OnDisable()
    {
        if (curMaterial)
            DestroyImmediate(curMaterial);
    }



    Camera GetTmpCam()
    {
        if (tmpCam == null)
        {
            if (_camera == null) _camera = GetComponent<Camera>();

            string name = "PostProcessCam";
            GameObject go = GameObject.Find(name);

            if (null == go) // couldn't find, recreate
            {
                tmpCam = new GameObject(name, typeof(Camera));
            }
            else
            {
                tmpCam = go;
            }
        }

        tmpCam.hideFlags = HideFlags.DontSave;
        tmpCam.transform.position = _camera.transform.position;
        tmpCam.transform.rotation = _camera.transform.rotation;
        tmpCam.transform.localScale = _camera.transform.localScale;
        tmpCam.GetComponent<Camera>().CopyFrom(_camera);

        tmpCam.GetComponent<Camera>().enabled = true;
        tmpCam.GetComponent<Camera>().depthTextureMode = DepthTextureMode.None;
        tmpCam.GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;

        return tmpCam.GetComponent<Camera>();
    }
}
