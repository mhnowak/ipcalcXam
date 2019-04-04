using System;

namespace IpAddressesv3
{
    public class Generator
    {
        #region Decimal to Binary Public Methods

        /// <summary>
        /// Converts IP to binary
        /// </summary>
        /// <param name="ip">ip to convert</param>
        /// <returns>binary ip string</returns>
        public string IP_2_Binary(string ip)
        {
            // Split where "." is
            string[] splitted = ip.Split('.');

            // Binary string to return
            string binIp = "";

            // For each "." in IP get 8 digit binary string value
            for (int i = 0; i < splitted.Length; i++)
            {
                // Convert each item to binary
                binIp += ToBinary(Convert.ToInt16(splitted[i]), 8);

                // Adds "." after the 8 bits
                binIp += ".";
            }

            // Remove unnecessary dot at the end of the string
            binIp = binIp.Remove(binIp.Length - 1);

            return binIp;
        }

        /// <summary>
        /// Converts prefix to SM
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public string Prefix_2_SM(int prefix)
        {
            // SM binary string
            string SM = "";

            // 32 times fill SM with 1's or 0's
            for (int i = 1; i <= 32; i++)
            {
                // until prefix is > 0 fill it with 1's
                if (prefix > 0)
                {
                    SM += "1";
                    prefix--;
                } // Fill it with 0's
                else
                    SM += "0";

                // Put an "." every 8th bit
                if (i % 8 == 0)
                    SM += ".";

            }

            // Remove unnecessary dot at the end of the string
            SM = SM.Remove(SM.Length - 1);

            return SM;
        }

        /// <summary>
        /// Takes binary Address and converts it into Decimal one
        /// </summary>
        /// <param name="binAddress"></param>
        /// <returns></returns>
        public string BinAddress_2_Dec(string binAddress)
        {
            // Decimal SM to return
            string decAddress = "";

            // Split every "."
            string[] splitted = binAddress.Split('.');

            // Temporary int to store converted value
            int temp;
            for (int i = 0; i < splitted.Length; i++)
            {
                temp = Convert.ToInt32(splitted[i], 2);
                decAddress += temp.ToString() + ".";
            }

            // Remove unnecessary dot at the end of the string
            decAddress = decAddress.Remove(decAddress.Length - 1);

            return decAddress;
        }

        #endregion

        #region Get Public Methods

        /// <summary>
        /// Gets Network Binary Address
        /// </summary>
        /// <param name="ip">Binary Address</param>
        /// <param name="sm">Binary Address</param>
        /// <returns>returns broadcast binary number</returns>
        public string GetNetworkAddress(string ip, string sm)
        {
            // broadcast binary number to return
            string network = "";

            // splitted ip and sm addresses
            string[] ipSplit = ip.Split('.');
            string[] smSplit = sm.Split('.');

            for (int i = 0; i < ipSplit.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    // &
                    if (ipSplit[i][j] == '1' && smSplit[i][j] == '1')
                        network += "1";
                    else
                        network += "0";
                }

                // Adds dot after 8th bit
                network += ".";
            }

            // Remove unnecessary dot at the end of the string
            network = network.Remove(network.Length - 1);

            return network;
        }

        /// <summary>
        /// Gets Broadcast Binary Address
        /// </summary>
        /// <param name="ip">Binary Address</param>
        /// <param name="sm">Binary Address</param>
        /// <returns></returns>
        public string GetBroadcastNumber(string ip, string sm)
        {
            // broadcast binary number to return
            string broadcast = "";

            // splitted ip and sm addresses
            string[] ipSplit = ip.Split('.');
            string[] smSplit = sm.Split('.');

            for (int i = 0; i < ipSplit.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    // &
                    if (ipSplit[i][j] == '1' || smSplit[i][j] == '0')
                        broadcast += "1";
                    else
                        broadcast += "0";
                }

                // Adds dot after 8th bit
                broadcast += ".";
            }

            // Remove unnecessary dot at the end of the string
            broadcast = broadcast.Remove(broadcast.Length - 1);

