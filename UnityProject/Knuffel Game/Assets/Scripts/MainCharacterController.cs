using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.U2D.Animation;

public class MainCharacterController : MonoBehaviour
{
    // Start is called before the first frame update

    public float WalkingSpeed = 0.1f;
    public SpriteRenderer SpriteRenderer;
    public SpriteLibraryAsset SpriteLibraryAsset;
    private EDirection lastDirection = EDirection.South;
    private const float DEADZONE = 0.1f;
    private float frameCounter = 0f;
    private const float FRAME_MULTIPLIER = 7;

    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        SpriteLibraryAsset = GetComponent<SpriteLibrary>().spriteLibraryAsset;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        var direction = lastDirection;
        var translationVector = new Vector2(x, y);
        if (translationVector.magnitude > DEADZONE)
        {
            gameObject.transform.Translate(x * WalkingSpeed, y * WalkingSpeed, 0);
            Vector2 fromVector2 = new Vector2(0, 1);
            Vector2 toVector2 = translationVector.normalized;

            float ang = Vector2.Angle(fromVector2, toVector2);
            Vector3 cross = Vector3.Cross(fromVector2, toVector2);

            if (cross.z > 0)
            {
                ang = 360 - ang;
            }

            if (ang < 45 || ang > 315)
            {
                direction = EDirection.North;
            }

            if (ang > 45 && ang < 135)
            {
                direction = EDirection.East;
            }

            if (ang > 135 && ang < 225)
            {
                direction = EDirection.South;
            }

            if (ang > 225 && ang < 315)
            {
                direction = EDirection.West;
            }
        }

        var category = SpriteLibraryAsset.GetCategoryNames()
            .First(x => x.Equals(direction.ToString(), StringComparison.InvariantCultureIgnoreCase));
        if (translationVector.magnitude < DEADZONE)
        {
            SpriteRenderer.sprite = SpriteLibraryAsset.GetSprite(category, "Standing");
        }
        else
        {
            frameCounter += translationVector.magnitude;
            if (frameCounter < FRAME_MULTIPLIER)
            {
                SpriteRenderer.sprite = SpriteLibraryAsset.GetSprite(category, "Walk 1");
            }
            else if (frameCounter < FRAME_MULTIPLIER * 2)
            {
                SpriteRenderer.sprite = SpriteLibraryAsset.GetSprite(category, "Standing");
            }
            else if (frameCounter < FRAME_MULTIPLIER * 3)
            {
                SpriteRenderer.sprite = SpriteLibraryAsset.GetSprite(category, "Walk 2");
            }
            else
            {
                SpriteRenderer.sprite = SpriteLibraryAsset.GetSprite(category, "Standing");
            }


            if (frameCounter > FRAME_MULTIPLIER * 4)
            {
                frameCounter = 0f;
            }
        }

        lastDirection = direction;
    }
}