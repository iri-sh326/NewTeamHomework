using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class O_o1 : XRSocketInteractor
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
            Debug.Log("O1 true");
            socket.isCorrect_3 = true;
        }
        else
        {
            socket.isCorrect_3 = false;
        }

        base.OnSelectEntered(args);
    }
}
