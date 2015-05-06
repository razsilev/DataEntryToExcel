namespace LibrariesTranscription.Model
{
    using System.Text;

    public class LibraryItem
    {
        public int Id { get; set; }

        public string Organization { get; set; }

        public string Library { get; set; }

        public string Type { get; set; }

        public string City { get; set; }

        public string County { get; set; }

        public string MoreInfoUrl { get; set; }

        public string Name { get; set; }

        public string URL { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string ZipCode4 { get; set; }

        public string MailingAddressLine1 { get; set; }

        public string MailingAddressLine2 { get; set; }

        public string MailingCity { get; set; }

        public string MailingState { get; set; }

        public string MailingZipCode { get; set; }

        public string MailingZipCode4 { get; set; }

        public string Email { get; set; }

        public string LibraryPhoneNumber { get; set; }

        public string LibraryFaxNumber { get; set; }

        public string CourierCode { get; set; }

        public string MarmotCode { get; set; }

        public string OCLCCode { get; set; }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine(string.Format("Organization~{0}", this.Organization));
            result.AppendLine(string.Format("Library~{0}", this.Library));
            result.AppendLine(string.Format("Type~{0}", this.Type));
            result.AppendLine(string.Format("City~{0}", this.City));
            result.AppendLine(string.Format("County~{0}", this.County));
            result.AppendLine(string.Format("MoreInfoUrl~{0}", this.MoreInfoUrl));
            result.AppendLine(string.Format("Name~{0}", this.Name));
            result.AppendLine(string.Format("URL~{0}", this.URL));
            result.AppendLine(string.Format("AddressLine1~{0}", this.AddressLine1));
            result.AppendLine(string.Format("AddressLine2~{0}", this.AddressLine2));
            result.AppendLine(string.Format("State~{0}", this.State));
            result.AppendLine(string.Format("ZipCode~{0}", this.ZipCode));
            result.AppendLine(string.Format("ZipCode4~{0}", this.ZipCode4));
            result.AppendLine(string.Format("MailingAddressLine1~{0}", this.MailingAddressLine1));
            result.AppendLine(string.Format("MailingAddressLine2~{0}", this.MailingAddressLine2));
            result.AppendLine(string.Format("MailingCity~{0}", this.MailingCity));
            result.AppendLine(string.Format("MailingState~{0}", this.MailingState));
            result.AppendLine(string.Format("MailingZipCode~{0}", this.MailingZipCode));
            result.AppendLine(string.Format("MailingZipCode4~{0}", this.MailingZipCode4));
            result.AppendLine(string.Format("Email~{0}", this.Email));
            result.AppendLine(string.Format("LibraryPhoneNumber~{0}", this.LibraryPhoneNumber));
            result.AppendLine(string.Format("LibraryFaxNumber~{0}", this.LibraryFaxNumber));
            result.AppendLine(string.Format("CourierCode~{0}", this.CourierCode));
            result.AppendLine(string.Format("MarmotCode~{0}", this.MarmotCode));
            result.AppendLine(string.Format("OCLCCode~{0}", this.OCLCCode));

            result.AppendLine("****");

            return result.ToString();
        }
    }
}
