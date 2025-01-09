using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
public class H2 : XRSocketInteractor
{
    Socket1 socket;

    protected override void Start()
    {
        base.Start();
        socket = GetComponentInParent<Socket1>();
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        
        if (args.interactableObject.transform.gameObject.CompareTag("H"))
        {
            Debug.Log("O3 true");
            socket.isCorrect_3 = true;
        }
        else
        {
            socket.isCorrect_3 = false;
        }

        base.OnSelectEntered(args);
    }
}
