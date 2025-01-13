using System;
using System.Windows.Forms;
using Fiddler;
using Nethereum.Web3;
using Nethereum.Contracts;

namespace BlockchainFilter
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        public static void StartFiddler()
        {
            FiddlerApplication.AfterSessionComplete += FiddlerApplication_AfterSessionComplete;
            FiddlerApplication.Startup(8888, FiddlerCoreStartupFlags.Default);
        }

        public static void StopFiddler()
        {
            FiddlerApplication.Shutdown();
        }

        private static void FiddlerApplication_AfterSessionComplete(Session oSession)
        {
            // Intercept HTTP requests here
            string host = oSession.oRequest.host.ToLower();
            CheckStatusCode(oSession, host);
        }

        private static async void CheckStatusCode(Session oSession, string host)
        {
            var web3 = new Web3("https://mainnet.infura.io/v3/YOUR_INFURA_PROJECT_ID");
            var contractAddress = "YOUR_CONTRACT_ADDRESS";
            var abi = @"[
                {
                    'constant': true,
                    'inputs': [
                        {
                            'name': '_website',
                            'type': 'string'
                        }
                    ],
                    'name': 'getRules',
                    'outputs': [
                        {
                            'name': '',
                            'type': 'uint256'
                        }
                    ],
                    'payable': false,
                    'stateMutability': 'view',
                    'type': 'function'
                }
            ]";
            var contract = web3.Eth.GetContract(abi, contractAddress);
            var getRulesFunction = contract.GetFunction("getRules");
            var statusCode = await getRulesFunction.CallAsync<int>(host);

            if (statusCode == 1)
            {
                InjectHtml(oSession);
            }
        }

        private static void InjectHtml(Session oSession)
        {
            oSession.utilDecodeResponse();
            oSession.utilReplaceInResponse("<body>", "<body><div style='color:red; font-size:24px;'>This website is closed</div>");
        }
    }
}
