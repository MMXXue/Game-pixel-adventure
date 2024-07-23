using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.LucidEditor;

public class WallChest : MonoBehaviour
{
    [FoldoutGroup("Reference")]
    public Animator animator;

    [FoldoutGroup("Runtime"), ShowInInspector, DisableInEditMode]
    public bool IsOpened
    {
        get { return isOpened; }
        set
        {
            isOpened = value;
            animator.SetBool("IsOpened", isOpened);
        }
    }
    private bool isOpened;

    [FoldoutGroup("Runtime"), Button("Open"), HorizontalGroup("Runtime/Button")]
    public void Open()
    {
        IsOpened = true;

        WallMovement player = FindObjectOfType<WallMovement>();
        if (player != null)
        {
            player.UnlockWallJump();
        }
    }

    [FoldoutGroup("Runtime"), Button("Close"), HorizontalGroup("Runtime/Button")]
    public void Close()
    {
        IsOpened = false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Open();
        }
    }
}
