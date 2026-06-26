interface IInteractable
{
    public void OnClick();
    public void OnPointerDown();
    public void OnPointerUp();

    public event System.Action OnFinishedInteraction;
}

