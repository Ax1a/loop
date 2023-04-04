using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractObject : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private InteractionPromptUI _interactionPromptUI;
    Outline outline;

    private readonly Collider[] _colliders = new Collider[3];
    [SerializeField] private int _numFound;

    private Interactable _interactable;

    public bool NearInteractable() {
        if (_numFound > 0) return true;

        return false;
    }

    public string GetObjectInteracter() {
        if (_colliders[0] != null) return _colliders[0].gameObject.name;

        return null;
    }

    void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);

        if (_numFound > 0 && DataManager.GetTutorialProgress() >= 3)
        {
            _interactable = _colliders[0].GetComponent<Interactable>();
            outline = _colliders[0].GetComponent<Outline>();

            if (_interactable != null)
            {
                if (outline != null) outline.enabled = true;
                if (!_interactionPromptUI.isDisplayed) _interactionPromptUI.SetUp(_interactable.InteractionPrompt);
                if (Input.GetKeyDown(InputManager.Instance.interact)) _interactable.Interact(this);
            }
        }
        else
        {
            if (_interactable != null) _interactable = null;
            if (outline != null) outline.enabled = false;
            if (_interactionPromptUI.isDisplayed) _interactionPromptUI.Close();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }
}
