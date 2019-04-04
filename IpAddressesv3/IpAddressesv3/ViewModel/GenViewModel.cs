using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace IpAddressesv3
{
    public class GenViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// Instance of Generator class
        /// </summary>
        Generator Gen { get; set; } = new Generator();

        #endregion

        #region Public Proporties

        /// <summary>
        /// Ip Address
        /// </summary>
        public string IpAddress { get; set; } = "";

        /// <summary>
        /// Ip Address Binary
        /// </summary>
        public string IpAddressBinary { get; set; } = "";

        /// <summary>
        /// Prefix
        /// </summary>
        public string Prefix { get; set; } = "";

        /// <summary>
        /// SM
        /// </summary>
        public string SM { get; set; } = "";

        /// <summary>
        /// SM Binary
        /// </summary>
        public string SMBinary { get; set; } = "";

        /// <summary>
        /// Network Address
        /// </summary>
        public string NetworkAddress { get; set; } = "";

        /// <summary>
        /// Network Address Binary
        /// </summary>
        public string NetworkAddressBinary { get; set; } = "";

        /// <summary>
        /// Broadcast Address
        /// </summary>
        public string BroadcastAddress { get; set; } = "";

        /// <summary>
        /// Broadcast Address Binary
        /// </summary>
        public string BroadcastAddressBinary { get; set; } = "";

        /// <summary>
        /// Range of Host Addresses
        /// </summary>
        public string HostsRange { get; set; } = "";

        /// <summary>
        /// Number of available hosts
        /// </summary>
        public string HostsCount { get; set; } = "";

        /// <summary>
        /// Network Class
        /// </summary>
        public string NetworkClass { get; set; } = "";

        /// <summary>
        /// Warning text if prefix is > 30
        /// </summary>
        public string Warning { get; set; } = "";

        /// <summary>
        /// Number of SubNetworks user wants
        /// </summary>
        public int SubNetworks { get; set; } = 0;

        /// <summary>
        /// Number of available subhosts
        /// </summary>
        public string SubHostsCount { get; set; } = "";

        /// <summary>
        /// New prefix, counting subnetworks etc.
        /// </summary>
        public string NewPrefix { get; set; } = "";

        #endregion

        #region Commands

        /// <summary>
        /// Gets result of the addresses
        /// </summary>
        public ICommand GetResult { get; set; }

        /// <summary>
        /// Resets all the addresses
        /// </summary>
        public ICommand Reset { get; set; }

        #endregion

        #region Constructos

        /// <summary>
        /// Basic constructor
        /// </summary>
        public GenViewModel()
        {
            GetResult = new RelayCommand(GetResults);
            Reset = new RelayCommand(ResetVars);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Calculates the results of the addresses
        /// </summary>
        public void GetResults()
        {
            // If Ip Address is correct
            if (Gen.IsAddressCorrect(IpAddress))
            {
                // Binary IP Address
                IpAddressBinary = Gen.IP_2_Binary(IpAddress);
                // Network Class
                NetworkClass = Gen.GetNetworkClass(IpAddressBinary);
            }

            // If Prefix is correct
            if (Gen.IsPrefixCorrect(Prefix))
            {
                // SM Binary and Decimal
                SMBinary = Gen.Prefix_2_SM(Convert.ToInt16(Prefix));
                SM = Gen.BinAddress_2_Dec(SMBinary);

                // Number of Available Hosts
                HostsCount = Gen.GetAvailableHosts(Convert.ToInt32(Prefix)).ToString();

                // New Prefix
                NewPrefix = Gen.GetNewPrefix(SubNetworks, Convert.ToInt32(Prefix)).ToString();

                // Subhosts count
                SubHostsCount = Gen.GetAvailableHosts(Convert.ToInt32(NewPrefix)).ToString();
            }

            if (Gen.IsPrefixCorrect(Prefix) && Gen.IsAddressCorrect(IpAddress))
            {
                // Network Address Binary and Decimal
                NetworkAddressBinary = Gen.GetNetworkAddress(IpAddressBinary, SMBinary);
                NetworkAddress = Gen.BinAddress_2_Dec(NetworkAddressBinary);

                // Broadcast Address Binary and Decimal
                BroadcastAddressBinary = Gen.GetBroadcastNumber(IpAddressBinary, SMBinary);
                BroadcastAddress = Gen.BinAddress_2_Dec(BroadcastAddressBinary);

                HostsRange = Gen.GetHostsRange(NetworkAddress, BroadcastAddress);

                // Add Prefix to the addresss
                BroadcastAddress = AddPrefix(BroadcastAddress, NewPrefix);
                NetworkAddress = AddPrefix(NetworkAddress, NewPrefix);
            }    

            if (Prefix == "32")
                Warning = "Network doesn't exist!";
            else if (Prefix == "31")
                Warning = "Network has no hosts available!";
            else if (NetworkClass == "D" || NetworkClass == "E")
                Warning = "Restricted Address!";
            // TODO: Warning about 2many subnetworks
            else
                Warning = "";



        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Set's all addresses to ""
        /// </summary>
        private void ResetVars()
        {
            IpAddress = "";
            IpAddressBinary = "";
            Prefix = "";
            SM = "";
            SMBinary = "";
            NetworkAddress = "";
            NetworkAddressBinary = "";
            BroadcastAddress = "";
            BroadcastAddressBinary = "";
            HostsRange = "";
            HostsCount = "";
            NetworkClass = "";
            Warning = "";
            SubNetworks = 0;
            SubHostsCount = "";
        }

        /// <summary>
        /// Adds prefix to the address
        /// </summary>
        /// <param name="address">address</param>
        /// <param name="prefix">prefix to add at the end</param>
        /// <returns>string 0.0.0.0/0</returns>
        private string AddPrefix(string address, string prefix) => address + '/' + prefix;

        #endregion
    }
}
