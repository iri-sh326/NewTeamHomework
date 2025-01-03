using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class O_o : XRSocketInteractor
{
    Socket socket;

    protected override void Start()
    {
        base.Start();
        socket = GetComponentInParent<Socket>();
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {

        if (args.interactableObject.transform.gameObject.CompareTag("O"))
        {
            Debug.Log("O2 true");
            socket.isCorrect_1 = true;
        }
        else
        {
            socket.isCorrect_1 = false;
        }

        base.OnSelectEntered(args);
    }
}
