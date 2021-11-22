using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
	public static CoreEngine PolyFold;

    public GameObject StepTimeText;

    public float Hertz;

	private float Accumulator;
    private float FrameStart;

    private float prevTime;

    void Start()
    {
        //Application.targetFrameRate = 60;

        PolyFold = new CoreEngine(1.0f / Hertz, 10);

        PolyFoldObject[] objects = FindObjectsOfType(typeof(PolyFoldObject)) as PolyFoldObject[];

        foreach(PolyFoldObject element in objects)
        {
            PolyFold.add(new Circle(Mathf.Max(element.gameObject.transform.localScale.x, element.gameObject.transform.localScale.y)/2), element);
        }

        for (float i = -10.5f; i < 10.5f; i+=3)
        {
            //Body b = PolyFold.add(new Circle(2.0f), i, Random.Range(-3, -5));
            //b.SetStatic();
        }
        /*Body b = PolyFold.add(new Circle(2.0f), 1, 5);

        Body b1 = PolyFold.add(new Circle(2.0f), 0, -5);
        b1.SetStatic();*/

        FrameStart = Time.time;
        Accumulator = 0f;
    }

    void Update()
    {
        float currentTime = Time.time;

        Accumulator += currentTime - FrameStart;

        FrameStart = currentTime;

        if(Accumulator > 0.2f)
        {
            Accumulator = 0.2f;
        }

        if(Accumulator > PolyFold.Dt)
        {
            StepTimeText.GetComponent<UnityEngine.UI.Text>().text = ((Time.time - prevTime)*1000).ToString("F2");
            PolyFold.step();
            //Debug.Log("Physics" + Time.time);
            Accumulator -= PolyFold.Dt;
            prevTime = Time.time;
        }

        float alpha = Accumulator / PolyFold.Dt;

        PolyFold.Render(alpha);

        PolyFold.Dt = 1.0f / Hertz;
    }
}
