using Structurizr.Client;

namespace Structurizr.Examples
{
    class RemoteWorkspace
    {

        static void Main(string[] args)
        {
            Workspace workspace = new Workspace("Remote workspace", "A test remote workspace");
            workspace.Source = "https://gist.githubusercontent.com/simonbrowndotje/3bc87b89b82358c74f96470edc7519ad/raw/642ebcffb84e0c6ae612853919cf78b181bb65fe/structurizr-techtribesje.json";

            StructurizrClient structurizrClient = new StructurizrClient("key", "secret");
            structurizrClient.PutWorkspace(1234, workspace);
        }

    }
}
