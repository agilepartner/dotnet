using Structurizr.IO.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Structurizr.Encryption
{

    [DataContract]
    public class EncryptedWorkspace : AbstractWorkspace
    {

        private Workspace workspace;
        public Workspace Workspace
        {
            get
            {
                if (this.workspace != null)
                {
                    return this.workspace;
                }
                else if (this.Ciphertext != null)
                {
                    this.Plaintext = EncryptionStrategy.Decrypt(Ciphertext);
                    StringReader stringReader = new StringReader(Plaintext);
                    return new JsonReader().Read(stringReader);
                }
                else {
                    return null;
                }
            }

            set
            {
                this.workspace = value;
            }
        }

        [DataMember(Name = "encryptionStrategy", EmitDefaultValue = false)]
        public EncryptionStrategy EncryptionStrategy { get; private set; }

        public string Plaintext { get; private set; }

        [DataMember(Name = "ciphertext", EmitDefaultValue = false)]
        public string Ciphertext { get; private set; }

        public EncryptedWorkspace() { }

        public EncryptedWorkspace(Workspace workspace, EncryptionStrategy encryptionStrategy)
        {
            this.Workspace = workspace;
            this.EncryptionStrategy = encryptionStrategy;

            StringWriter stringWriter = new StringWriter();
            JsonWriter jsonWriter = new JsonWriter(false);
            jsonWriter.Write(workspace, stringWriter);

            this.Id = workspace.Id;
            this.Name = workspace.Name;
            this.Description = workspace.Description;
            this.Thumbnail = workspace.Thumbnail;

            this.Plaintext = stringWriter.ToString();
            this.Ciphertext = encryptionStrategy.Encrypt(Plaintext);
        }

    }
}
