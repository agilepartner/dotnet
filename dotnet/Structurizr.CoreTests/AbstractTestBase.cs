namespace Structurizr.CoreTests
{
    public abstract class AbstractTestBase
    {

        protected Workspace workspace;
        protected Model model;
        protected ViewSet views;

        public AbstractTestBase()
        {
            workspace = new Workspace("Name", "Description");
            model = workspace.Model;
            views = workspace.Views;
        }

    }
}
