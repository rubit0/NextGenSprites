using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class BasicFPSCounter : MonoBehaviour
{
    private TextMesh _textMesh;
    private int _framesCounted;
    private IEnumerator _frameCounter;

    private readonly WaitForEndOfFrame _frameEnd = new WaitForEndOfFrame();
    private readonly WaitForSeconds _secondPassed = new WaitForSeconds(1f);

	void Start ()
	{
	    _textMesh = GetComponent<TextMesh>();
	    _frameCounter = CountFrames();
	    StartCoroutine(Counter());
	}

    public IEnumerator Counter()
    {
        while (true)
        {
            StartCoroutine(_frameCounter);
            yield return _secondPassed;
            StopCoroutine(_frameCounter);

            _textMesh.text = "FPS: " + _framesCounted.ToString();
            _framesCounted = 0;
        }
    }

    private IEnumerator CountFrames()
    {
        while (true)
        {
            yield return _frameEnd;
            ++_framesCounted;
        }
    }
}
