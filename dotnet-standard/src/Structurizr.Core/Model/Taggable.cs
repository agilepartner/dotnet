using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Structurizr
{

    [DataContract]
    public abstract class Taggable
    {

        private List<string> tags = new List<string>();

        [DataMember(Name = "tags", EmitDefaultValue = false)]
        public string Tags
        {
            get
            {
                List<string> listOfTags = new List<string>(getRequiredTags());
                listOfTags.AddRange(tags);

                if (listOfTags.Count == 0)
                {
                    return "";
                }

                StringBuilder buf = new StringBuilder();
                foreach (string tag in listOfTags)
                {
                    buf.Append(tag);
                    buf.Append(",");
                }

                string tagsAsString = buf.ToString();
                return tagsAsString.Substring(0, tagsAsString.Length - 1);
            }

            set
            {
                if (value == null)
                {
                    return;
                }

                this.tags.Clear();
                this.tags.AddRange(value.Split(','));
            }
        }

        internal Taggable()
        {
        }

        public void AddTags(params string[] tags)
        {
            if (tags == null)
            {
                return;
            }

            foreach (string tag in tags)
            {
                if (tag != null)
                {
                    this.tags.Add(tag);
                }
            }
        }

        public void RemoveTag(string tag)
        {
            if (tag != null)
            {
                this.tags.Remove(tag);
            }
        }

        public abstract List<string> getRequiredTags();

    }
}
