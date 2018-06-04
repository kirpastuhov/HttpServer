using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace HttpServer
{

    [DataContract]
    public class Books
    {
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "Annotation")]
        public string Annotation { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "Author")]
        public string Author { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "ISBN")]
        public string ISBN { get; set; }

        //public DateTime PublicationDate;
        [DataMember]
        [JsonProperty(PropertyName = "PublicationDate")]
        public string PublicationDate { get; set; }


        private string ToString(bool annotation = true)
        {

            var stringBuilder = new StringBuilder();

            stringBuilder.Append("Title: ").Append(Title).Append('\n');
            stringBuilder.Append("Author: ").Append(Author).Append('\n');
            stringBuilder.Append("ISBN: ").Append(ISBN).Append('\n');
            //stringBuilder.Append("PublicationDate: ").Append(PublicationDate.ToString("d-m-yyyy")).Append('\n');
            stringBuilder.Append("PublicationDate:").Append(PublicationDate).Append('\n');

            if (annotation)
            {
                stringBuilder.Append("Annotation: ").Append(Annotation).Append('\n');
            }

            return stringBuilder.ToString();
        }

        public override string ToString() => ToString();
    }
}
