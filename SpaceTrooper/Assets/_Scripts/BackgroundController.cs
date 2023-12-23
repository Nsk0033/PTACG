using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour 
{
	private float width;
	private SpriteRenderer spriteRenderer;
	public float movingRate = 100f;

	protected void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		width = spriteRenderer.sprite.texture.width * transform.parent.localScale.x;
	}

	protected void Update () {
		Vector3 pos = transform.position;
		if (pos.x + width/2 < -Camera.main.pixelWidth / 2) {
			pos.x += width * 2;
		}
		pos.x -= movingRate * Time.deltaTime;
		transform.position = pos;
	}
}

