namespace SpaceShooter.Menu
{
    public class SceneLoaderPresenter
    {
        private readonly SceneLoaderModel _model;

        public SceneLoaderPresenter(SceneLoaderModel model)
        {
            _model = model;
        }

        public void OnPlayClicked()
        {
            _model.LoadGameScene();
        }
    }
}


