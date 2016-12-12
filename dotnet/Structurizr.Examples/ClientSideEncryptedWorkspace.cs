using Structurizr.Client;
using Structurizr.Encryption;

namespace Structurizr.Examples
{

    /// <summary>
    /// This is an example client-side encrypted workspace. You can see it in your web browser
    /// at https://structurizr.com/public/41 (the passphrase is "password").
    /// </summary>
    class ClientSideEncryptedWorkspace
    {

        static void Main(string[] args)
        {
            Workspace workspace = new Workspace("Client-side encrypted workspace", "This is a client-side encrypted workspace. The passphrase is 'password'.");
            Model model = workspace.Model;

            Person user = model.AddPerson("User", "A user of my software system.");
            SoftwareSystem softwareSystem = model.AddSoftwareSystem("Software System", "My software system.");
            user.Uses(softwareSystem, "Uses");

            ViewSet viewSet = workspace.Views;
            SystemContextView contextView = viewSet.CreateSystemContextView(softwareSystem, "context", "A simple example of a client-side encrypted workspace.");
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle(Tags.SoftwareSystem) { Background = "#d34407", Color = "#ffffff" });
            styles.Add(new ElementStyle(Tags.Person) { Background = "#f86628", Color = "#ffffff" });

            StructurizrClient structurizrClient = new StructurizrClient("key", "secret");
            structurizrClient.EncryptionStrategy = new AesEncryptionStrategy("password");
            structurizrClient.PutWorkspace(41, workspace);
        }

    }
}
