using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Posteffect : MonoBehaviour
{
    [SerializeField] private Volume volume;
    private Vignette vignette;
    private DepthOfField depthOfField;

    [SerializeField] private int vignette_speed;

    private bool Is_On_corutine;

    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out depthOfField);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine(Hit_Effect());
        }
    }

    IEnumerator Hit_Effect()
    {
        vignette.active = true;
        Is_On_corutine = true;
        depthOfField.active = true;

        vignette.intensity.value = 0;
        depthOfField.focalLength.value = 0;

        for (int i = 0; vignette.intensity.value <=0.3f; i++)
        {
            vignette.intensity.value += vignette_speed * Time.smoothDeltaTime;
            depthOfField.focalLength.value += 100 * vignette_speed *Time.smoothDeltaTime;
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(0.05f);

        for(float i = 0; vignette.intensity.value >= 0.01f; i++)
        {
            vignette.intensity.value -= vignette_speed * Time.smoothDeltaTime;
            depthOfField.focalLength.value -= 100 * vignette_speed * Time.smoothDeltaTime;
            yield return new WaitForSeconds(0.02f);
        }

        vignette.active = false;
        depthOfField.active = false;
        Is_On_corutine = false;
        yield return null;
    }
}
