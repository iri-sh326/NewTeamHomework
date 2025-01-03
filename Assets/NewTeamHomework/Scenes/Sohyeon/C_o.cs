using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class C_o : XRSocketInteractor
{
    Socket socket;

    protected override void Start()
    {
        base.Start();
        socket = GetComponentInParent<Socket>();
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {

        if (args.interactableObject.transform.gameObject.CompareTag("C"))
        {
            Debug.Log("C true");
            socket.isCorrect_2 = true;
        }
        else
        {
            socket.isCorrect_2 = false;
        }

        base.OnSelectEntered(args);
    }
}
