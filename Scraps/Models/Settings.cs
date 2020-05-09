using System.Xml;
using System.Xml.Serialization;

namespace Scraps.Models
{
    public class Settings
    {
        [XmlAnyElement("CookieInfo")]
        public XmlComment CookieInfo { get; set; } = new XmlDocument().CreateComment("This cookie allows the bot to log in as you on Scrap.TF. This cookie will so long as *you* are still logged in. If you log out then you will need to log in again and change this value. Also, do not give this cookie to anyone!");
        public string Cookie { get; set; }

        public bool SeenWarning { get; set; } = false;
    }
}
