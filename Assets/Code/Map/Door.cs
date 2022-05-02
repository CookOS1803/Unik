using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] protected float openTime = 2f;
    protected NavMeshObstacle obstacle;
    protected bool _isClosed = true;
    protected bool isClosed
    {
        get => _isClosed;
        set
        {
            _isClosed = value;

            obstacle.enabled = !value;
        }
    }
    protected readonly Quaternion openingRotation = Quaternion.Euler(0f, 90f, 0f);
    protected readonly Quaternion closingRotation = Quaternion.Euler(0f, -90f, 0f);

    void Start()
    {
        obstacle = GetComponent<NavMeshObstacle>();
    }

    public virtual void Use(Transform user)
    {
        if (isClosed)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    protected void Open()
    {
        transform.forward = openingRotation * transform.forward;
        isClosed = false;
    }

    protected void Close()
    {
        transform.forward = closingRotation * transform.forward;
        isClosed = true;
    }

    public void OpenTemporarily()
    {
        if (isClosed)
        {
            Open();
            StartCoroutine(StayingOpen());
        }
    }

    IEnumerator StayingOpen()
    {
        float clock = 0f;

        while (clock < openTime && !isClosed)
        {
            clock += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        if (!isClosed)
        {
            Close();
        }
    }
}
