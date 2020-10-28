namespace RunAsUser
{
    using NDesk.Options;
    using System;
    using System.Diagnostics;
    using System.Security;

    class Program
    {
        public static RAUObject rauObj { get; set; }

        /// <summary>
        /// Main - This is the method that is executed when the application is run
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                // If the argument count is greater than 0 we check the arguments
                // If -h or --help is supplied as an argument we will not enter this if condition below
                if (CheckArguments(args))
                {
                    // We are not testing the credentials or whether the file exists - this just simply attempts to run the program specified with the given credentials
                    StartProcess(rauObj.username, rauObj.password.ToSecureString(), rauObj.filename, rauObj.arguments);
                }
            }
            else
            {
                // Let the user supply the arguments manually
                GetArguments();
                // Start the process with the arguments you have submitted
                StartProcess(rauObj.username, rauObj.password.ToSecureString(), rauObj.filename, rauObj.arguments);
            }
        }

        /// <summary>
        /// ShowHelp will display the options if you choose to run this via command line arguments
        /// </summary>
        /// <param name="p"></param>
        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine();
            Console.WriteLine("Example: RunAsUser.exe -u <username> -p <password> -f <file> -a <arguments>");
            Console.WriteLine(@"Example: RunAsUser.exe -u administrator -p MySecurePassword1 -f c:\users\public\nc.exe -a '10.10.14.10 443 -e cmd.exe'");
            Console.WriteLine();
            Console.WriteLine("If no options are passed then you will need to have input your options in a wizard type approach");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }

        /// <summary>
        /// CheckArguments is used to check the arguments supplied and ultimately populate an instance of rauObject
        /// </summary>
        /// <param name="args"></param>
        public static bool CheckArguments(string[] args)
        {
            // TODO: add some error handling
            bool show_help = false;
            rauObj = new RAUObject();

            var p = new OptionSet()
            {
                { "u|username=",
                    "username",
                   v => rauObj.username = v
                },
                { "p|password=",
                   "password",
                    v => rauObj.password = v
                },
                { "f|file=",
                   "file such as cmd.exe",
                    v => rauObj.filename = v
                },
                { "a|arguments=",
                   "specifiy arguments to run (not mandatory)",
                    v => rauObj.arguments = v
                },
                { "h|help",  "show the help menu",
                   v => show_help = v != null
                },
            };

            p.Parse(args);

            // Basic check whether we are ok to proceed. If -h or --help is not supplied we will attempt to execute with the given the arguments
            if (show_help)
            {
                ShowHelp(p);
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// GetArguments will display questions on the command line and allow users to enter the answers. Kind of like a wizard approach
        /// </summary>
        /// <returns></returns>
        static RAUObject GetArguments()
        {
            rauObj = new RAUObject();

            Console.Write("Enter the username you wish to run as: ");
            rauObj.username = Console.ReadLine();
            Console.Write("Enter the password for the above user: ");
            rauObj.password = Console.ReadLine();
            Console.Write("Enter the program you wish to run: ");
            rauObj.filename = Console.ReadLine();
            Console.Write("Enter the program arguments (if any): ");
            rauObj.arguments = Console.ReadLine();

            return rauObj;
        }

        static void StartProcess(string user, SecureString password, string filename, string arguments)
        {
            // No checks completed on whether credentials work or if file exists and the validation of the arguments
            // Possibly build into next release. For now this is just a quick tool
            // Same goes for error handling - generic catching of exceptions and showing the exception message
            try
            {
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.UserName = user;
                p.StartInfo.Password = password;
                p.StartInfo.Arguments = arguments;
                p.StartInfo.FileName = filename;
                p.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    /// <summary>
    /// RAUObject is the object where we hold the arguments supplied
    /// </summary>
    internal class RAUObject
    {
        internal string username { get; set; }
        internal string password { get; set; }
        internal string filename { get; set; }
        internal string arguments { get; set; }
    }

    /// <summary>
    /// Extensions class to hold extension methods
    /// </summary>
    static class Extensions
    {
        /// <summary>
        /// Extension method to covert string to SecureString
        /// </summary>
        /// <param name="plainString"></param>
        /// <returns></returns>
        public static SecureString ToSecureString(this string plainString)
        {
            if (plainString == null)
                return null;

            SecureString secureString = new SecureString();
            foreach (char c in plainString.ToCharArray())
            {
                secureString.AppendChar(c);
            }
            return secureString;
        }
    }
}
