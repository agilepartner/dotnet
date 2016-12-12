using Structurizr.Client;

namespace Structurizr.Examples
{
    class GettingStarted
    {

        static void Main(string[] args)
        {
            Workspace workspace = new Workspace("My model", "This is a model of my software system.");
            Model model = workspace.Model;

            Person user = model.AddPerson("User", "A user of my software system.");
            SoftwareSystem softwareSystem = model.AddSoftwareSystem("Software System", "My software system.");
            user.Uses(softwareSystem, "Uses");

            ViewSet viewSet = workspace.Views;
            SystemContextView contextView = viewSet.CreateSystemContextView(softwareSystem, "context", "A simple example of a System Context diagram.");
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle(Tags.SoftwareSystem) { Background = "#1168bd", Color = "#ffffff" });
            styles.Add(new ElementStyle(Tags.Person) { Background = "#08427b", Color = "#ffffff" });

            StructurizrClient structurizrClient = new StructurizrClient("key", "secret");
            structurizrClient.PutWorkspace(1234, workspace);
        }

    }
}
