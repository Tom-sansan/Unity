using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// NameSpecificSocket Class
/// </summary>
public class NameSpecificSocket : XRSocketInteractor
{
    #region Variables

    #region SerializeField

    /// <summary>
    /// Accept only IXRHoverInteractables with the name set here
    /// ここで設定された名前の IXRHoverInteractable だけ受け入れる
    /// </summary>
    [SerializeField]
    private string interactableName;

    #endregion SerializeField

    #endregion Variables

    #region Methods

    #region Public Methods

    /// <summary>
    /// true if the IXRHoverInteractable passed as argument is acceptable
    /// false if not accepted
    /// 引数で渡された IXRHoverInteractable を受け入れるなら true
    /// 受け入れないなら false を返す
    /// </summary>
    /// <param name="interactable"></param>
    /// <returns></returns>
    public override bool CanHover(IXRHoverInteractable interactable)
    {
        // If the name is not interactableNmae's name, it is not accepted even if the inheritor is OK
        // 継承元がOKしても interactableNmae の名前でないなら受け入れない
        return base.CanHover(interactable) && interactable.transform.name == interactableName;
    }
    /// <summary>
    /// true if the IXRSelectInteractable passed as argument is acceptable
    /// false if not accepted
    /// 引数で渡された IXRSelectInteractable を受け入れるなら true
    /// 受け入れないなら false を返す
    /// </summary>
    /// <param name="interactable"></param>
    /// <returns></returns>
    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        // If the name is not interactableNmae's name, it is not accepted even if the inheritor is OK
        // 継承元がOKしても interactableNmae の名前でないなら受け入れない
        return base.CanSelect(interactable) && interactable.transform.name == interactableName;
    }
    #endregion Public Methods

    #endregion Methods
}