            return broadcast;
        }

        /// <summary>
        /// Gets host range
        /// </summary>
        /// <param name="networkAddres">Decimal network address</param>
        /// <param name="broadcastAddress">Decimal broadcast address</param>
        /// <returns>Range of the available hosts in network (string)</returns>
        public string GetHostsRange(string networkAddres, string broadcastAddress)
        {
            // split addresses strings into string numbers
            string[] netSplit = networkAddres.Split('.');
            string[] broadSplit = broadcastAddress.Split('.');

            // Convert last numbers into int
            int net = Convert.ToInt32(netSplit[3]);
            int broad = Convert.ToInt32(broadSplit[3]);

            // ++ and -- them
            net++;
            broad--;

            // Convert them back to string
            netSplit[3] = net.ToString();
            broadSplit[3] = broad.ToString();

            // result string to return
            string result = "";

            // Convert array back to 1 solid string
            for (int i = 0; i < 4; i++)
                result += netSplit[i] + '.';

            // Remove unnecessary dot
            result = result.Remove(result.Length - 1);

            // Add " - " in between
            result += " - ";

            // Convert array back to 1 solid string
            for (int i = 0; i < 4; i++)
                result += broadSplit[i] + '.';

            // Remove unnecessary dot
            result = result.Remove(result.Length - 1);

            return result;
        }

        /// <summary>
        /// Calculates available hosts
        /// </summary>
        /// <param name="prefix">prefix 0 - 32</param>
        /// <returns>Double number of hosts</returns>
        public double GetAvailableHosts(int prefix) => Math.Pow(2, 32 - prefix) - 2;

        /// <summary>
        /// Gets in count subnetworks for the prefix
        /// </summary>
        /// <param name="subNets">Number of subnetworks you want</param>
        /// <param name="prefix">Just prefix</param>
        /// <returns>returns new prefix</returns>
        public int GetNewPrefix(int subNets, int prefix)
        {
            // Calculates the exponent of the hosts
            int expo = GetClosestExponent(subNets);

            // Returns new prefix
            return expo + prefix;
        }

        /// <summary>
        /// Returns class of the network
        /// </summary>
        /// <param name="ip">Ip Binary</param>
        /// <returns>Network Class (A / B / C / D / E )</returns>
        public string GetNetworkClass(string ip) => ip[0] == '0' ? "A" : ip[1] == '0' ? "B" : ip[2] == '0' ? "C" : ip[3] == '0' ? "D" : "E";


        #endregion

        #region Public Bools

        /// <summary>
        /// Checks if the address is correctly spelled
        /// </summary>
        /// <param name="address">Decimal Address</param>
        /// <returns>False if you can't produce anything from this address or true if it's correct</returns>
        public bool IsAddressCorrect(string address)
        {
            // Splits address every '.'
            string[] splitted = address.Split('.');

            // Checks if address has 3 dots
            if (splitted.Length != 4)
                return false;

            int helper;
            for (int i = 0; i < 4; i++)
            {
                // Try Convert string to number if fails return false
                try { helper = Convert.ToInt32(splitted[i]); }
                catch { return false; }

                // Checks if range is correct
                if (helper < 0 || helper > 255)
                    return false;
            }

            // If all corrects returns true
            return true;
        }

        /// <summary>
        /// Checks if the binary address is correctly spelled
        /// </summary>
        /// <param name="address">Binary Address</param>
        /// <returns>False if you can't produce anything from this address or true if it's correct</returns>
        public bool IsBinaryAddressCorrect(string address)
        {
            // Splits address every '.'
            string[] splitted = address.Split('.');

            // Checks if address has 3 dots
            if (splitted.Length != 4)
                return false;

            int helper;
            for (int i = 0; i < 4; i++)
            {
                // Try Convert string to number if fails return false
                try { helper = Convert.ToInt32(splitted[i]); }
                catch { return false; }

                // Checks if range is correct
                if (helper < 0 || helper > 255)
                    return false;

                // Checks if it's binary
                for (int j = 0; j < splitted[i].Length; j++)
                {
                    if (splitted[i][j] != '0' || splitted[i][j] != '1')
                        return false;
                }
            }

            // If all corrects returns true
            return true;
        }

        /// <summary>
        /// Determinates if prefix is in range 
        /// </summary>
        /// <param name="prefix">Intiger prefix</param>
        /// <returns>True or false if prefix is in range</returns>
        public bool IsPrefixCorrect(string prefix)
        {
            int pref;

            // If prefix is convertable
            try { pref = Convert.ToInt32(prefix); }
            catch { return false; }

            // If prefix is in range
            return pref < 0 || pref > 32 ? false : true;
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Gets closest 2^exponent to the given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>Closest exponent to the given number</returns>
        private int GetClosestExponent(int number)
        {
            // count and power
            int i = 0, power = 1;

            // multiple until i is bigger or equal
            while (power < number)
            {
                power *= 2;
                i++;
            }

            // returns closest exponent to the given number
            return i;
        }

        /// <summary>
        /// Converts Intiger value to binary
        /// </summary>
        /// <param name="numb">Just number to convert</param>
        /// <returns>8 digit binary string</returns>
        private string ToBinary(int numb, int bits)
        {
            // Empty binary string to return
            string binString = "";

            // divie by 2 until you have 8 digit number string
            for (int i = 0; i < bits; i++)
            {
                if (numb % 2 == 1)
                    binString = "1" + binString;
                else
                    binString = "0" + binString;

                numb /= 2;
            }

            return binString;
        }

        #endregion
    }
}
