// -----------------------------------------------------------------------
// <copyright file="DummyUsers.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace TestEngagementsService_WinForms.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Create necessary dummy users
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "*", Justification = "Test application")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.SpacingRules", "*", Justification = "Test application")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "*", Justification = "Test application")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.LayoutRules", "*", Justification = "Test application")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "*", Justification = "Test application")]
    public class DummyUsers
    {
        private List<string> usedUsers = new List<string>();

        public string[] DummyUserNames
        {
            get
            {
                return this.GetUserNames();
            }
        }

        public string GetNextUser()
        {
            string foundName = string.Empty;

            while (foundName == string.Empty)
            {
                Random r = new Random();
                int ind = r.Next(0, this.GetUserNames().Length - 1);
                string userName = this.GetUserNames()[ind];

                //(list1.Select(x => x.ID));

                var foundUsers = this.usedUsers.Find(item => item == userName);

                if (foundUsers == null)
                {
                    foundName = userName;
                    this.usedUsers.Add(userName);
                }
            }

            return (foundName);

        }

        /// <summary>
        /// Get the usernames
        /// </summary>
        /// <returns></returns>
        private string[] GetUserNames()
        {
            string[] uns = { "raulstearns", "reneedavis", "richardkucinich", "johnsmith", "mariopeters", "mikeneal", "rickhoyer", "gracemccarthy", "timschweikert", "jimnapolitano", "rondeutch", "cliffinslee", "earlmccollum", "stevepayne", "richardlewis", "shelleypaul", "connieschmidt", "billowens", "donnadoggett", "jasonamash", "andrerohrabacher", "keithroby", "debbiewestmoreland", "stenystivers", "luisjackson-lee", "devinhochul", "steveeshoo", "daleissa", "albiotierney", "marlinpoe", "ericshuster", "johnlangevin", "scottcole", "corrinehuelskamp", "briancourtney", "scottmccaul", "tomboswell", "kayjordan", "richdicks", "steveadams", "frankbutterfield", "dougengel", "samrigel", "leonardcuellar", "roblamborn", "steveboustany", "jaredcostello", "billybarton", "mikehinojosa", "philpingree", "normmckeon", "morgancoffman", "danahiggins", "jimhunter", "louienugent", "billschrader", "thaddeuslujan", "jackiefarr", "edshuler", "billslaughter", "genefleischmann", "russupton", "judywelch", "judycosta", "lucillemeeks", "hansennunes", "brianhall", "johnribble", "peterooney", "davelofgren", "nikibuchanan", "mikewaxman", "jerryflores", "franklatta", "mikewalden", "timscalise", "lamarfudge", "davidcastor", "davidreichert", "ileanathornberry", "jimgrimm", "frankwolf", "peterjohnson", "karenbartlett", "mazieheinrich", "stephenrunyan", "silvestregowdy", "michaelsutton", "robcapito", "jaimebrown", "jeffdent", "jimconaway", "samgingrey", "rosavisclosky", "billbaca", "maxineterry", "marcymurphy", "eddiaz-balart", "jeffturner", "georgeconyers", "blaineedwards", "darrellryan", "samlabrador", "nitagaramendi", "cedricsherman", "ronmckinley", "adamlungren", "terripence", "danculberson", "petereed", "petefilner", "johnbenishek", "joetiberi", "gerrycrenshaw", "donhultgren", "howardsimpson", "alceegranger", "kristipeterson", "jerrylucas", "charliehayworth", "jimcrowley", "bobpearce", "austinmchenry", "charliecanseco", "mikegibson", "joeroe", "emanuelschultz", "charlesbucshon", "donaldrichmond", "dianakelly", "johncapuano", "loiswatt", "marshalandry", "mikelarsen", "timnunnelee", "patharper", "johnkinzinger", "jocamp", "lindaelmers", "henryguthrie", "mikekind", "mikeross", "denniskissell", "tomhirono", "barbarakildee", "lorettabishop", "garypalazzo", "patrickgibbs", "annadesjarlais", "wallyhahn", "randysmith", "johnhastings", "michaelakin", "collinlehtinen", "glennpelosi", "billduncan", "tomclarke", "tammywilson", "stevefrelinghuysen", "mikebecerra", "timluetkemeyer", "adrianmcgovern", "jamesschwartz", "johnpetri", "tomclyburn", "alscott", "stevewu", "tomcoble", "billprice", "toddbrady", "sanderdegette", "davidbiggert", "bobmack", "dennisdefazio", "timlatourette", "jerrylewis", "barneyalexander" };
            return uns;
        }

    }
}
