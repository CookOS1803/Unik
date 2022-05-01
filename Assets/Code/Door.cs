using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IInteractable
{
    void Use(Transform user);
}

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private float openTime = 2f;
    private NavMeshObstacle obstacle;
    private bool _isClosed = true;
    private bool isClosed
    {
        get => _isClosed;
        set
        {
            _isClosed = value;

            obstacle.enabled = !value;
        }
    }
    private readonly Quaternion openingRotation = Quaternion.Euler(0f, 90f, 0f);
    private readonly Quaternion closingRotation = Quaternion.Euler(0f, -90f, 0f);

    void Start()
    {
        obstacle = GetComponent<NavMeshObstacle>();
    }

    public void Use(Transform user)
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

    private void Open()
    {
        transform.forward = openingRotation * transform.forward;
        isClosed = false;
    }

    private void Close()
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
