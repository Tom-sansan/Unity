/// <summary>
/// Title Scene Class
/// </summary>
public class TitleScene : SceneBase
{
    #region Variables

    #region SerializeField

    /// <summary>
    /// View list
    /// </summary>
    //[SerializeField]
    //List<GameObject> viewList = new List<GameObject>();
    
    #endregion SerializeField

    #endregion Variables

    #region Methods

    #region Unity Methods
    protected override void Start()
    {
        base.Start();
        //Test_ChangeView(0);
        //StartCoroutine(ChangeViewWaitForSeconds());
    }

    #endregion Unity Methods

    #region Public Methods

    //public void Test_ChangeView(int index)
    //{
    //    foreach (var view in viewList)
    //    {
    //        // Show those with the same index as the specifiedIndex and hide others
    //        if (viewList.IndexOf(view) == index) view.SetActive(true);
    //        else view.SetActive(false);
    //    }
    //}

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Wait 3 seconds and move to Title View
    /// </summary>
    /// <returns></returns>
    //private IEnumerator ChangeViewWaitForSeconds()
    //{
    //    yield return new WaitForSeconds(3f);
    //    Test_ChangeView(1);
    //}

    #endregion Private Methods

    #endregion Methods
}
